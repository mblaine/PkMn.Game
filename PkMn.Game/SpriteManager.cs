using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PkMn.Game.Enums;
using PkMn.Model.Enums;

namespace PkMn.Game
{
    public static class SpriteManager
    {
        private static ScreenPalette currentPalette;
        private static bool wavyShader;
        private static int wavyFrameCount;

        private static PkMnGame game;

        internal static SpriteFont Font;
        internal static SpriteFont FontSmall;

        internal static Texture2D Black;
        internal static Texture2D White;
        internal static Texture2D HudSelf;
        internal static Texture2D HudFoe;
        internal static Texture2D HudFrame;
        internal static Texture2D Balls;
        internal static Texture2D HPBar;
        internal static Texture2D PartyIcons;
        internal static Texture2D HudSmall;

        internal static Texture2D Front;
        internal static Texture2D Back;

        internal static Dictionary<string, Texture2D> EffectSprites;

        internal static Effect ShaderColor;
        internal static Effect ShaderWavy;

        internal static int CharWidth = 32;
        internal static int CharHeight = 32;

        internal static int CharWidthSm = 16;
        internal static int CharHeightSm = 16;

        internal static int FrontWidth = 224;
        internal static int FrontHeight = 224;
        internal static int BackWidth = 256;
        internal static int BackHeight = 256;
        internal static int PartyIconWidth = 64;
        internal static int PartyIconHeight = 64;

        internal static Color ForegroundColor
        {
            get
            {
                switch (currentPalette)
                {
                    case ScreenPalette.DarkerAlt2:
                    case ScreenPalette.Lighter2:
                        if (game.battle.PlayerCurrent.Monster != null)
                            return Palettes.Light[game.battle.PlayerCurrent.Monster.Species.Palette];
                        else
                            return Palettes.Light[Palette.None];
                    case ScreenPalette.Lighter1:
                        if (game.battle.PlayerCurrent.Monster != null)
                            return Palettes.Dark[game.battle.PlayerCurrent.Monster.Species.Palette];
                        else
                            return Palettes.Dark[Palette.None];
                    case ScreenPalette.Inverted:
                    case ScreenPalette.Lightest:
                        return Palettes.White;
                    default:
                        return Palettes.Black;
                }
            }
        }

        internal static Color BackgroundColor
        {
            get 
            {
                switch (currentPalette)
                {
                    case ScreenPalette.DarkerAlt2:
                    case ScreenPalette.Inverted:
                    case ScreenPalette.Darkest:
                        return Palettes.Black;
                    default:
                        return Palettes.White;
                }
            }
        }

        internal static Color HudColor
        {
            get
            {
                switch (currentPalette)
                {
                    case ScreenPalette.DarkerAlt2:
                        return Palettes.Beige;
                    case ScreenPalette.Lighter2:
                        return Palettes.Yellow;
                    case ScreenPalette.Lighter1:
                        return Palettes.Green;
                    case ScreenPalette.Inverted:
                    case ScreenPalette.Lightest:
                        return Palettes.White;
                    default:
                        return Palettes.Black;
                }
            }
        }

        internal static Color HPBarLowColor
        {
            get
            {
                switch (currentPalette)
                {
                    case ScreenPalette.Lighter1:
                    case ScreenPalette.Inverted:
                        return Palettes.Yellow;
                    case ScreenPalette.Lighter2:
                    case ScreenPalette.Lightest:
                        return Palettes.White;
                    case ScreenPalette.Darker1:
                    case ScreenPalette.Darker2:
                    case ScreenPalette.Darkest:
                    case ScreenPalette.DarkerAlt1:
                        return Palettes.Black;
                    default:
                        return Palettes.Red;
                }
            }
        }

        internal static Color HPBarMediumColor
        {
            get
            {
                switch (currentPalette)
                {
                    case ScreenPalette.Lighter1:
                    case ScreenPalette.Inverted:
                        return Palettes.Yellow;
                    case ScreenPalette.Lighter2:
                    case ScreenPalette.Lightest:
                        return Palettes.White;
                    case ScreenPalette.Darker1:
                    case ScreenPalette.Darker2:
                    case ScreenPalette.Darkest:
                    case ScreenPalette.DarkerAlt1:
                        return Palettes.Black;
                    default:
                        return Palettes.Gold;
                }
            }
        }

        internal static Color HPBarHighColor
        {
            get
            {
                switch (currentPalette)
                {
                    case ScreenPalette.Lighter1:
                    case ScreenPalette.Inverted:
                        return Palettes.Yellow;
                    case ScreenPalette.Lighter2:
                    case ScreenPalette.Lightest:
                        return Palettes.White;
                    case ScreenPalette.Darker1:
                    case ScreenPalette.Darker2:
                    case ScreenPalette.Darkest:
                    case ScreenPalette.DarkerAlt1:
                        return Palettes.Black;
                    default:
                        return Palettes.Green;
                }
            }
        }
        
        internal static void LoadContent(PkMnGame game, ContentManager content, GraphicsDevice graphicsDevice)
        {
            currentPalette = ScreenPalette.Normal;
            wavyShader = false;

            SpriteManager.game = game;

            Font = content.Load<SpriteFont>("font");
            Font.LineSpacing += 28;

            FontSmall = content.Load<SpriteFont>("font-small");

            Black = new Texture2D(graphicsDevice, 1, 1);
            Black.SetData(new Color[] { Color.Black });

            White = new Texture2D(graphicsDevice, 1, 1);
            White.SetData(new Color[] { Color.White });

            HudSelf = content.Load<Texture2D>("hud-self");
            HudFoe = content.Load<Texture2D>("hud-foe");
            HudFrame = content.Load<Texture2D>("hud-frame");
            Balls = content.Load<Texture2D>("balls");
            HPBar = content.Load<Texture2D>("hp-bar");
            Front = content.Load<Texture2D>("front");
            Back = content.Load<Texture2D>("back");
            PartyIcons = content.Load<Texture2D>("party-icons");
            HudSmall = content.Load<Texture2D>("hud-small");

            EffectSprites = new Dictionary<string, Texture2D>();
            foreach (string file in Directory.GetFiles(Path.Combine(content.RootDirectory, "effect-sprites")))
            {
                EffectSprites[Path.GetFileNameWithoutExtension(file)] = content.Load<Texture2D>(Path.Combine("effect-sprites", Path.GetFileNameWithoutExtension(file)));
            }

            ShaderColor = content.Load<Effect>("shader-color");
            ShaderWavy = content.Load<Effect>("shader-wavy");
        }

        internal static void UnloadContent()
        {
            Black.Dispose();
            White.Dispose();
            HudSelf.Dispose();
            HudFoe.Dispose();
            HudFrame.Dispose();
            Balls.Dispose();
            HPBar.Dispose();
            Front.Dispose();
            Back.Dispose();
            PartyIcons.Dispose();
            HudSmall.Dispose();

            foreach (Texture2D texture in EffectSprites.Values)
                texture.Dispose();

            ShaderColor.Dispose();
            ShaderWavy.Dispose();
        }

        internal static void SetPalette(ScreenPalette palette)
        {
            currentPalette = palette;
        }

        internal static ScreenPalette GetPalette()
        {
            return currentPalette;
        }

        public static void SetShader(Palette palette, bool normalOverride = false)
        {
            if (normalOverride)
            {
                ShaderColor.Parameters["black"].SetValue(Palettes.Black.ToVector4());
                ShaderColor.Parameters["darkGray"].SetValue(Palettes.Dark[palette].ToVector4());
                ShaderColor.Parameters["lightGray"].SetValue(Palettes.Light[palette].ToVector4());
                ShaderColor.Parameters["white"].SetValue(Palettes.White.ToVector4());
            }
            else
            {

                switch (currentPalette)
                {
                    case ScreenPalette.Darkest:
                        ShaderColor.Parameters["black"].SetValue(Palettes.Black.ToVector4());
                        ShaderColor.Parameters["darkGray"].SetValue(Palettes.Black.ToVector4());
                        ShaderColor.Parameters["lightGray"].SetValue(Palettes.Black.ToVector4());
                        ShaderColor.Parameters["white"].SetValue(Palettes.Black.ToVector4());
                        break;
                    case ScreenPalette.Darker2:
                        ShaderColor.Parameters["black"].SetValue(Palettes.Black.ToVector4());
                        ShaderColor.Parameters["darkGray"].SetValue(Palettes.Black.ToVector4());
                        ShaderColor.Parameters["lightGray"].SetValue(Palettes.Black.ToVector4());
                        ShaderColor.Parameters["white"].SetValue(Palettes.White.ToVector4());
                        break;
                    case ScreenPalette.Darker1:
                        ShaderColor.Parameters["black"].SetValue(Palettes.Black.ToVector4());
                        ShaderColor.Parameters["darkGray"].SetValue(Palettes.Black.ToVector4());
                        ShaderColor.Parameters["lightGray"].SetValue(Palettes.Dark[palette].ToVector4());
                        ShaderColor.Parameters["white"].SetValue(Palettes.White.ToVector4());
                        break;
                    case ScreenPalette.Lighter1:
                        ShaderColor.Parameters["black"].SetValue(Palettes.Dark[palette].ToVector4());
                        ShaderColor.Parameters["darkGray"].SetValue(Palettes.Light[palette].ToVector4());
                        ShaderColor.Parameters["lightGray"].SetValue(Palettes.White.ToVector4());
                        ShaderColor.Parameters["white"].SetValue(Palettes.White.ToVector4());
                        break;
                    case ScreenPalette.Lighter2:
                        ShaderColor.Parameters["black"].SetValue(Palettes.Light[palette].ToVector4());
                        ShaderColor.Parameters["darkGray"].SetValue(Palettes.White.ToVector4());
                        ShaderColor.Parameters["lightGray"].SetValue(Palettes.White.ToVector4());
                        ShaderColor.Parameters["white"].SetValue(Palettes.White.ToVector4());
                        break;
                    case ScreenPalette.Lightest:
                        ShaderColor.Parameters["black"].SetValue(Palettes.White.ToVector4());
                        ShaderColor.Parameters["darkGray"].SetValue(Palettes.White.ToVector4());
                        ShaderColor.Parameters["lightGray"].SetValue(Palettes.White.ToVector4());
                        ShaderColor.Parameters["white"].SetValue(Palettes.White.ToVector4());
                        break;
                    case ScreenPalette.DarkerAlt1:
                        ShaderColor.Parameters["black"].SetValue(Palettes.Black.ToVector4());
                        ShaderColor.Parameters["darkGray"].SetValue(Palettes.Black.ToVector4());
                        ShaderColor.Parameters["lightGray"].SetValue(Palettes.Light[palette].ToVector4());
                        ShaderColor.Parameters["white"].SetValue(Palettes.White.ToVector4());
                        break;
                    case ScreenPalette.DarkerAlt2:
                        ShaderColor.Parameters["black"].SetValue(Palettes.Light[palette].ToVector4());
                        ShaderColor.Parameters["darkGray"].SetValue(Palettes.Dark[palette].ToVector4());
                        ShaderColor.Parameters["lightGray"].SetValue(Palettes.Black.ToVector4());
                        ShaderColor.Parameters["white"].SetValue(Palettes.Black.ToVector4());
                        break;
                    case ScreenPalette.Inverted:
                        ShaderColor.Parameters["black"].SetValue(Palettes.White.ToVector4());
                        ShaderColor.Parameters["darkGray"].SetValue(Palettes.Light[palette].ToVector4());
                        ShaderColor.Parameters["lightGray"].SetValue(Palettes.Dark[palette].ToVector4());
                        ShaderColor.Parameters["white"].SetValue(Palettes.Black.ToVector4());
                        break;
                    default:
                        ShaderColor.Parameters["black"].SetValue(Palettes.Black.ToVector4());
                        ShaderColor.Parameters["darkGray"].SetValue(Palettes.Dark[palette].ToVector4());
                        ShaderColor.Parameters["lightGray"].SetValue(Palettes.Light[palette].ToVector4());
                        ShaderColor.Parameters["white"].SetValue(Palettes.White.ToVector4());
                        break;
                }
            }
            ShaderColor.CurrentTechnique.Passes[0].Apply();
        }

        public static void BeginWavyShader()
        {
            wavyShader = true;
            wavyFrameCount = 0;
        }

        public static void EndWavyShader()
        {
            wavyShader = false;
        }

        public static void ApplyWavyShader(int screenWidth, int screenHeight, int windowWidth, int windowHeight)
        {
            if (!wavyShader)
                return;

            //to use pixel shader version 3.0 we need to manually create a v3.0 vertex shader too; this is so it doesn't actually do anything
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, windowWidth, windowHeight, 0, 0, 1);
            Matrix halfPixelOffset = Matrix.CreateTranslation(-0.5f, -0.5f, 0);
            ShaderWavy.Parameters["matrixTransform"].SetValue(halfPixelOffset * projection);

            float[] offsets = new float[] { 0, 0, 0, 0, 0, 1, 1, 1, 2, 2, 2, 2, 2, 1, 1, 1, 0, 0, 0, 0, 0, -1, -1, -1, -2, -2, -2, -2, -2, -1, -1, -1 };
            offsets = offsets.Skip((wavyFrameCount) % 32).Concat(offsets.Take((wavyFrameCount) % 32)).ToArray();
            
            ShaderWavy.Parameters["screenWidth"].SetValue(screenWidth);
            ShaderWavy.Parameters["screenHeight"].SetValue(screenHeight);
            ShaderWavy.Parameters["lineOffsets"].SetValue(offsets);
            ShaderWavy.CurrentTechnique.Passes[0].Apply();

            wavyFrameCount+=2;
        }
    }
}
