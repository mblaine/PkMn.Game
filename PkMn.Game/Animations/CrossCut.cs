using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PkMn.Game.Controls;
using PkMn.Game.SpecialEffects;

namespace PkMn.Game.Animations
{
    public class CrossCut : Animation
    {
        protected static int spriteWidth = 256;
        protected static int spriteHeight = 256;

        public CrossCut(MonsterRenderer renderer, int delay, int param = 0, int startAt = 0, int stopEarly = int.MaxValue)
            : base(renderer, delay, param, startAt, stopEarly)
        {
        }

        public override bool IsOver(int frameCounter)
        {
            if (frameCounter < delay * 4)
                return false;
            return true;
        }

        public override void Draw(GameTime gameTime, GraphicsDevice device, SpriteBatch spriteBatch, int frameCounter)
        {
            Texture2D sprite = SpriteManager.EffectSprites["cross-cut"];

            int animationFrame = frameCounter / delay;
            
            Rectangle srcRect = new Rectangle(spriteWidth * param, spriteHeight * (animationFrame % 4), spriteWidth, spriteHeight);
            Rectangle destRect = new Rectangle(renderer.MonSpriteRect.X + renderer.MonSpriteRect.Width / 2 - spriteWidth / 2 + (renderer.ScreenEdgeDirection > 0 ? -16 : 0), renderer.MonSpriteRect.Y, spriteWidth, spriteHeight);

            spriteBatch.Draw(sprite, destRect, srcRect, Color.White, 0f, new Vector2(), renderer.ScreenEdgeDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
        }
    }
}
