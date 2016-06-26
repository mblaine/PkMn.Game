using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PkMn.Game.Controls;
using PkMn.Game.SpecialEffects;

namespace PkMn.Game.Animations
{
    public class Spikes : Animation
    {
        protected static int spriteHeight = 112;

        public Spikes(MonsterRenderer renderer, int delay, int param = 0, int startAt = 0, int stopEarly = int.MaxValue)
            : base(renderer, delay, param, startAt, stopEarly)
        {
        }

        public override bool IsOver(int frameCounter)
        {
            if (frameCounter < delay * 6)
                return false;
            return true;
        }

        public override bool UseMonDefaultPalette(MonsterRenderer renderer)
        {
            return renderer == this.renderer;
        }

        public override void Draw(GameTime gameTime, GraphicsDevice device, SpriteBatch spriteBatch, int frameCounter)
        {
            Texture2D sprite = SpriteManager.EffectSprites["spikes"];

            int animationFrame = frameCounter / delay;
            Rectangle srcRect = new Rectangle(0, spriteHeight * animationFrame, sprite.Width, spriteHeight);
            Rectangle destRect = new Rectangle(renderer.MonSpriteRect.X + renderer.MonSpriteRect.Width / 2 + (renderer.ScreenEdgeDirection > 0 ? -sprite.Width + 44 : -64), renderer.ScreenEdgeDirection > 0 ? renderer.MonSpriteBottom - 32 : renderer.MonSpriteRect.Y - spriteHeight + 88, sprite.Width, spriteHeight);

            spriteBatch.Draw(sprite, destRect, srcRect, Color.White, 0f, new Vector2(), renderer.ScreenEdgeDirection < 0 ? SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically : SpriteEffects.None, 0f);
        }
    }
}
