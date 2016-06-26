using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PkMn.Game.Controls;
using PkMn.Game.SpecialEffects;

namespace PkMn.Game.Animations
{
    public class Egg : Animation
    {
        protected static int spriteHeight = 64;

        public Egg(MonsterRenderer renderer, int delay, int param = 0, int startAt = 0, int stopEarly = int.MaxValue)
            : base(renderer, delay, param, startAt, stopEarly)
        {
        }

        public override bool UseMonDefaultPalette(MonsterRenderer renderer)
        {
            return renderer == this.renderer;
        }

        public override bool IsOver(int frameCounter)
        {
            if (frameCounter < delay * 8)
                return false;
            return true;
        }

        public override void Draw(GameTime gameTime, GraphicsDevice device, SpriteBatch spriteBatch, int frameCounter)
        {
            Texture2D sprite = SpriteManager.EffectSprites["egg"];

            int animationFrame = (frameCounter / delay) % 4;
            Rectangle srcRect = new Rectangle(0, spriteHeight * animationFrame, sprite.Width, spriteHeight);
            Rectangle destRect = new Rectangle(renderer.MonSpriteRect.X + renderer.MonSpriteRect.Width / 2 - sprite.Width / 2, renderer.MonSpriteBottom - spriteHeight - 32, sprite.Width, spriteHeight);

            spriteBatch.Draw(sprite, destRect, srcRect, Color.White, 0f, new Vector2(), SpriteEffects.None, 0f);
        }
    }
}
