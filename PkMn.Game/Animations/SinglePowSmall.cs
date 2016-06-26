using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PkMn.Game.Controls;
using PkMn.Game.SpecialEffects;

namespace PkMn.Game.Animations
{
    public class SinglePowSmall : Animation
    {
        public SinglePowSmall(MonsterRenderer renderer, int delay, int param = 0, int startAt = 0, int stopEarly = int.MaxValue)
            : base(renderer, delay, param, startAt, stopEarly)
        {
        }

        public override bool IsOver(int frameCounter)
        {
            if (frameCounter < delay)
                return false;
            return true;
        }

        public override void Draw(GameTime gameTime, GraphicsDevice device, SpriteBatch spriteBatch, int frameCounter)
        {
            Texture2D sprite = SpriteManager.EffectSprites["pow"];

            Rectangle rect;

            rect = new Rectangle((int)renderer.MonSpriteRect.X + renderer.MonSpriteRect.Width - sprite.Width + (renderer.ScreenEdgeDirection < 0 ? -112: -32), (int)renderer.MonSpriteRect.Y + 96, sprite.Width, sprite.Height);
            if (param == 1)
                rect.Offset(renderer.ScreenEdgeDirection > 0 ? -24 : 96, renderer.ScreenEdgeDirection > 0 ? -48 : -64);

            spriteBatch.Draw(sprite, rect, null, Color.White, 0f, new Vector2(), renderer.ScreenEdgeDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
        }
    }
}
