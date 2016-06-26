using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PkMn.Game.Controls;
using PkMn.Game.SpecialEffects;

namespace PkMn.Game.Animations
{
    public class DoublePow : Animation
    {
        public DoublePow(MonsterRenderer renderer, int delay, int param = 0, int startAt = 0, int stopEarly = int.MaxValue)
            : base(renderer, delay, param, startAt, stopEarly)
        {
        }

        public override bool IsOver(int frameCounter)
        {
            if (frameCounter <= delay * 2)
                return false;
            return true;
        }

        public override void Draw(GameTime gameTime, GraphicsDevice device, SpriteBatch spriteBatch, int frameCounter)
        {
            Texture2D sprite = SpriteManager.EffectSprites["pow"];

            Rectangle rect;

            rect = new Rectangle((int)renderer.FaceCoords.X - (renderer.ScreenEdgeDirection < 0 ? sprite.Width : 0), (int)renderer.FaceCoords.Y, sprite.Width, sprite.Height);

            if (frameCounter >= delay)
                rect = new Rectangle(rect.X + sprite.Width * 2 / 3 * renderer.ScreenEdgeDirection, rect.Y + sprite.Height * 2 / 3, sprite.Width, sprite.Height);

            spriteBatch.Draw(sprite, rect, null, Color.White, 0f, new Vector2(), renderer.ScreenEdgeDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
        }
    }
}
