
namespace PkMn.Game.SpecialEffects
{
    public class Delay : ISpecialEffect
    {
        private int frames;

        public Delay(int frames)
        {
            this.frames = frames;
        }
        

        public void Begin()
        {
        }

        public void Step(int frameCounter)
        {
        }

        public bool IsOver(int frameCounter)
        {
            if (frameCounter <= frames)
                return false;
            return true;
        }

        public void End()
        {
        }
    }
}
