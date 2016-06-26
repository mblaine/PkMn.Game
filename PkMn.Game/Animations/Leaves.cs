using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PkMn.Game.Controls;
using PkMn.Game.SpecialEffects;

namespace PkMn.Game.Animations
{
    public class Leaves : Animation
    {
        protected class Leaf
        {
            public readonly int StartX;
            public readonly int StartY;
            public readonly int StartFrame;
            public bool Visible;

            public Leaf(int startX, int startY, int startFrame)
            {
                StartX = startX;
                StartY = startY;
                StartFrame = startFrame;
                Visible = true;
            }

        }

        protected int spriteWidth = 288;
        protected int spriteHeight = 168;
        protected int frameHeightOffset = 144;
        protected int numberOfFrames = 18;
        protected int verticalCutoff = 418;
        protected List<Leaf> leaves;

        public Leaves(MonsterRenderer renderer, int delay, int param = 0, int startAt = 0, int stopEarly = int.MaxValue)
            : base(renderer, delay, param, startAt, stopEarly)
        {
            leaves = new List<Leaf>();
            leaves.Add(new Leaf(32, -24, 5));
            leaves.Add(new Leaf(144, -80, 16));
            leaves.Add(new Leaf(192, -128, 10));
        }

        public override bool IsOver(int frameCounter)
        {
            //if (frameCounter < delay * 51)
            if(leaves.Any(p => p.Visible))
                return false;
            return true;
        }

        public override void Draw(GameTime gameTime, GraphicsDevice device, SpriteBatch spriteBatch, int frameCounter)
        {
            Texture2D sprite = SpriteManager.EffectSprites["leaves"];

            int animationFrame = frameCounter / delay;

            foreach (Leaf leaf in leaves)
            {
                if (leaf.Visible)
                {
                    int currentFrame = leaf.StartFrame + animationFrame;
                    Rectangle srcRect = new Rectangle(0, (currentFrame % numberOfFrames) * spriteHeight, spriteWidth, spriteHeight);
                    Rectangle destRect = new Rectangle(leaf.StartX, leaf.StartY + frameHeightOffset * (currentFrame / numberOfFrames), spriteWidth, spriteHeight);
                    spriteBatch.Draw(sprite, destRect, srcRect, Color.White, 0f, new Vector2(), SpriteEffects.None, 0f);

                    if (destRect.Y + (currentFrame % numberOfFrames) * 8 + 32 >= verticalCutoff)
                        leaf.Visible = false;

                }
            }
        }
    }
}
