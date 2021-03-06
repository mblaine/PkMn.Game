﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PkMn.Game.Controls;
using PkMn.Game.SpecialEffects;

namespace PkMn.Game.Animations
{
    public class Slash : Animation
    {
        protected static int spriteHeight = 192;

        public Slash(MonsterRenderer renderer, int delay, int param = 0, int startAt = 0, int stopEarly = int.MaxValue)
            : base(renderer, delay, param, startAt, stopEarly)
        {
        }

        public override bool IsOver(int frameCounter)
        {
            if (frameCounter < delay * 4)
                return false;
            return true;
        }

        public override void Draw(GameTime gameTime, GraphicsDevice device, SpriteBatch spriteBatch, int frameCounter)
        {
            Texture2D sprite = SpriteManager.EffectSprites["slash"];

            int animationFrame = frameCounter / delay;
            
            Rectangle srcRect = new Rectangle(0, spriteHeight * (animationFrame % 4), sprite.Width, spriteHeight);
            Rectangle destRect = new Rectangle(renderer.MonSpriteRect.X + renderer.MonSpriteRect.Width / 2 - sprite.Width / 2, renderer.MonSpriteBottom - spriteHeight + 32, sprite.Width, spriteHeight);

            spriteBatch.Draw(sprite, destRect, srcRect, Color.White, 0f, new Vector2(), renderer.ScreenEdgeDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
        }
    }
}
