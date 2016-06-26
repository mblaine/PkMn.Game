using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PkMn.Game.Controls;
using PkMn.Game.SpecialEffects;

namespace PkMn.Game.Animations
{
    public class Triangle : Animation
    {
        protected static Point[] offsets = new Point[] 
        {
            new Point(-64, 64),
            new Point(-64, 0),
            new Point(-32, 64),
            new Point(-96, 0),
            new Point(-64, 64)
        };

        public Triangle(MonsterRenderer renderer, int delay, int param = 0, int startAt = 0, int stopEarly = int.MaxValue)
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
            Texture2D sprite = SpriteManager.EffectSprites["triangle"];

            int animationFrame = frameCounter / delay;

            Rectangle srcRect = new Rectangle(0, 0, sprite.Width, sprite.Height);
            Rectangle destRect = new Rectangle(renderer.MonSpriteRect.X + (renderer.ScreenEdgeDirection > 0 ? - sprite.Width : renderer.MonSpriteRect.Width - 32), renderer.MonSpriteRect.Y + (renderer.ScreenEdgeDirection > 0 ? renderer.MonSpriteRect.Height - sprite.Height : 64), sprite.Width, sprite.Height);

            for(int i = 0; i < animationFrame; i++)
                destRect.Offset(new Point(offsets[i].X * renderer.ScreenEdgeDirection, offsets[i].Y * renderer.ScreenEdgeDirection));

            bool flip = animationFrame % 2 == 0;

            if (renderer.ScreenEdgeDirection < 0)
                flip = !flip;
            
            spriteBatch.Draw(sprite, destRect, srcRect, Color.White, 0f, new Vector2(), flip ? SpriteEffects.FlipVertically : SpriteEffects.None, 0f);
        }
    }
}
