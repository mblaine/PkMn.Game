using System.Collections.Generic;
using Microsoft.Xna.Framework;
using PkMn.Model.Enums;

namespace PkMn.Game
{
    public static class Palettes
    {
        public static Color Black = new Color(24, 24, 24);
        public static Color White = new Color(248, 248, 248);

        public static Color Yellow = new Color(255, 226, 127);
        public static Color Lime = new Color(126, 194, 35);
        public static Color Red = new Color(208, 80, 48);
        public static Color Green = new Color(72, 160, 88);
        public static Color Gold = new Color(248, 168, 0);
        public static Color Beige = new Color(240, 208, 120);
        public static Color Crimson = new Color(250, 42, 0);
        public static Color OffWhite = new Color(235, 235, 235);

        public static Dictionary<Palette, Color> Dark = new Dictionary<Palette, Color>()
        {
            {Palette.None, new Color(85, 85, 85)},
            {Palette.Green, new Color(74, 165, 90)},
            {Palette.Red, new Color(214, 82, 49)},
            {Palette.Cyan, new Color(115, 156, 206)},
            {Palette.Yellow, new Color(214, 165, 0)},
            {Palette.Brown, new Color(173, 115, 74)},
            {Palette.Gray, new Color(123, 123, 148)},
            {Palette.Purple, new Color(173, 123, 189)},
            {Palette.Blue, new Color(90, 123, 189)},
            {Palette.Pink, new Color(230, 123, 173)},
            {Palette.Mew, new Color(132, 115, 156)}
        };

        public static Dictionary<Palette, Color> Light = new Dictionary<Palette, Color>()
        {
            {Palette.None, new Color(170, 170, 170)},
            {Palette.Green, new Color(165, 214, 132)},
            {Palette.Red, new Color(255, 165, 82)},
            {Palette.Cyan, new Color(173, 206, 239)},
            {Palette.Yellow, new Color(255, 230, 115)},
            {Palette.Brown, new Color(230, 165, 123)},
            {Palette.Gray, new Color(214, 173, 181)},
            {Palette.Purple, new Color(222, 181, 197)},
            {Palette.Blue, new Color(148, 165, 222)},
            {Palette.Pink, new Color(247, 181, 197)},
            {Palette.Mew, new Color(247, 181, 140)}
        };
    }
}
