﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PkMn.Game.Controls;
using PkMn.Game.SpecialEffects;

namespace PkMn.Game.Animations
{
    public class Explosion : Animation
    {
        protected static int spriteWidth = 224;
        protected static int spriteHeight = 256;

        public Explosion(MonsterRenderer renderer, int delay, int param = 0, int startAt = 0, int stopEarly = int.MaxValue)
            : base(renderer, delay, param, startAt, stopEarly)
        {
        }

        public override bool IsOver(int frameCounter)
        {
            if (frameCounter < delay * 21)
                return false;
            return true;
        }

        public override void Draw(GameTime gameTime, GraphicsDevice device, SpriteBatch spriteBatch, int frameCounter)
        {
            Texture2D sprite = SpriteManager.EffectSprites["explosion"];

            int animationFrame = frameCounter / delay;
            
            Rectangle srcRect = new Rectangle(spriteWidth * (animationFrame % 3), spriteHeight * (animationFrame / 3), spriteWidth, spriteHeight);
            Rectangle destRect = new Rectangle(renderer.MonSpriteRect.X + (renderer.ScreenEdgeDirection > 0 ? renderer.MonSpriteRect.Width - spriteWidth : 0), renderer.MonSpriteRect.Y, spriteWidth, spriteHeight);

            spriteBatch.Draw(sprite, destRect, srcRect, Color.White, 0f, new Vector2(), renderer.ScreenEdgeDirection < 0 ? SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically : SpriteEffects.None, 0f);
        }
    }
}
