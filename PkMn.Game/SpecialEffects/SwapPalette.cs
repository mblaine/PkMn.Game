using PkMn.Game.Enums;

namespace PkMn.Game.SpecialEffects
{
    public class SwapPalette : ISpecialEffect
    {
        private ScreenPalette palette;

        private static ScreenPalette savedPalette;

        public SwapPalette(ScreenPalette palette)
        {
            this.palette = palette;
        }

        public void Begin()
        {
            if (palette == ScreenPalette.Set)
                savedPalette = SpriteManager.GetPalette();
            else if(palette == ScreenPalette.Reset)
                SpriteManager.SetPalette(savedPalette);
            else
                SpriteManager.SetPalette(palette);
        }

        public void Step(int frameCounter)
        {
        }

        public bool IsOver(int frameCounter)
        {
            return true;
        }

        public void End()
        {
        }
    }
}
