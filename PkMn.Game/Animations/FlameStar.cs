using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PkMn.Game.Controls;
using PkMn.Game.SpecialEffects;

namespace PkMn.Game.Animations
{
    public class FlameStar : Animation
    {
        public FlameStar(MonsterRenderer renderer, int delay, int param = 0, int startAt = 0, int stopEarly = int.MaxValue)
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
            Rectangle rect;
            Texture2D sprite = frameCounter % (3 * delay) < delay ? SpriteManager.EffectSprites["flame-star"] : SpriteManager.EffectSprites["flame-star-large"];

            rect = new Rectangle((int)renderer.FaceCoords.X - (renderer.ScreenEdgeDirection < 0 ? sprite.Width : 0), (int)renderer.FaceCoords.Y - 32, sprite.Width, sprite.Height);

            if(frameCounter % (3  * delay) < 2 * delay)
                spriteBatch.Draw(sprite, rect, null, Color.White, 0f, new Vector2(), renderer.ScreenEdgeDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
        }
    }
}
