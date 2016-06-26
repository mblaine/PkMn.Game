using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PkMn.Game.Controls;
using PkMn.Game.SpecialEffects;

namespace PkMn.Game.Animations
{
    public class ExplosionSingle : Animation
    {
        protected static int spriteHeight = 128;

        public ExplosionSingle(MonsterRenderer renderer, int delay, int param = 0, int startAt = 0, int stopEarly = int.MaxValue)
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
            Texture2D sprite = SpriteManager.EffectSprites["explosion-single"];

            int animationFrame = frameCounter / delay;
            Rectangle srcRect = new Rectangle(0, spriteHeight * animationFrame, sprite.Width, spriteHeight);
            Rectangle destRect = new Rectangle((int)renderer.MonSpriteRect.X + renderer.MonSpriteRect.Width - sprite.Width + (renderer.ScreenEdgeDirection > 0 ? -48 : -8), (int)renderer.MonSpriteRect.Y + 88 + (renderer.ScreenEdgeDirection > 0 ? -48 : -64), sprite.Width, spriteHeight);
            
            spriteBatch.Draw(sprite, destRect, srcRect, Color.White, 0f, new Vector2(), renderer.ScreenEdgeDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
        }
    }
}
