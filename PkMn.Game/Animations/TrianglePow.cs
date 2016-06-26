using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PkMn.Game.Controls;
using PkMn.Game.SpecialEffects;

namespace PkMn.Game.Animations
{
    public class TrianglePow : Animation
    {
        public TrianglePow(MonsterRenderer renderer, int delay, int param = 0, int startAt = 0, int stopEarly = int.MaxValue)
            : base(renderer, delay, param, startAt, stopEarly)
        {
        }

        public override bool IsOver(int frameCounter)
        {
            if (frameCounter <= delay * 3)
                return false;
            return true;
        }

        public override void Draw(GameTime gameTime, GraphicsDevice device, SpriteBatch spriteBatch, int frameCounter)
        {
            Texture2D sprite = SpriteManager.EffectSprites["pow"];

            Rectangle rect;

            rect = new Rectangle((int)renderer.MonSpriteRect.X + renderer.MonSpriteRect.Width - sprite.Width - (renderer.ScreenEdgeDirection < 0 ? sprite.Width * 3 / 2: 0), (int)renderer.MonSpriteRect.Y + 64, sprite.Width, sprite.Height);

            if (frameCounter >= delay && frameCounter < delay * 2)
                rect = new Rectangle(rect.X - sprite.Width * 2 / 3 * renderer.ScreenEdgeDirection, rect.Y + sprite.Height * 2 / 3, sprite.Width, sprite.Height);
            else if (frameCounter >= delay * 2)
                rect = new Rectangle(rect.X - sprite.Width * renderer.ScreenEdgeDirection, rect.Y + sprite.Height * 1 / 3, sprite.Width, sprite.Height); ;

            spriteBatch.Draw(sprite, rect, null, Color.White, 0f, new Vector2(), renderer.ScreenEdgeDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
        }
    }
}
