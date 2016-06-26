using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PkMn.Game.Controls;
using PkMn.Game.SpecialEffects;

namespace PkMn.Game.Animations
{
    public class Beam : Animation
    {
        protected static int spriteHeight = 160;

        public Beam(MonsterRenderer renderer, int delay, int param, int startAt = 0, int stopEarly = int.MaxValue)
            : base(renderer, delay, param, startAt, stopEarly)
        {
        }

        public override bool IsOver(int frameCounter)
        {
            if (frameCounter < delay * 7)
                return false;
            return true;
        }

        public override void Draw(GameTime gameTime, GraphicsDevice device, SpriteBatch spriteBatch, int frameCounter)
        {
            int animationFrame = frameCounter / delay;
            Rectangle srcRect = new Rectangle(0, (spriteHeight * param) + spriteHeight - (64 + 16 * animationFrame), 96 + 32 * animationFrame, 64 + 16 * animationFrame);

            Rectangle destRect;
            if(renderer.ScreenEdgeDirection > 0)
                destRect = new Rectangle((int)renderer.BeamOriginCoords.X - srcRect.Width, (int)renderer.BeamOriginCoords.Y - 64, srcRect.Width, srcRect.Height);
            else
                destRect = new Rectangle((int)renderer.BeamOriginCoords.X + 32, (int)renderer.BeamOriginCoords.Y - 64 - srcRect.Height, srcRect.Width, srcRect.Height);

            spriteBatch.Draw(SpriteManager.EffectSprites["beam"], destRect, srcRect, Color.White, 0f, new Vector2(), renderer.ScreenEdgeDirection < 0 ? SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically : SpriteEffects.None, 0f);
        }
    }
}
