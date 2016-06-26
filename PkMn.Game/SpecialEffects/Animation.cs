using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PkMn.Game.Controls;

namespace PkMn.Game.SpecialEffects
{
    public abstract class Animation : ISpecialEffect
    {
        public MonsterRenderer renderer { get; protected set; }
        protected int delay;
        protected int param;
        public readonly int StartAt;
        public readonly int StopEarly;

        public Animation(MonsterRenderer renderer, int delay, int param, int startAt, int stopEarly)
        {
            this.renderer = renderer;
            this.delay = delay;
            this.param = param;
            this.StartAt = startAt;
            this.StopEarly = stopEarly;
        }

        public void Begin()
        {

        }

        public void Step(int frameCounter)
        {
        }

        public abstract bool IsOver(int frameCounter);

        public virtual bool UseMonDefaultPalette(MonsterRenderer renderer)
        {
            return false;
        }

        public virtual bool DrawBeforeMon(int frameCounter)
        {
            return false;
        }

        public void End()
        {
        }

        public abstract void Draw(GameTime gameTime, GraphicsDevice device, SpriteBatch spriteBatch, int frameCounter);
    }
}
