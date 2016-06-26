using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PkMn.Game.Controls;
using PkMn.Game.SpecialEffects;

namespace PkMn.Game.Animations
{
    public class FlameLine : Animation
    {
        public FlameLine(MonsterRenderer renderer, int delay, int param = 0, int startAt = 0, int stopEarly = int.MaxValue)
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
            Texture2D sprite = SpriteManager.EffectSprites["flame-large"];
            
            Rectangle rect;

            rect = new Rectangle((int)renderer.BeamOriginCoords.X - sprite.Width / 2, (int)renderer.BeamOriginCoords.Y - sprite.Height, sprite.Width, sprite.Height);

            for (int i = 0; i < frameCounter / delay; i++)
            {
                spriteBatch.Draw(sprite, rect, null, Color.White, 0f, new Vector2(), renderer.ScreenEdgeDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
                rect.Offset(-renderer.ScreenEdgeDirection * sprite.Width * 5 / 4, renderer.ScreenEdgeDirection * sprite.Height / 2);
            }
        }
    }
}
