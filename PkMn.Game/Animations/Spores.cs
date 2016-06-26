using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PkMn.Game.Controls;
using PkMn.Game.SpecialEffects;

namespace PkMn.Game.Animations
{
    public class Spores : Animation
    {
        protected static int rowHeight = 32;
        protected static int columnWidth = 32;

        public Spores(MonsterRenderer renderer, int delay, int param = 0, int startAt = 0, int stopEarly = int.MaxValue)
            : base(renderer, delay, param, startAt, stopEarly)
        {
        }

        public override bool IsOver(int frameCounter)
        {
            if (frameCounter < delay * 8)
                return false;
            return true;
        }

        public override void Draw(GameTime gameTime, GraphicsDevice device, SpriteBatch spriteBatch, int frameCounter)
        {
            Texture2D sprite = SpriteManager.EffectSprites["spores"];

            int rows = (frameCounter / delay / 2) + 1;
            int columnOffset = frameCounter % (delay * 2) < delay ? 0 : 1;

            Rectangle srcRect = new Rectangle(0, 0, sprite.Width, rowHeight * rows);
            Rectangle destRect = new Rectangle(renderer.MonSpriteRect.X - 16 * renderer.ScreenEdgeDirection + columnOffset * columnWidth * renderer.ScreenEdgeDirection, renderer.MonSpriteRect.Y, sprite.Width, srcRect.Height);

            spriteBatch.Draw(sprite, destRect, srcRect, Color.White, 0f, new Vector2(), renderer.ScreenEdgeDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
        }
    }
}
