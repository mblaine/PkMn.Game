using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PkMn.Game.Controls;
using PkMn.Game.SpecialEffects;

namespace PkMn.Game.Animations
{
    public class Droplets : Animation
    {
        protected int spriteWidth = 692;
        protected int spriteHeight = 408;

        public Droplets(MonsterRenderer renderer, int delay, int param = 0, int startAt = 0, int stopEarly = int.MaxValue)
            : base(renderer, delay, param, startAt, stopEarly)
        {
        }

        public override bool IsOver(int frameCounter)
        {
            if (frameCounter < 128)
                return false;
            return true;
        }

        public override void Draw(GameTime gameTime, GraphicsDevice device, SpriteBatch spriteBatch, int frameCounter)
        {
            Texture2D sprite = SpriteManager.EffectSprites["droplets"];

            int animationFrame = frameCounter / delay;
            int animationLoop = animationFrame / 12;
            int loopFrame = animationFrame % 12;

            Rectangle srcRect = new Rectangle(0, sprite.Height - spriteHeight, spriteWidth, spriteHeight);

            for (int i = 0; i < loopFrame / 2; i++)
            {
                srcRect.Offset(i % 2 == 0 ? 36 : 60, -32);
            }
           
            Rectangle destRect = new Rectangle(0, 0, spriteWidth, spriteHeight);
            
            if(loopFrame % 2 == 0)
                spriteBatch.Draw(sprite, destRect, srcRect, Color.White, 0f, new Vector2(), SpriteEffects.None, 0f);
        }
    }
}
