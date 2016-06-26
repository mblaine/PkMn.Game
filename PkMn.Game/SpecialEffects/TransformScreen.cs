using PkMn.Game.Enums;

namespace PkMn.Game.SpecialEffects
{
    public class TransformScreen : ISpecialEffect
    {        
        private PkMnGame game;
        private ScreenTransformation transformation;
        private int offset;
        public readonly bool Parallel;

        public TransformScreen(PkMnGame game, ScreenTransformation transformation, int offset, bool parallel)
        {
            this.game = game;
            this.transformation = transformation;
            this.offset = offset;
            this.Parallel = parallel;
        }

        public void Begin()
        {
            if (transformation == ScreenTransformation.WavyBegin)
                SpriteManager.BeginWavyShader();
            else if (transformation == ScreenTransformation.WavyEnd)
                SpriteManager.EndWavyShader();
        }

        public void Step(int frameCounter)
        {
            if (transformation == ScreenTransformation.ShakeHorizontallyFast || transformation == ScreenTransformation.ShakeVertically)
            {
                int currentOffset = offset - frameCounter / 8;

                if (frameCounter % 8 >= 4)
                    currentOffset = 0;

                if(transformation == ScreenTransformation.ShakeHorizontallyFast)
                    game.SetScreenOffset(currentOffset * 4, 0);
                else
                    game.SetScreenOffset(0, currentOffset * 4);
            }
            else if (transformation == ScreenTransformation.ShakeHorizontallySlow)
            {
                int currentOffset = (frameCounter / 2) % (offset * 2);
                if (currentOffset > offset)
                    currentOffset = offset * 2 - currentOffset;

                game.SetScreenOffset(currentOffset * 4, 0);
            }
        }

        public bool IsOver(int frameCounter)
        {
            switch (transformation)
            {
                case ScreenTransformation.ShakeHorizontallyFast:
                case ScreenTransformation.ShakeVertically:
                    if (frameCounter < 8 * offset)
                        return false;
                    break;
                case ScreenTransformation.ShakeHorizontallySlow:
                    if (frameCounter < 8 * offset)
                        return false;
                    break;
            }

            return true;
        }

        public void End()
        {
        }
    }
}
