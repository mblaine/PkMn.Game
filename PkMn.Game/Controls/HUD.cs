using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PkMn.Game.Enums;
using PkMn.Instance;
using PkMn.Model.Enums;

namespace PkMn.Game.Controls
{
    public class HUD : Panel
    {
        public EventHandler OnAnimationEnd;

        public bool ShowParty;

        protected ActiveMonster monster;
        protected Monster lastMonster;
        protected int hpFrom;
        protected int hpTo;
        protected long frameCounter;
        protected int extraAnimationCounter;
        protected bool mirror;

        protected string statusText;

        internal Rectangle hudRect;
        internal Texture2D hudSprite;
        internal Vector2 nameCoords;
        internal Vector2 statusCoords;
        internal Vector2 hpBarCoords;
        internal Vector2 hpTextCoords;
        internal bool drawHPText;

        public HUD(PkMnGame parent, int x, int y, int width, int height, bool mirror)
            : base(parent, x, y, width, height)
        {
            extraAnimationCounter = -1;
            statusText = "";
            ShowParty = false;
            this.mirror = mirror;
        }

        public void SetMonster(ActiveMonster monster)
        {
            this.monster = monster;
        }

        public void UpdateStatus()
        {
            statusText = monster.Monster.StatusText;
        }

        public void AnimateHPChange(int hpFrom, int hpTo)
        {
            this.hpFrom = hpFrom;
            this.hpTo = hpTo;
            frameCounter = 0;
            extraAnimationCounter = -1;
            this.state = State.Animating;
        }

        public override void Update(GameTime gameTime)
        {
            frameCounter++;
            bool stop = false;
            if (state == State.Animating && frameCounter > Math.Abs(hpTo - hpFrom))
                stop = true;
            
            if (stop)
            {
                state = State.ReadyForNext;
                if (OnAnimationEnd != null)
                    OnAnimationEnd(this, null);
                extraAnimationCounter = 0;
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, GraphicsDevice device, SpriteBatch spriteBatch)
        {
            if (monster == null || monster.Monster == null)
                return;
            try
            {
                if (monster.Monster != lastMonster)
                {
                    lastMonster = monster.Monster;
                    UpdateStatus();
                }

                if (ShowParty)
                {
                    spriteBatch.Draw(SpriteManager.HudFrame, new Rectangle(hudRect.X, hudRect.Y + hudRect.Height - SpriteManager.HudFrame.Height, SpriteManager.HudFrame.Width, SpriteManager.HudFrame.Height), null, SpriteManager.HudColor, 0f, new Vector2(), mirror ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);

                    if (monster.Trainer.Party.Length <= 6)
                    {
                        int[] offsets = new int[6];
                        for (int i = 0; i < monster.Trainer.Party.Length; i++)
                        {
                            if (monster.Trainer.Party[i] == null)
                                offsets[i] = 3;
                            else if (monster.Trainer.Party[i].CurrentHP > 0)
                            {
                                if (monster.Trainer.Party[i].Status == StatusCondition.None)
                                    offsets[i] = 0;
                                else
                                    offsets[i] = 1;
                            }
                            else
                                offsets[i] = 2;
                        }

                        for (int i = monster.Trainer.Party.Length; i < 6; i++)
                            offsets[i] = 3;

                        Rectangle drawTo = new Rectangle(hudRect.X + (mirror ? 64 : 52), hudRect.Y + hudRect.Height - 52, 32, 32);

                        if (mirror)
                            offsets = offsets.Reverse().ToArray();

                        for (int i = offsets.Length - 1; i >= 0; i--)
                        {
                            spriteBatch.Draw(SpriteManager.Balls, drawTo, new Rectangle(offsets[i] * 32, 0, 32, 32), Color.White);
                            drawTo.Offset(32, 0);
                        }
                    }
                    else
                    {
                        int healthy = monster.Trainer.Party.Count(m => m.CurrentHP > 0 && m.Status == StatusCondition.None);
                        int unhealthy = monster.Trainer.Party.Count(m => m.CurrentHP > 0 && m.Status != StatusCondition.None);
                        int fainted = monster.Trainer.Party.Count(m => m.CurrentHP <= 0);

                        string healthyStr = string.Format("{0}{1}", SpecialCharacters.MultiplicationX, healthy);
                        string unhealthyStr = string.Format("{0}{1}", SpecialCharacters.MultiplicationX, unhealthy);
                        string faintedStr = string.Format("{0}{1}", SpecialCharacters.MultiplicationX, fainted);

                        int totalLength = 72 + (int)SpriteManager.Font.MeasureString(healthyStr + faintedStr + (unhealthy > 0 ? unhealthyStr : "")).X + (unhealthy > 0 ? 40 : 0);

                        Rectangle drawTo = new Rectangle(hudRect.X + (mirror ? hudRect.Width - totalLength - 12 : 32), hudRect.Y + hudRect.Height - 52, 32, 32);

                        spriteBatch.Draw(SpriteManager.Balls, drawTo, new Rectangle(0, 0, 32, 32), Color.White);
                        drawTo.Offset(32, 0);
                        spriteBatch.DrawString(SpriteManager.Font, healthyStr, new Vector2(drawTo.X, drawTo.Y), SpriteManager.HudColor);
                        drawTo.Offset((int)SpriteManager.Font.MeasureString(healthyStr).X + 8, 0);
                        
                        if (unhealthy > 0)
                        {
                            spriteBatch.Draw(SpriteManager.Balls, drawTo, new Rectangle(32, 0, 32, 32), Color.White);
                            drawTo.Offset(32, 0);
                            spriteBatch.DrawString(SpriteManager.Font, unhealthyStr, new Vector2(drawTo.X, drawTo.Y), SpriteManager.HudColor);
                            drawTo.Offset((int)SpriteManager.Font.MeasureString(unhealthyStr).X + 8, 0);
                        }

                        spriteBatch.Draw(SpriteManager.Balls, drawTo, new Rectangle(64, 0, 32, 32), Color.White);
                        drawTo.Offset(32, 0);
                        spriteBatch.DrawString(SpriteManager.Font, faintedStr, new Vector2(drawTo.X, drawTo.Y), SpriteManager.HudColor);
                    }
                }
                else if (monster.Monster.Status != StatusCondition.Faint)
                {
                    spriteBatch.Draw(hudSprite, hudRect, SpriteManager.HudColor);
                    spriteBatch.DrawString(SpriteManager.Font, SpecialCharacters.ReplaceChars(monster.Monster.Name), nameCoords, SpriteManager.HudColor);

                    string line2 = "   " + SpecialCharacters.ColonL + monster.Monster.Level.ToString();
                    if (!string.IsNullOrWhiteSpace(statusText))
                        line2 = "   " + statusText;

                    spriteBatch.DrawString(SpriteManager.Font, line2, statusCoords, SpriteManager.HudColor);

                    decimal hpPercent = ((decimal)monster.Monster.CurrentHP) / monster.Monster.Stats.HP;
                    int hpToDraw = monster.Monster.CurrentHP;

                    if (state == State.Animating || extraAnimationCounter >= 0)
                    {
                        int change = hpFrom > hpTo ? (int)-frameCounter : (int)frameCounter;
                        if (Math.Abs(hpTo - hpFrom) < Math.Abs(change))
                            change = hpTo - hpFrom;
                        hpPercent = ((decimal)hpFrom + change) / monster.Monster.Stats.HP;
                        hpToDraw = hpFrom + change;
                    }

                    if (state != State.Animating && extraAnimationCounter >= 0)
                        extraAnimationCounter++;
                    if (extraAnimationCounter > 5)
                        extraAnimationCounter = -1;

                    Color color = hpPercent > 0.5m ? SpriteManager.HPBarHighColor : hpPercent > 0.2m ? SpriteManager.HPBarMediumColor : SpriteManager.HPBarLowColor;

                    spriteBatch.Draw(SpriteManager.White, new Rectangle((int)hpBarCoords.X, (int)hpBarCoords.Y, (int)(192 * hpPercent), 8), color);

                    if (drawHPText)
                        spriteBatch.DrawString(SpriteManager.Font, string.Format("{0,4}/{1,3}", hpToDraw, monster.Monster.Stats.HP), hpTextCoords, SpriteManager.HudColor);
                }
            }
            catch (NullReferenceException)
            {
            }
        }

    }
}
