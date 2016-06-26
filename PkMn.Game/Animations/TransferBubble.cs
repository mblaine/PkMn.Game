using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PkMn.Game.Controls;
using PkMn.Game.SpecialEffects;

namespace PkMn.Game.Animations
{
    public class TransferBubble : Animation
    {
        protected static int spriteWidth = 64;

        protected static Point startPosition = new Point(482, 80);

        protected static Point[] offsets = new Point[] 
        {
            new Point(-32, -16),
            new Point(-32, -16),
            new Point(-32, -8),
            new Point(-32, -8),
            new Point(-16, -8),
            new Point(-16, +8),
            new Point(-16, +16),
            new Point(-16, +16),
            new Point(-64, +80),
            new Point(-66, +100)
        };

        public TransferBubble(MonsterRenderer renderer, int delay, int param = 0, int startAt = 0, int stopEarly = int.MaxValue)
            : base(renderer, delay, param, startAt, stopEarly)
        {
        }

        public override bool IsOver(int frameCounter)
        {
            if (frameCounter < delay * 11)
                return false;
            return true;
        }

        public override bool UseMonDefaultPalette(MonsterRenderer renderer)
        {
            return param == 1 && renderer == this.renderer;
        }

        public override void Draw(GameTime gameTime, GraphicsDevice device, SpriteBatch spriteBatch, int frameCounter)
        {
            Texture2D sprite = SpriteManager.EffectSprites["transfer-bubble"];

            int animationFrame = frameCounter / delay;
            int offset = renderer.ScreenEdgeDirection > 0 ? animationFrame % 11 : (10 - animationFrame) % 11;

            Rectangle srcRect = new Rectangle(spriteWidth * param, 0, spriteWidth, sprite.Height);
            Rectangle destRect = new Rectangle(startPosition.X, startPosition.Y, spriteWidth, sprite.Height);

            for(int i = 0; i < offset; i++)
                destRect.Offset(offsets[i]);
            
            spriteBatch.Draw(sprite, destRect, srcRect, Color.White, 0f, new Vector2(), renderer.ScreenEdgeDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
        }
    }
}
