using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PkMn.Game.Controls;
using PkMn.Game.SpecialEffects;

namespace PkMn.Game.Animations
{
    public class Leaf : Animation
    {
        protected static Point startPosition = new Point(352, 192);

        protected static Point[] offsets = new Point[] 
        {
            new Point(-64, 32),
            new Point(-64, 32),
            new Point(-64, 32),
            new Point(-64, 32),
            new Point(-64, 32)
        };

        public Leaf(MonsterRenderer renderer, int delay, int param = 0, int startAt = 0, int stopEarly = int.MaxValue)
            : base(renderer, delay, param, startAt, stopEarly)
        {
        }

        public override bool IsOver(int frameCounter)
        {
            if (frameCounter < delay * 12)
                return false;
            return true;
        }

        public override void Draw(GameTime gameTime, GraphicsDevice device, SpriteBatch spriteBatch, int frameCounter)
        {
            Texture2D sprite = SpriteManager.EffectSprites["leaf"];

            int animationFrame = frameCounter / delay;
            int offset = animationFrame % 6;

            Rectangle srcRect = new Rectangle(0, 0, sprite.Width, sprite.Height);
            Rectangle destRect = new Rectangle(startPosition.X, startPosition.Y, sprite.Width, sprite.Height);

            if (renderer.ScreenEdgeDirection < 0)
                destRect.Offset(-96, 32);

            if (animationFrame >= 6)
                destRect.Offset(0, 64 * renderer.ScreenEdgeDirection);
            
            for(int i = 0; i < offset; i++)
                destRect.Offset(new Point(offsets[i].X * renderer.ScreenEdgeDirection, offsets[i].Y * renderer.ScreenEdgeDirection));
            
            spriteBatch.Draw(sprite, destRect, srcRect, Color.White, 0f, new Vector2(), renderer.ScreenEdgeDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
        }
    }
}
