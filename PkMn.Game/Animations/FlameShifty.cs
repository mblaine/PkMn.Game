using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PkMn.Game.Controls;
using PkMn.Game.SpecialEffects;

namespace PkMn.Game.Animations
{
    public class FlameShifty : Animation
    {
        public FlameShifty(MonsterRenderer renderer, int delay, int param = 0, int startAt = 0, int stopEarly = int.MaxValue)
            : base(renderer, delay, param, startAt, stopEarly)
        {
        }

        public override bool IsOver(int frameCounter)
        {
            if (frameCounter <= delay * 12)
                return false;
            return true;
        }

        public override void Draw(GameTime gameTime, GraphicsDevice device, SpriteBatch spriteBatch, int frameCounter)
        {
            Rectangle rect;
            Texture2D sprite = frameCounter % (delay * 2) < delay ? SpriteManager.EffectSprites["flame"] : SpriteManager.EffectSprites["flame-large"];

            rect = new Rectangle(renderer.MonSpriteRect.X + renderer.MonSpriteRect.Width / 2 - sprite.Width / 2, renderer.MonSpriteBottom - sprite.Height, sprite.Width, sprite.Height);

            if (frameCounter >= delay * 4 && frameCounter < delay * 8)
                rect = new Rectangle(rect.X + renderer.ScreenEdgeDirection * sprite.Width, rect.Y, rect.Width, rect.Height);
            else if (frameCounter >= delay * 8)
                rect = new Rectangle(rect.X + renderer.ScreenEdgeDirection * sprite.Width / 2, rect.Y, rect.Width, rect.Height);

            spriteBatch.Draw(sprite, rect, null, Color.White, 0f, new Vector2(), renderer.ScreenEdgeDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
        }
    }
}
