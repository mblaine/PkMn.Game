using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PkMn.Game.Controls;
using PkMn.Game.SpecialEffects;

namespace PkMn.Game.Animations
{
    public class Soundwave : Animation
    {
        protected static Point startPosition = new Point(386, 144);

        protected static Point[] offsets = new Point[] 
        {
            new Point(-16, 0),
            new Point(-16, 32),
            new Point(-16, 0),
            new Point(-16, 32),
            new Point(-16, 0),
            new Point(-16, 32),
            new Point(-16, 0),
            new Point(-16, 32),
            new Point(-16, 0)
        };

        public Soundwave(MonsterRenderer renderer, int delay, int param = 0, int startAt = 0, int stopEarly = int.MaxValue)
            : base(renderer, delay, param, startAt, stopEarly)
        {
        }

        public override bool IsOver(int frameCounter)
        {
            if (frameCounter < delay * 10)
                return false;
            return true;
        }

        public override void Draw(GameTime gameTime, GraphicsDevice device, SpriteBatch spriteBatch, int frameCounter)
        {
            Texture2D sprite = SpriteManager.EffectSprites["soundwave"];

            int animationFrame = frameCounter / delay;
            int offset = renderer.ScreenEdgeDirection > 0 ? animationFrame % 10 : (9 - animationFrame) % 10;

            Rectangle srcRect = new Rectangle(0, 0, sprite.Width, sprite.Height);
            Rectangle destRect = new Rectangle(startPosition.X, startPosition.Y, sprite.Width, sprite.Height);

            for(int i = 0; i < offset; i++)
                destRect.Offset(offsets[i]);

            spriteBatch.Draw(sprite, destRect, srcRect, Color.White, 0f, new Vector2(), renderer.ScreenEdgeDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
        }
    }
}
