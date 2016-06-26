using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PkMn.Game.Controls;
using PkMn.Game.SpecialEffects;

namespace PkMn.Game.Animations
{
    public class Bubbles : Animation
    {
        protected static int spriteHeight = 256;

        public Bubbles(MonsterRenderer renderer, int delay, int param = 0, int startAt = 0, int stopEarly = int.MaxValue)
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
            Texture2D sprite = SpriteManager.EffectSprites["bubbles"];

            int animationFrame = frameCounter / delay;

            Rectangle srcRect = new Rectangle(0, spriteHeight * animationFrame, sprite.Width, spriteHeight);
            Rectangle destRect = new Rectangle(renderer.ScreenEdgeDirection > 0 ? renderer.MonSpriteRect.X + renderer.MonSpriteRect.Width - sprite.Width - 32 : renderer.MonSpriteRect.X + 32, renderer.ScreenEdgeDirection > 0 ? renderer.MonSpriteRect.Y + 32 : renderer.MonSpriteRect.Y + renderer.MonSpriteRect.Height - spriteHeight, sprite.Width, spriteHeight);
 
            spriteBatch.Draw(sprite, destRect, srcRect, Color.White, 0f, new Vector2(), renderer.ScreenEdgeDirection > 0 ? SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically : SpriteEffects.None, 0f);
        }
    }
}
