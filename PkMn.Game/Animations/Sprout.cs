using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PkMn.Game.Controls;
using PkMn.Game.SpecialEffects;

namespace PkMn.Game.Animations
{
    public class Sprout : Animation
    {
        public Sprout(MonsterRenderer renderer, int delay, int param = 0, int startAt = 0, int stopEarly = int.MaxValue)
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
            Texture2D sprite = SpriteManager.EffectSprites["sprout"];

            Rectangle rect;

            rect = new Rectangle(renderer.MonSpriteRect.X + (renderer.ScreenEdgeDirection > 0 ? 0 : renderer.MonSpriteRect.Width - sprite.Width - 32), renderer.MonSpriteBottom - 16 - sprite.Height, sprite.Width, sprite.Height);

            if (frameCounter >= delay)
                rect.Offset(renderer.ScreenEdgeDirection * 128, 0);
            if (frameCounter >= delay * 2)
                rect.Offset(renderer.ScreenEdgeDirection * -64, 16);

            spriteBatch.Draw(sprite, rect, null, Color.White, 0f, new Vector2(), renderer.ScreenEdgeDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
        }
    }
}
