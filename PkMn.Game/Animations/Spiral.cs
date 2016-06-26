using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PkMn.Game.Controls;
using PkMn.Game.SpecialEffects;

namespace PkMn.Game.Animations
{
    public class Spiral : Animation
    {
        protected static int spriteWidth = 224;
        protected static int spriteHeight = 224;

        public Spiral(MonsterRenderer renderer, int delay, int param = 0, int startAt = 0, int stopEarly = int.MaxValue)
            : base(renderer, delay, param, startAt, stopEarly)
        {
        }

        public override bool IsOver(int frameCounter)
        {
            if (frameCounter < delay * 19)
                return false;
            return true;
        }

        public override bool UseMonDefaultPalette(MonsterRenderer renderer)
        {
            return renderer == this.renderer;
        }

        public override void Draw(GameTime gameTime, GraphicsDevice device, SpriteBatch spriteBatch, int frameCounter)
        {
            Texture2D sprite = SpriteManager.EffectSprites["spiral"];

            int animationFrame = frameCounter / delay;
            Rectangle srcRect = new Rectangle(spriteWidth * (animationFrame % 5), spriteHeight * (animationFrame / 5), spriteWidth, spriteHeight);
            Rectangle destRect = new Rectangle(renderer.MonSpriteRect.X + renderer.MonSpriteRect.Width / 2 - spriteWidth / 2, renderer.MonSpriteRect.Y + (renderer.ScreenEdgeDirection > 0 ? 8 : 0), spriteWidth, spriteHeight);

            spriteBatch.Draw(sprite, destRect, srcRect, Color.White, 0f, new Vector2(), SpriteEffects.None, 0f);
        }
    }
}
