using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PkMn.Game.Controls;
using PkMn.Game.SpecialEffects;

namespace PkMn.Game.Animations
{
    public class ThunderBall : Animation
    {
        protected static int spriteHeight = 264;
        protected static int[] frameOrder = new int[] { 1, 2, 3, 1, 2, 4, 5, 6, 4, 5 };

        public ThunderBall(MonsterRenderer renderer, int delay, int param = 0, int startAt = 0, int stopEarly = int.MaxValue)
            : base(renderer, delay, param, startAt, stopEarly)
        {
        }

        public override bool IsOver(int frameCounter)
        {
            if (frameCounter < delay * 31)
                return false;
            return true;
        }

        public override void Draw(GameTime gameTime, GraphicsDevice device, SpriteBatch spriteBatch, int frameCounter)
        {
            Texture2D sprite = SpriteManager.EffectSprites["thunder-ball"];

            int animationFrame;
            if (frameCounter < delay)
                animationFrame = 0;
            else
                animationFrame = frameOrder[((frameCounter / delay) - 1) % 10];

            Rectangle srcRect = new Rectangle(0, spriteHeight * animationFrame, sprite.Width, spriteHeight);
            Rectangle destRect = new Rectangle(renderer.MonSpriteRect.X, renderer.MonSpriteRect.Y, sprite.Width , spriteHeight);

            spriteBatch.Draw(sprite, destRect, srcRect, Color.White, 0f, new Vector2(), SpriteEffects.None, 0f);
        }
    }
}
