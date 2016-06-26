﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PkMn.Game.Controls;
using PkMn.Game.SpecialEffects;

namespace PkMn.Game.Animations
{
    public class Geyser : Animation
    {
        protected int spriteWidth = 64;

        public Geyser(MonsterRenderer renderer, int delay, int param = 0, int startAt = 0, int stopEarly = int.MaxValue)
            : base(renderer, delay, param, startAt, stopEarly)
        {
        }

        public override bool IsOver(int frameCounter)
        {
            if (frameCounter < delay * 16)
                return false;
            return true;
        }

        public override void Draw(GameTime gameTime, GraphicsDevice device, SpriteBatch spriteBatch, int frameCounter)
        {
            Texture2D sprite = SpriteManager.EffectSprites["geyser"];

            int animationFrame = frameCounter / delay;
            int subAnimation = animationFrame / 8;
            int subAnimationFrame = animationFrame % 8;

            Rectangle srcRect = new Rectangle(animationFrame % 2 == 0 ? 0 : spriteWidth, 0, spriteWidth, 64 + Math.Min(subAnimationFrame, 4) * 32);
            Rectangle destRect;
            if(renderer.ScreenEdgeDirection > 0)
                destRect = new Rectangle(renderer.MonSpriteRect.X + 32 + subAnimation * spriteWidth, renderer.MonSpriteBottom - srcRect.Height , srcRect.Width, srcRect.Height);
            else
                destRect = new Rectangle(renderer.MonSpriteRect.X + renderer.MonSpriteRect.Width - 72 - (subAnimation + 1) * spriteWidth, renderer.MonSpriteBottom - srcRect.Height, srcRect.Width, srcRect.Height);

            spriteBatch.Draw(sprite, destRect, srcRect, Color.White, 0f, new Vector2(), renderer.ScreenEdgeDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
        }
    }
}
