using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PkMn.Game.Controls;
using PkMn.Game.SpecialEffects;

namespace PkMn.Game.Animations
{
    public class Poof : Animation
    {
        protected static int spriteHeight = 192;

        public Poof(MonsterRenderer renderer, int delay, int param = 0, int startAt = 0, int stopEarly = int.MaxValue)
            : base(renderer, delay, param, startAt, stopEarly)
        {
        }

        public override bool IsOver(int frameCounter)
        {
            if (frameCounter < delay * 5)
                return false;
            return true;
        }

        public override void Draw(GameTime gameTime, GraphicsDevice device, SpriteBatch spriteBatch, int frameCounter)
        {
            Texture2D sprite = SpriteManager.EffectSprites["poof"];

            int animationFrame = frameCounter / delay;
            Rectangle srcRect = new Rectangle(0, spriteHeight * animationFrame, sprite.Width, spriteHeight);
            Rectangle destRect;
            
            if(param == 1)
                destRect = new Rectangle(renderer.MonSpriteRect.X + renderer.MonSpriteRect.Width / 2 - sprite.Width / 2, renderer.MonSpriteRect.Y + renderer.MonSpriteRect.Height / 2 - spriteHeight / 2, sprite.Width, spriteHeight);
            else
                destRect = new Rectangle(renderer.MonSpriteRect.X + renderer.MonSpriteRect.Width / 2 - sprite.Width / 2, renderer.MonSpriteRect.Y + renderer.MonSpriteRect.Height - spriteHeight / 2 - 64, sprite.Width, spriteHeight);

            spriteBatch.Draw(sprite, destRect, srcRect, Color.White, 0f, new Vector2(), SpriteEffects.None, 0f);
        }
    }
}
