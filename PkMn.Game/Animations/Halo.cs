using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PkMn.Game.Controls;
using PkMn.Game.SpecialEffects;

namespace PkMn.Game.Animations
{
    public class Halo : Animation
    {
        protected static int spriteWidth = 64;

        protected static Point[] offsets = new Point[] 
        {
            new Point(-96, 0),
            new Point(-32, -16),
            new Point(32, -16),
            new Point(96, 0),
            new Point(32, 16)
        };

        public Halo(MonsterRenderer renderer, int delay, int param = 0, int startAt = 0, int stopEarly = int.MaxValue)
            : base(renderer, delay, param, startAt, stopEarly)
        {
        }

        public override bool IsOver(int frameCounter)
        {
            if (frameCounter < delay * 6)
                return false;
            return true;
        }

        public override void Draw(GameTime gameTime, GraphicsDevice device, SpriteBatch spriteBatch, int frameCounter)
        {
            Texture2D sprite = SpriteManager.EffectSprites["note-heart"];

            int animationFrame = frameCounter / delay;

            Rectangle srcRect = new Rectangle(0, 0, spriteWidth, sprite.Height);
            Rectangle destRect = new Rectangle(renderer.ScreenEdgeDirection > 0 ? renderer.MonSpriteRect.X + renderer.MonSpriteRect.Width - 32 - spriteWidth : renderer.MonSpriteRect.X + 32, renderer.MonSpriteRect.Y + 72, spriteWidth, sprite.Height);

            for(int i = 0; i < animationFrame; i++)
                destRect.Offset(offsets[i].X * renderer.ScreenEdgeDirection, offsets[i].Y);

            
            spriteBatch.Draw(sprite, destRect, srcRect, Color.White, 0f, new Vector2(), renderer.ScreenEdgeDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
        }
    }
}
