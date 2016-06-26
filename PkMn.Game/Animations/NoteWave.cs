using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PkMn.Game.Controls;
using PkMn.Game.SpecialEffects;

namespace PkMn.Game.Animations
{
    public class NoteWave : Animation
    {
        protected static int spriteWidth = 64;

        protected static Point startRight = new Point(506, 134);
        protected static Point startLeft = new Point(160, 304);

        protected static Point[] offsets = new Point[] 
        {
            new Point(-56, 88),
            new Point(-56, 40),
            new Point(-56, -32),
            new Point(-56, -32),
            new Point(-56, 40),
            new Point(-56, 88),
            new Point(-56, 0),
            new Point(-56, -32)
        };

        public NoteWave(MonsterRenderer renderer, int delay, int param = 0, int startAt = 0, int stopEarly = int.MaxValue)
            : base(renderer, delay, param, startAt, stopEarly)
        {
        }

        public override bool IsOver(int frameCounter)
        {
            if (frameCounter < delay * 9)
                return false;
            return true;
        }

        public override void Draw(GameTime gameTime, GraphicsDevice device, SpriteBatch spriteBatch, int frameCounter)
        {
            Texture2D sprite = SpriteManager.EffectSprites["note-heart"];

            int animationFrame = frameCounter / delay;
            int offset = animationFrame % 9;

            Rectangle srcRect = new Rectangle(spriteWidth * (param % 2), 0, spriteWidth, sprite.Height);
            Rectangle destRect = new Rectangle(renderer.ScreenEdgeDirection > 0 ? startRight.X : startLeft.X, renderer.ScreenEdgeDirection > 0 ? startRight.Y : startLeft.Y, spriteWidth, sprite.Height);

            for(int i = 0; i < offset - 1; i++)
                destRect.Offset(offsets[i].X * renderer.ScreenEdgeDirection, offsets[i].Y * renderer.ScreenEdgeDirection);

            if(param >= 2)
                spriteBatch.Draw(sprite, destRect, srcRect, Color.White, 0f, new Vector2(), SpriteEffects.None, 0f);

            for (int i = Math.Max(0, offset - 1); i < offset; i++)
                destRect.Offset(offsets[offset - 1].X * renderer.ScreenEdgeDirection, offsets[offset - 1].Y * renderer.ScreenEdgeDirection);
            
            spriteBatch.Draw(sprite, destRect, srcRect, Color.White, 0f, new Vector2(), SpriteEffects.None, 0f);
        }
    }
}
