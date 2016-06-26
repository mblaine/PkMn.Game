using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using PkMn.Game.Controls;
using PkMn.Game.Enums;
using PkMn.Game.SpecialEffects;
using PkMn.Instance;
using PkMn.Model;
using PkMn.Model.Enums;

namespace PkMn.Game
{
    public partial class PkMnGame
    {
        private void BeginBattle()
        {
            if (File.Exists("parties.xml"))
            {
                Trainer.ReadFromFile("parties.xml", out player, out rival);
            }
            else
            {
                player = new Trainer()
                {
                    Name = "Ash",
                    MonNamePrefix = "",
                    Party = new Monster[6],
                    IsPlayer = true
                };

                rival = new Trainer()
                {
                    Name = "Gary",
                    MonNamePrefix = "Enemy ",
                    Party = new Monster[6],
                    IsPlayer = false
                };

                List<string> mons = Species.Spp.Select(p => p.Key).ToList();

                for (int i = 0; i < player.Party.Length; i++)
                {
                    player.Party[i] = new Monster(mons[Rng.Next(1, mons.Count)], 70, Generator.SimulatePlayer);
                    mons.Remove(player.Party[i].Species.Name);
                }

                mons = Species.Spp.Select(p => p.Key).ToList();

                for (int i = 0; i < rival.Party.Length; i++)
                {
                    rival.Party[i] = new Monster(mons[Rng.Next(1, mons.Count)], 70, Generator.Trainer);
                    mons.Remove(rival.Party[i].Species.Name);
                }
            }
            
            //player.Party[0] = new Monster("Raichu", 70, Generator.SimulatePlayer);
            //rival.Party[0] = new Monster("Metapod", 70);
            //rival.Party[0].CurrentHP = 1000;
            //player.Party[0].Moves[0] = Move.Moves["Peck"];
            //player.Party[0].Moves[1] = Move.Moves["Solar Beam"];
            //rival.Party[0].Moves[0] = Move.Moves["Agility"];
            //player.Party[2].Status = StatusCondition.Burn;
            //player.Party[0].Stats.Speed = 300;

            partyMenu.Clear();
            foreach (Monster m in player.Party)
                partyMenu.AddMenuItem(m);

            battle = new Battle(player, rival, false, true);
            battle.SendMessage += battle_SendMessage;
            battle.PlayerChooseAction += battle_ChooseAction;
            battle.PlayerChooseNextMon += battle_ChooseNextMon;
            battle.PlayerChooseMoveToMimic += battle_ChooseMoveToMimic;
            battle.BattleEvent += battle_BattleEvent;

            selfHUD.SetMonster(battle.PlayerCurrent);
            foeHUD.SetMonster(battle.FoeCurrent);

            selfRenderer.SetMonster(battle.PlayerCurrent);
            foeRenderer.SetMonster(battle.FoeCurrent);

            selfExtraHUD.SetMonster(battle.PlayerCurrent);
            foeExtraHUD.SetMonster(battle.FoeCurrent);

            syncronize = new ManualResetEvent(false);

            thread = new Thread(new ThreadStart(RunBattle));
            thread.Start();
        }

        private void RunBattle()
        {
            while (battle.Step()) ;// { battle.PlayerCurrent.EffectiveStats.Accuracy = 500; }

            //specialEffectManager.QueueSpecialEffects(Sequence.Sequences["Fade Out"], selfRenderer, foeRenderer);
            //hasFocus = specialEffectManager;
            //syncronize.Reset();
            //syncronize.WaitOne();
            //selfRenderer.Position = SpritePosition.Hidden;
            //foeRenderer.Position = SpritePosition.Hidden;
            //selfHUD.SetMonster(null);
            //foeHUD.SetMonster(null);
            //textBox.Clear();
            //specialEffectManager.QueueSpecialEffects(Sequence.Sequences["Fade In"], selfRenderer, foeRenderer);
            //hasFocus = specialEffectManager;
            //syncronize.Reset();
            //syncronize.WaitOne();
            //BeginBattle();

            running = false;
            foeHUD.ShowParty = true;
            selfHUD.ShowParty = true;
        }

        private static Regex[] staticPatterns = new Regex[]
        {
            new Regex(" used .+?!$"),
            new Regex(" sent out "),
            new Regex("^Go! "),
            new Regex("Come back!$"),
            new Regex("is fast asleep!$"),
            new Regex("is confused!$"),
            new Regex("attack continues!$"),
            new Regex("thrashing about!$"),
            new Regex("^Get'm!"),
            new Regex("^The enemy's weak"),
            new Regex("^Do it!")
        };

        private void battle_SendMessage(string text)
        {
            Panel previousFocus = hasFocus;
            hasFocus = textBox;
            if (staticPatterns.Any( p => p.IsMatch(text)))
                textBox.QueueStaticText(text);
            else
                textBox.QueueText(text);

            if (text.Contains("is about to use"))
                foeHUD.ShowParty = true;
            else if(text.Contains(" sent out "))
                foeHUD.ShowParty = false;

            syncronize.Reset();
            syncronize.WaitOne();
            hasFocus = previousFocus;
            
        }

        private BattleAction battle_ChooseAction(ActiveMonster current, Trainer trainer, bool canAttack)
        {
            selectedAction = null;
            canSelectMove = canAttack;
            Panel previousFocus = hasFocus;
            textBox.Clear();
            while (selectedAction == null)
            {
                hasFocus = battleMenu;
                battleMenu.Open();
                syncronize.Reset();
                syncronize.WaitOne();
            }
            
            if(selectedAction.Type == BattleActionType.ChangeMon)
                moveMenu.TrySelect(0);

            hasFocus = previousFocus;
            return selectedAction;
        }

        private Monster battle_ChooseNextMon(Trainer trainer, bool optional)
        {
            selectedMonster = null;
            Panel previousFocus = hasFocus;
            textBox.Clear();
            hasFocus = partyMenu;
            shifting = optional;
            while (selectedMonster == null)
            {
                partyMenu.Open(optional);
                syncronize.Reset();
                syncronize.WaitOne();
                
                if (optional)
                    break;
            }
            shifting = false;
            
            hasFocus = previousFocus;
            if(selectedMonster != null && selectedMonster != battle.PlayerCurrent.Monster)
                moveMenu.TrySelect(0);

            return selectedMonster;
        }

        private int battle_ChooseMoveToMimic(Move[] moves)
        {
            Panel previousFocus = hasFocus;
            moveMenu.Clear();
            textBox.Clear();
            foreach (Move m in moves)
                if (m != null)
                    moveMenu.AddMenuItem(m.Name.ToUpper());
            hasFocus = moveMenu;
            supressMoveDisplay = true;
            moveMenu.Open(false);
            syncronize.Reset();
            syncronize.WaitOne();
            supressMoveDisplay = false;
            hasFocus = previousFocus;

            return moveMenu.SelectedIndex;
        }

        private void battle_BattleEvent(object sender, BattleEventArgs e)
        {
            HUD display = e.Monster == battle.PlayerCurrent ? (HUD)selfHUD : (HUD)foeHUD;
            MonsterRenderer current = e.Monster == battle.PlayerCurrent ? (MonsterRenderer)selfRenderer : (MonsterRenderer)foeRenderer;
            MonsterRenderer opponent = current == selfRenderer ? (MonsterRenderer)foeRenderer : (MonsterRenderer)selfRenderer;
            Panel previousFocus = hasFocus;
            syncronize.Reset();

            if (e.Type == BattleEventType.MonHPChanged)
            {
                hasFocus = display;
                display.AnimateHPChange(e.HPBefore, e.HPAfter);
            }
            else if (e.Type == BattleEventType.MonFainted)
            {
                hasFocus = specialEffectManager;
                specialEffectManager.QueueSpecialEffects(Sequence.Sequences["Fainted"], current, opponent);
                
            }
            else if (e.Type == BattleEventType.MonRecalled)
            {
                hasFocus = specialEffectManager;
                specialEffectManager.QueueSpecialEffects(Sequence.Sequences["Recalled"], current, opponent);
            }
            else if (e.Type == BattleEventType.MonSentOut)
            {
                hasFocus = specialEffectManager;
                display.UpdateStatus();
                specialEffectManager.QueueSpecialEffects(Sequence.Sequences["Sent Out"], current, opponent);
            }
            else if (e.Type == BattleEventType.MonSpawned)
            {
                hasFocus = specialEffectManager;
                display.UpdateStatus();
                specialEffectManager.QueueSpecialEffects(Sequence.Sequences["Sent Out"], current, opponent);
            }
            else if (e.Type == BattleEventType.AttackHit || e.Type == BattleEventType.RecurringAttackHit)
            {
                hasFocus = specialEffectManager;
                specialEffectManager.QueueSpecialEffect(new Delay(30));

                if(e.Monster.SubstituteHP > 0 && e.Move.Name != "Substitute")
                    specialEffectManager.QueueSpecialEffects(Sequence.Sequences["Substitute Pre-Attack"], current, opponent);

                Sequence seq = null;
                if (e.Type == BattleEventType.RecurringAttackHit && Sequence.Sequences.ContainsKey(e.Move.Name + " Recurring"))
                    seq = Sequence.Sequences[e.Move.Name + " Recurring"];
                else if(Sequence.Sequences.ContainsKey(e.Move.Name))
                    seq = Sequence.Sequences[e.Move.Name];

                if(seq != null)
                    specialEffectManager.QueueSpecialEffects(seq, current, opponent);
                else
                    specialEffectManager.QueueSpecialEffect(new Delay(30));
                switch (e.Move.AttackType)
                {
                    case AttackType.Damaging:
                        specialEffectManager.QueueSpecialEffects(Sequence.Sequences[e.Monster == battle.PlayerCurrent ? "Player Damaging" : "Enemy Damaging"], current, opponent);
                        break;
                    case AttackType.DamagingWithEffectChance:
                        specialEffectManager.QueueSpecialEffects(Sequence.Sequences[e.Monster == battle.PlayerCurrent ? "Player Damaging With Effect Chance" : "Enemy Damaging With Effect Chance"], current, opponent);
                        break;
                    case AttackType.NonDamaging:
                        specialEffectManager.QueueSpecialEffects(Sequence.Sequences[e.Monster == battle.PlayerCurrent ? "Player Non-Damaging" : "Enemy Non-Damaging"], current, opponent);
                        break;
                    default:
                        specialEffectManager.QueueSpecialEffect(new Delay(20));
                        break;
                }

                if (e.Monster.SubstituteHP > 0 && e.Move.Name != "Substitute")
                    specialEffectManager.QueueSpecialEffects(Sequence.Sequences["Substitute Post-Attack"], current, opponent);
            }
            else if (e.Type == BattleEventType.AttackMissed)
            {
                hasFocus = specialEffectManager;
                specialEffectManager.QueueSpecialEffect(new TransformSprite(SpriteTransformation.Show, current));
                specialEffectManager.QueueSpecialEffect(new Delay(50));
            }
            else if (e.Type == BattleEventType.AttackCharged)
            {
                hasFocus = specialEffectManager;
                specialEffectManager.QueueSpecialEffect(new Delay(30));

                if (Sequence.Sequences.ContainsKey(e.Move.Name + " Charge"))
                    specialEffectManager.QueueSpecialEffects(Sequence.Sequences[e.Move.Name + " Charge"], current, opponent);
                else
                    specialEffectManager.QueueSpecialEffect(new Delay(30));
            }
            else if (e.Type == BattleEventType.StatusAilment)
            {
                hasFocus = specialEffectManager;
                if (e.Status == StatusCondition.Sleep)
                    specialEffectManager.QueueSpecialEffects(Sequence.Sequences["Asleep"], current, opponent);
                else if (e.Status == StatusCondition.Confusion)
                    specialEffectManager.QueueSpecialEffects(Sequence.Sequences["Confused"], current, opponent);
                else if (e.Status == StatusCondition.Seeded)
                    specialEffectManager.QueueSpecialEffects(Sequence.Sequences["Seeded"], current, opponent);
                else if (e.Status == StatusCondition.Burn)
                    specialEffectManager.QueueSpecialEffects(Sequence.Sequences["Burned"], current, opponent);
                else if (e.Status == StatusCondition.Poison || e.Status == StatusCondition.BadlyPoisoned)
                    specialEffectManager.QueueSpecialEffects(Sequence.Sequences["Poisoned"], current, opponent);
                else
                    throw new Exception();
            }
            else if (e.Type == BattleEventType.MonTransformed)
            {
                hasFocus = specialEffectManager;
                specialEffectManager.QueueSpecialEffect(new TransformSprite(SpriteTransformation.Show, current));
            }
            else if (e.Type == BattleEventType.SubstituteBroke)
            {
                hasFocus = specialEffectManager;
                display.UpdateStatus();
                specialEffectManager.QueueSpecialEffects(Sequence.Sequences["Substitute Broke"], current, opponent);
            }
            else if (e.Type == BattleEventType.MonCrashed)
            {
                hasFocus = specialEffectManager;
                specialEffectManager.QueueSpecialEffect(new TransformScreen(this, ScreenTransformation.ShakeHorizontallyFast, 4, false));
            }
            else if (e.Type == BattleEventType.MonHurtItself)
            {
                hasFocus = specialEffectManager;
                specialEffectManager.QueueSpecialEffect(new Animations.DoublePow(current, 8));
            }
            else if (e.Type == BattleEventType.MonStatusUpdate)
            {
                display.UpdateStatus();
            }
            else
                throw new Exception();

            if(e.Type != BattleEventType.MonStatusUpdate)
                syncronize.WaitOne();
            
            hasFocus = previousFocus;
        }
    }
}
