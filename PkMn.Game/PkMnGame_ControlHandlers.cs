using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PkMn.Game.Controls;
using PkMn.Instance;
using PkMn.Model;
using PkMn.Model.Enums;

namespace PkMn.Game
{
    public partial class PkMnGame
    {
        private void LoadControls()
        {
            battleMenu = new HorizontalMenu(this, 290, GraphicsDevice.Viewport.Height - 202, GraphicsDevice.Viewport.Width - 302, 192);
            battleMenu.AddMenuItem("FIGHT");
            battleMenu.AddMenuItem("PkMn");
            battleMenu.AddMenuItem("ITEM");
            battleMenu.AddMenuItem("RUN");
            battleMenu.OnSubmit += battleMenu_OnSubmit;
            hasFocus = battleMenu;

            moveMenu = new Menu(this, 142, GraphicsDevice.Viewport.Height - 202, GraphicsDevice.Viewport.Width - 154, 192);
            moveMenu.OnSubmit += moveMenu_OnSubmit;
            moveMenu.OnCancel += moveMenu_OnCancel;
            moveMenu.OnSelectedItemChanged += moveMenu_OnSelectedItemChanged;

            partyMenu = new PartyMenu(this, 0, 4, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height - 206);
            partyMenu.OnSubmit += partyMenu_OnSubmit;
            partyMenu.OnCancel += partyMenu_OnCancel;

            moveDisplay = new MoveDisplay(this, 12, GraphicsDevice.Viewport.Height - 318, 320, 128);

            textBox = new ScrollingTextBox(this, 12, GraphicsDevice.Viewport.Height - 202, GraphicsDevice.Viewport.Width - 24, 192);
            textBox.LoadContent(GraphicsDevice);
            textBox.OnClose += TextBox_OnClose;

            foeHUD = new HUD(this, 0, 4, GraphicsDevice.Viewport.Width, (GraphicsDevice.Viewport.Height - 202) / 2 - 4, false);
            foeHUD.hudRect = new Rectangle(foeHUD.X + 44, foeHUD.Y + SpriteManager.CharHeight * 2, SpriteManager.HudFoe.Width, SpriteManager.HudFoe.Height);
            foeHUD.hudSprite = SpriteManager.HudFoe;
            foeHUD.nameCoords = new Vector2(foeHUD.X + SpriteManager.CharWidth, foeHUD.Y + 0);
            foeHUD.statusCoords = new Vector2(foeHUD.X + SpriteManager.CharWidth, foeHUD.Y + SpriteManager.CharHeight);
            foeHUD.hpBarCoords = new Vector2(foeHUD.X + 128, foeHUD.Y + SpriteManager.CharHeight * 2 + 12);
            foeHUD.drawHPText = false;
            foeHUD.OnAnimationEnd += foeHUD_OnAnimationEnd;

            selfHUD = new HUD(this, 0, foeHUD.Height + 2, GraphicsDevice.Viewport.Width, (GraphicsDevice.Viewport.Height - 182) / 2, true);
            selfHUD.hudRect = new Rectangle(selfHUD.X + selfHUD.Width - SpriteManager.HudSelf.Width - SpriteManager.CharWidth - 16, selfHUD.Y + selfHUD.Height - SpriteManager.HudSelf.Height - 20, SpriteManager.HudSelf.Width, SpriteManager.HudSelf.Height);
            selfHUD.hudSprite = SpriteManager.HudSelf;
            selfHUD.nameCoords = new Vector2(selfHUD.X + selfHUD.Width - (SpriteManager.CharWidth * 10) - 4, selfHUD.hudRect.Y - SpriteManager.CharHeight * 2);
            selfHUD.statusCoords = new Vector2(selfHUD.X + selfHUD.Width - (SpriteManager.CharWidth * 10) - 4, selfHUD.hudRect.Y - SpriteManager.CharHeight);
            selfHUD.hpBarCoords = new Vector2(selfHUD.hudRect.X + 96, selfHUD.hudRect.Y + 12);
            selfHUD.hpTextCoords = new Vector2(selfHUD.X + selfHUD.Width - (SpriteManager.CharWidth * 10) + - 4, selfHUD.hudRect.Y + SpriteManager.CharHeight);
            selfHUD.drawHPText = true;
            selfHUD.OnAnimationEnd += selfHUD_OnAnimationEnd;

            specialEffectManager = new SpecialEffectManager(this);
            specialEffectManager.OnEffectsEnd += specialEffectManager_OnEffectsEnd;
            
            foeRenderer = new FoeRenderer(this, foeHUD.Bounds.X, foeHUD.Bounds.Y + 2, foeHUD.Bounds.Width, foeHUD.Bounds.Height);
            foeRenderer.OnTransformMonster += specialEffectManager.TransformMonster;

            selfRenderer = new SelfRenderer(this, selfHUD.Bounds.X, selfHUD.Bounds.Y + 4, selfHUD.Bounds.Width, selfHUD.Bounds.Height);
            selfRenderer.OnTransformMonster += specialEffectManager.TransformMonster;

            foeExtraHUD = new ExtraHUD(this, WideWindowWidth / 2 + GameScreenWidth / 2, 0, (WideWindowWidth - GameScreenWidth) / 2, WideWindowHeight, false);
            selfExtraHUD = new ExtraHUD(this, 0, WideWindowHeight / 3, (WideWindowWidth - GameScreenWidth) / 2, WideWindowHeight * 2 / 3, true);

        }

        private void battleMenu_OnSubmit(object sender, EventArgs e)
        {
            switch (battleMenu.SelectedIndex)
            {
                case 0:
                    if (canSelectMove)
                    {
                        hasFocus = moveMenu;
                        int i = moveMenu.SelectedIndex;
                        moveMenu.Clear();
                        foreach (Move m in battle.PlayerCurrent.Moves)
                            if (m != null)
                                moveMenu.AddMenuItem(m.Name.ToUpper());
                        moveMenu.TrySelect(i);
                        moveMenu.Open();
                    }
                    else
                    {
                        selectedAction = new BattleAction(BattleActionType.UseMove);
                        syncronize.Set();
                    }
                    break;
                case 1:
                    hasFocus = partyMenu;
                    partyMenu.Open();
                    break;
                case 2:
                    hasFocus = textBox;
                    textBox.QueueText("Not implemented!");
                    break;
                case 3:
                    hasFocus = textBox;
                    textBox.QueueText("No! There's no running from a trainer battle!");
                    break;
            }
        }

        private void moveMenu_OnSubmit(object sender, EventArgs e)
        {
            moveDisplay.Hide();
            selectedAction = new BattleAction(BattleActionType.UseMove);
            selectedAction.WhichMove = moveMenu.SelectedIndex;
            syncronize.Set();
        }

        private void moveMenu_OnCancel(object sender, EventArgs e)
        {
            hasFocus = battleMenu;
            moveDisplay.Hide();
            battleMenu.Open();
        }

        private void moveMenu_OnSelectedItemChanged(object sender, EventArgs e)
        {
            if(!supressMoveDisplay)
                moveDisplay.Show(battle.PlayerCurrent.Moves[moveMenu.SelectedIndex].Type.Name.ToUpper(), battle.PlayerCurrent.CurrentPP[moveMenu.SelectedIndex], battle.PlayerCurrent.Moves[moveMenu.SelectedIndex].PP, battle.PlayerCurrent.DisabledCount > 0 && battle.PlayerCurrent.DisabledMoveIndex == moveMenu.SelectedIndex);
        }

        private void partyMenu_OnSubmit(object sender, EventArgs e)
        {
            selectedAction = new BattleAction(BattleActionType.ChangeMon, player.Party[partyMenu.SelectedIndex]);
            selectedMonster = player.Party[partyMenu.SelectedIndex];
            battleMenu.ResetSelected();
            syncronize.Set();
        }

        private void partyMenu_OnCancel(object sender, EventArgs e)
        {
            if (!shifting)
            {
                hasFocus = battleMenu;
                battleMenu.Open();
            }
            else
            {
                selectedMonster = null;
                syncronize.Set();
            }
        }

        private void TextBox_OnClose(object sender, EventArgs e)
        {
            syncronize.Set();
        }

        private void selfHUD_OnAnimationEnd(object sender, EventArgs e)
        {
            syncronize.Set();
        }

        private void foeHUD_OnAnimationEnd(object sender, EventArgs e)
        {
            syncronize.Set();
        }

        private void specialEffectManager_OnEffectsEnd(object sender, EventArgs e)
        {
            syncronize.Set();
        }
    }
}
