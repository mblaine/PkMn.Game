using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PkMn.Game.Controls;
using PkMn.Game.SpecialEffects;

namespace PkMn.Game.Animations
{
    public class FlameAcross : Animation
    {
        public FlameAcross(MonsterRenderer renderer, int delay, int param, int startAt = 0, int stopEarly = int.MaxValue)
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
            Texture2D sprite = SpriteManager.EffectSprites["flame-large"];

            Rectangle rect;

            rect = new Rectangle(renderer.MonSpriteRect.X + renderer.ScreenEdgeDirection * sprite.Height / 4 + (renderer.ScreenEdgeDirection < 0 ? renderer.MonSpriteRect.Width - sprite.Width : 0), renderer.MonSpriteBottom - sprite.Height, sprite.Width, sprite.Height);

            if (frameCounter >= delay)
                rect = new Rectangle(rect.X + renderer.ScreenEdgeDirection * sprite.Width, rect.Y, rect.Width, rect.Height);
            if (frameCounter >= delay * 2)
                rect = new Rectangle(rect.X + renderer.ScreenEdgeDirection * sprite.Width, rect.Y, rect.Width, rect.Height);

            for (int i = 0; i < param; i++)
            {
                spriteBatch.Draw(sprite, rect, null, Color.White, 0f, new Vector2(), renderer.ScreenEdgeDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
                rect.Offset(0, -sprite.Height);
            }
        }
    }
}
