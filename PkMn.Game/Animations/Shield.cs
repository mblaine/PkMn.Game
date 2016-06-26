using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PkMn.Game.Controls;
using PkMn.Game.SpecialEffects;

namespace PkMn.Game.Animations
{
    public class Shield : Animation
    {
        public Shield(MonsterRenderer renderer, int delay, int param, int startAt = 0, int stopEarly = int.MaxValue)
            : base(renderer, delay, param, startAt, stopEarly)
        {
        }

        public override bool IsOver(int frameCounter)
        {
            if (frameCounter < delay * 3)
                return false;
            return true;
        }

        public override void Draw(GameTime gameTime, GraphicsDevice device, SpriteBatch spriteBatch, int frameCounter)
        {
            Texture2D sprite = SpriteManager.EffectSprites["shield"];

            Rectangle srcRect = new Rectangle(0, 0, sprite.Width, sprite.Height);
            Rectangle destRect = new Rectangle(renderer.MonSpriteRect.X + (renderer.ScreenEdgeDirection > 0 ? -24 : renderer.MonSpriteRect.Width - 16), renderer.ScreenEdgeDirection > 0 ? renderer.MonSpriteRect.Y + 64 : renderer.MonSpriteBottom - sprite.Height, sprite.Width, sprite.Height);

            
            if(frameCounter % delay * 2 < delay)
                spriteBatch.Draw(sprite, destRect, srcRect, Color.White, 0f, new Vector2(), renderer.ScreenEdgeDirection < 0 ? SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically : SpriteEffects.None, 0f);
        }
    }
}
