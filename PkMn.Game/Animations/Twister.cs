﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PkMn.Game.Controls;
using PkMn.Game.SpecialEffects;

namespace PkMn.Game.Animations
{
    public class Twister : Animation
    {
        protected static Point startPosition = new Point(192, 276);

        protected static Point[] offsets = new Point[] 
        {
            
            new Point(32, 0),
            new Point(32, 0),
            new Point(32, 0),
            new Point(32, 0),
            new Point(32, -32),
            new Point(32, -32),
            new Point(32, -64)
        };

        public Twister(MonsterRenderer renderer, int delay, int param = 0, int startAt = 0, int stopEarly = int.MaxValue)
            : base(renderer, delay, param, startAt, stopEarly)
        {
        }

        public override bool IsOver(int frameCounter)
        {
            if (frameCounter < delay * 8)
                return false;
            return true;
        }

        public override void Draw(GameTime gameTime, GraphicsDevice device, SpriteBatch spriteBatch, int frameCounter)
        {
            Texture2D sprite = SpriteManager.EffectSprites["twister"];

            int animationFrame = frameCounter / delay;
            int offset = renderer.ScreenEdgeDirection > 0 ? animationFrame % 8 : (7 - animationFrame) % 8;

            Rectangle srcRect = new Rectangle(0, 0, sprite.Width, sprite.Height);
            Rectangle destRect = new Rectangle(startPosition.X, startPosition.Y, sprite.Width, sprite.Height);

            for(int i = 0; i < offset; i++)
                destRect.Offset(offsets[i]);
            
            spriteBatch.Draw(sprite, destRect, srcRect, Color.White, 0f, new Vector2(), offset % 2 == 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
        }
    }
}
