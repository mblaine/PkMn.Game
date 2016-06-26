using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PkMn.Game.Controls;
using PkMn.Game.SpecialEffects;

namespace PkMn.Game.Animations
{
    public class CloudLow : Animation
    {
        public CloudLow(MonsterRenderer renderer, int delay, int param = 0, int startAt = 0, int stopEarly = int.MaxValue)
            : base(renderer, delay, param, startAt, stopEarly)
        {
        }

        public override bool IsOver(int frameCounter)
        {
            if (frameCounter < delay * 6)
                return false;
            return true;
        }

        public override void Draw(GameTime gameTime, GraphicsDevice device, SpriteBatch spriteBatch, int frameCounter)
        {
            Texture2D sprite = SpriteManager.EffectSprites["cloud-low"];

            int animationFrame = frameCounter / delay;
            Rectangle srcRect = new Rectangle(0, 0, sprite.Width, sprite.Height);
            Rectangle destRect = new Rectangle(renderer.MonSpriteRect.X + renderer.MonSpriteRect.Width / 2 - sprite.Width / 2, renderer.MonSpriteBottom - sprite.Height - 24, sprite.Width, sprite.Height);

            if(animationFrame % 2 == 0)
                spriteBatch.Draw(sprite, destRect, srcRect, Color.White, 0f, new Vector2(), SpriteEffects.None, 0f);
        }
    }
}
