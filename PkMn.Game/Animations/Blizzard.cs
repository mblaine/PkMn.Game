using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PkMn.Game.Controls;
using PkMn.Game.SpecialEffects;

namespace PkMn.Game.Animations
{
    public class Blizzard : Animation
    {
        protected static int spriteWidth = 320;
        protected static int spriteHeight = 224;

        public Blizzard(MonsterRenderer renderer, int delay, int param = 0, int startAt = 0, int stopEarly = int.MaxValue)
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
            Texture2D sprite = SpriteManager.EffectSprites["blizzard"];

            int animationFrame = frameCounter / delay;
            Rectangle srcRect = new Rectangle(spriteWidth * (animationFrame % 4), spriteHeight * (animationFrame / 4), spriteWidth, spriteHeight);
            Rectangle destRect = new Rectangle(renderer.MonSpriteRect.X + (renderer.ScreenEdgeDirection > 0 ? renderer.MonSpriteRect.Width - spriteWidth : 0), renderer.MonSpriteBottom - spriteHeight, spriteWidth, spriteHeight);

            spriteBatch.Draw(sprite, destRect, srcRect, Color.White, 0f, new Vector2(), renderer.ScreenEdgeDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
        }
    }
}
