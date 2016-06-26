using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PkMn.Game.Controls;
using PkMn.Game.SpecialEffects;

namespace PkMn.Game.Animations
{
    public class Squares : Animation
    {
        protected static int spriteWidth = 192;
        protected static int spriteHeight = 192;

        public Squares(MonsterRenderer renderer, int delay, int param = 0, int startAt = 0, int stopEarly = int.MaxValue)
            : base(renderer, delay, param, startAt, stopEarly)
        {
        }

        public override bool IsOver(int frameCounter)
        {
            if (frameCounter < delay * 3)
                return false;
            return true;
        }

        public override bool UseMonDefaultPalette(MonsterRenderer renderer)
        {
            return param == 1 && renderer == this.renderer;
        }

        public override void Draw(GameTime gameTime, GraphicsDevice device, SpriteBatch spriteBatch, int frameCounter)
        {
            Texture2D sprite = SpriteManager.EffectSprites["squares"];

            int animationFrame = frameCounter / delay;
            Rectangle srcRect = new Rectangle(spriteWidth * param, spriteHeight * animationFrame, spriteWidth, spriteHeight);
            Rectangle destRect = new Rectangle(renderer.MonSpriteRect.X + renderer.MonSpriteRect.Width / 2 - spriteWidth / 2, renderer.MonSpriteRect.Y, spriteWidth, spriteHeight);

            spriteBatch.Draw(sprite, destRect, srcRect, Color.White, 0f, new Vector2(), renderer.ScreenEdgeDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
        }
    }
}
