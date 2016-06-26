using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PkMn.Instance;
using PkMn.Model.Enums;

namespace PkMn.Game.Controls
{
    public class ExtraHUD : Panel
    {
        public EventHandler OnAnimationEnd;

        protected ActiveMonster monster;
        protected Monster lastMonster;
        protected int hpFrom;
        protected int hpTo;
        protected long frameCounter;
        protected int extraAnimationCounter;
        protected bool flip;

        protected Dictionary<string, StatType> stats = new Dictionary<string,StatType>()
        {
            {"Atk", StatType.Attack},
            {"Def", StatType.Defense},
            {"Spc", StatType.Special},
            {"Spe", StatType.Speed},
            {"Acc", StatType.Accuracy},
            {"Eva", StatType.Evade}
        };

        protected Dictionary<int, string> stages = new Dictionary<int, string>()
        {
            {6, SpecialCharacters.MultiplicationX + "4.0"},
            {5, SpecialCharacters.MultiplicationX + "3.5"},
            {4, SpecialCharacters.MultiplicationX + "3.0"},
            {3, SpecialCharacters.MultiplicationX + "2.5"},
            {2, SpecialCharacters.MultiplicationX + "2.0"},
            {1, SpecialCharacters.MultiplicationX + "1.5"},
            {0, SpecialCharacters.MultiplicationX + "1.0"},
            {-1, SpecialCharacters.MultiplicationX + ".66"},
            {-2, SpecialCharacters.MultiplicationX + ".50"},
            {-3, SpecialCharacters.MultiplicationX + ".40"},
            {-4, SpecialCharacters.MultiplicationX + ".33"},
            {-5, SpecialCharacters.MultiplicationX + ".28"},
            {-6, SpecialCharacters.MultiplicationX + ".25"}
        };

        public ExtraHUD(PkMnGame parent, int x, int y, int width, int height, bool flip)
            : base(parent, x, y, width, height)
        {
            this.flip = flip;
        }

        public void SetMonster(ActiveMonster monster)
        {
            this.monster = monster;
        }

        public override void Draw(GameTime gameTime, GraphicsDevice device, SpriteBatch spriteBatch)
        {
            if (monster == null || monster.Monster == null)
                return;

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("HP: {0,3} / {1,3}", monster.Monster.CurrentHP, monster.Monster.Stats.HP);
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine(monster.Type1.Name);
                if (monster.Type2 != null)
                    sb.AppendFormat("{0,13}", "/ " + monster.Type2.Name);

                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine("   Base      Eff");

                foreach (var pair in stats)
                {
                    if (pair.Value == StatType.Accuracy || pair.Value == StatType.Evade)
                        sb.AppendFormat("{0,3} 100", pair.Key);
                    else
                        sb.AppendFormat("{0,3} {1,3}", pair.Key, monster.Stats[pair.Value]);
                    sb.AppendLine();
                }


                spriteBatch.DrawString(SpriteManager.FontSmall, sb.ToString(), new Vector2( X + SpriteManager.CharWidthSm, Y + SpriteManager.CharHeightSm), Color.Black);

                int lineY = Y + SpriteManager.CharHeightSm * 7;
                foreach (var pair in stats)
                {
                    spriteBatch.DrawString(SpriteManager.FontSmall, string.Format("{0,5}", stages[monster.StatStages[pair.Value]]), new Vector2(X + SpriteManager.CharWidthSm * 8, lineY), monster.StatStages[pair.Value] > 0 ? Palettes.Lime : monster.StatStages[pair.Value] < 0 ? Palettes.Crimson : Color.Black);

                    int diff = monster.EffectiveStats[pair.Value] - (pair.Value == StatType.Accuracy || pair.Value == StatType.Evade ? 100 : monster.Stats[pair.Value]);
                    spriteBatch.DrawString(SpriteManager.FontSmall, string.Format("{0,4}", monster.EffectiveStats[pair.Value]), new Vector2(X + SpriteManager.CharWidthSm * 13, lineY), diff > 0 ? Palettes.Lime : diff < 0 ? Palettes.Crimson : Color.Black);
                    
                    lineY += SpriteManager.CharHeightSm;
                }

                spriteBatch.Draw(SpriteManager.HudSmall, new Rectangle(X + 4, Y + SpriteManager.CharHeightSm * 6 - 4, SpriteManager.HudSmall.Width, SpriteManager.HudSmall.Height), null, Color.Black, 0f, new Vector2(), flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);

                sb.Clear();
                if (monster.Monster.SleepCounter > 0)
                    sb.AppendFormat("Sleep{0,9}", monster.Monster.SleepCounter).AppendLine();
                if(monster.ConfusedCount > 0)
                    sb.AppendFormat("Confusion{0,5}", monster.ConfusedCount).AppendLine();
                if(monster.QueuedMoveLimit > 0)
                    sb.AppendFormat("Current Move{0,2}", monster.QueuedMoveLimit > 9 ? "Inf" : monster.QueuedMoveLimit.ToString()).AppendLine();
                if (monster.DisabledCount > 0)
                    sb.AppendFormat("Disabled{0,6}", monster.DisabledCount).AppendLine();

                if(sb.Length > 0)
                    spriteBatch.DrawString(SpriteManager.FontSmall, "Turns Left" + Environment.NewLine + sb.ToString(), new Vector2(X + SpriteManager.CharWidthSm, Y + SpriteManager.CharHeightSm * 16), Color.Black);

            }
            catch (NullReferenceException)
            {
            }
        }

    }
}
