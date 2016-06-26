using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PkMn.Game.Controls;
using PkMn.Game.SpecialEffects;

namespace PkMn.Game.Animations
{
    public class Column : Animation
    {
        protected static int spriteHeight = 32;

        public Column(MonsterRenderer renderer, int delay, int param = 0, int startAt = 0, int stopEarly = int.MaxValue)
            : base(renderer, delay, param, startAt, stopEarly)
        {
        }

        public override bool UseMonDefaultPalette(MonsterRenderer renderer)
        {
            return renderer == this.renderer;
        }

        public override bool IsOver(int frameCounter)
        {
            if (frameCounter < delay * 9)
                return false;
            return true;
        }

        public override void Draw(GameTime gameTime, GraphicsDevice device, SpriteBatch spriteBatch, int frameCounter)
        {
            Texture2D sprite = SpriteManager.EffectSprites["column"];

            int animationFrame = frameCounter / delay;

            Rectangle srcRect = new Rectangle(0, 0, sprite.Width, spriteHeight * ((10 - animationFrame) / 2));
            Rectangle destRect = new Rectangle(renderer.MonSpriteRect.X + renderer.MonSpriteRect.Width / 2 - sprite.Width / 2, renderer.MonSpriteRect.Y + 16 * (animationFrame % 2), sprite.Width, srcRect.Height);
 
            spriteBatch.Draw(sprite, destRect, srcRect, Color.White, 0f, new Vector2(), SpriteEffects.None, 0f);
        }
    }
}
