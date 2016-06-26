using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PkMn.Game.Controls;
using PkMn.Game.SpecialEffects;

namespace PkMn.Game.Animations
{
    public class Braces : Animation
    {
        protected static int spriteHeight = 128;

        public Braces(MonsterRenderer renderer, int delay, int param, int startAt = 0, int stopEarly = int.MaxValue)
            : base(renderer, delay, param, startAt, stopEarly)
        {
        }

        public override bool IsOver(int frameCounter)
        {
            if (frameCounter < delay * 2)
                return false;
            return true;
        }

        public override bool UseMonDefaultPalette(MonsterRenderer renderer)
        {
            return param == 0 && renderer != this.renderer;
        }

        public override void Draw(GameTime gameTime, GraphicsDevice device, SpriteBatch spriteBatch, int frameCounter)
        {
            Texture2D sprite = SpriteManager.EffectSprites["braces"];

            Rectangle srcRect = new Rectangle(0, spriteHeight * param, sprite.Width, spriteHeight);
            Rectangle destRect = new Rectangle(renderer.MonSpriteRect.X - 32, renderer.MonSpriteBottom - spriteHeight, sprite.Width, spriteHeight);

            if (frameCounter >= delay)
                destRect.Offset(8 * renderer.ScreenEdgeDirection, 0);

            spriteBatch.Draw(sprite, destRect, srcRect, Color.White, 0f, new Vector2(), renderer.ScreenEdgeDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
        }
    }
}
