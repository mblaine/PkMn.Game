using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PkMn.Game.Controls;
using PkMn.Game.SpecialEffects;

namespace PkMn.Game.Animations
{
    public class VerticalPow : Animation
    {
        public VerticalPow(MonsterRenderer renderer, int delay, int param = 0, int startAt = 0, int stopEarly = int.MaxValue)
            : base(renderer, delay, param, startAt, stopEarly)
        {
        }

        public override bool IsOver(int frameCounter)
        {
            if (frameCounter <= delay * 4)
                return false;
            return true;
        }

        public override void Draw(GameTime gameTime, GraphicsDevice device, SpriteBatch spriteBatch, int frameCounter)
        {
            Texture2D sprite = SpriteManager.EffectSprites["pow"];

            Rectangle rect;

            rect = new Rectangle((int)renderer.FaceCoords.X + (renderer.ScreenEdgeDirection > 0 ? 64 : -sprite.Width - 80), (int)renderer.FaceCoords.Y - 32, sprite.Width, sprite.Height);

            if (frameCounter >= delay)
                rect.Offset(0, 32);
            if (frameCounter >= delay * 2)
                rect.Offset(0, 32);
            if (frameCounter >= delay * 3)
                rect.Offset(0, 32);

            spriteBatch.Draw(sprite, rect, null, Color.White, 0f, new Vector2(), renderer.ScreenEdgeDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
        }
    }
}
