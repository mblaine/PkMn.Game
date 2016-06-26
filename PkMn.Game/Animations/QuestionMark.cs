using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PkMn.Game.Controls;
using PkMn.Game.SpecialEffects;

namespace PkMn.Game.Animations
{
    public class QuestionMark : Animation
    {
        public QuestionMark(MonsterRenderer renderer, int delay, int param = 0, int startAt = 0, int stopEarly = int.MaxValue)
            : base(renderer, delay, param, startAt, stopEarly)
        {
        }

        public override bool IsOver(int frameCounter)
        {
            if (frameCounter < delay * 2)
                return false;
            return true;
        }

        public override void Draw(GameTime gameTime, GraphicsDevice device, SpriteBatch spriteBatch, int frameCounter)
        {
            Texture2D sprite = SpriteManager.EffectSprites["question-mark"];

            int animationFrame = frameCounter / delay;

            Rectangle srcRect = new Rectangle(0, 0, sprite.Width, sprite.Height);
            Rectangle destRect = new Rectangle(renderer.ScreenEdgeDirection > 0 ? renderer.MonSpriteRect.X + 32 : renderer.MonSpriteRect.X + renderer.MonSpriteRect.Width - sprite.Width - 64, renderer.MonSpriteRect.Y, sprite.Width, sprite.Height);

            if (frameCounter >= delay)
                destRect.Offset(renderer.ScreenEdgeDirection > 0 ? -64 : 96, renderer.ScreenEdgeDirection > 0 ? 64 : 32);
 
            spriteBatch.Draw(sprite, destRect, srcRect, Color.White, 0f, new Vector2(), SpriteEffects.None, 0f);
        }
    }
}
