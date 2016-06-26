using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PkMn.Game.Controls;
using PkMn.Game.SpecialEffects;

namespace PkMn.Game.Animations
{
    public class Petals : Animation
    {
        protected class Petal
        {
            public readonly int StartX;
            public readonly int StartY;
            public readonly int StartFrame;
            public readonly int WhichColumn;
            public bool Visible;

            public Petal(int startX, int startY, int startFrame, int whichColumn)
            {
                StartX = startX;
                StartY = startY;
                StartFrame = startFrame;
                WhichColumn = whichColumn;
                Visible = true;
            }

        }

        protected int spriteWidth = 288;
        protected int spriteHeight = 168;
        protected int frameHeightOffset = 144;
        protected int numberOfFrames = 18;
        protected int verticalCutoff = 418;
        protected List<Petal> petals;

        public Petals(MonsterRenderer renderer, int delay, int param = 0, int startAt = 0, int stopEarly = int.MaxValue)
            : base(renderer, delay, param, startAt, stopEarly)
        {
            petals = new List<Petal>();
            petals.Add(new Petal(32, -24, 5, 0));
            petals.Add(new Petal(144, -80, 16, 1));
            petals.Add(new Petal(100, 64, 2, 0));
            petals.Add(new Petal(400, 16, 12, 1));
            petals.Add(new Petal(512, 72, 9, 0));
            petals.Add(new Petal(540, 88, 11, 1));
            petals.Add(new Petal(92, 176, 4, 0));
            petals.Add(new Petal(280, 120, 15, 1));
            petals.Add(new Petal(240, 328, 1, 0));
            petals.Add(new Petal(192, -128, 10, 1));
        }

        public override bool UseMonDefaultPalette(MonsterRenderer renderer)
        {
            return renderer == this.renderer;
        }

        public override bool IsOver(int frameCounter)
        {
            //if (frameCounter < delay * 51)
            if(petals.Any(p => p.Visible))
                return false;
            return true;
        }

        public override void Draw(GameTime gameTime, GraphicsDevice device, SpriteBatch spriteBatch, int frameCounter)
        {
            Texture2D sprite = SpriteManager.EffectSprites["petals"];

            int animationFrame = frameCounter / delay;

            foreach (Petal petal in petals)
            {
                if (petal.Visible)
                {
                    int currentFrame = petal.StartFrame + animationFrame;
                    Rectangle srcRect = new Rectangle(petal.WhichColumn * spriteWidth, (currentFrame % numberOfFrames) * spriteHeight, spriteWidth, spriteHeight);
                    Rectangle destRect = new Rectangle(petal.StartX, petal.StartY + frameHeightOffset * (currentFrame / numberOfFrames), spriteWidth, spriteHeight);
                    spriteBatch.Draw(sprite, destRect, srcRect, Color.White, 0f, new Vector2(), SpriteEffects.None, 0f);

                    if (destRect.Y + (currentFrame % numberOfFrames) * 8 + 32 >= verticalCutoff)
                        petal.Visible = false;

                }
            }
        }
    }
}
