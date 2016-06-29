using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PkMn.Game.Controls;
using PkMn.Game.Enums;
using PkMn.Instance;

namespace PkMn.Game
{
    public partial class PkMnGame : Microsoft.Xna.Framework.Game
    {
        private const int GameScreenWidth = 690;
        private const int GameScreenHeight = 610;
        private const int WideWindowWidth = 1264;
        private const int WideWindowHeight = 610;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private RenderTarget2D renderTarget;
        private Panel hasFocus;
        private bool running;
        private Menu battleMenu;
        private Menu moveMenu;
        private MoveDisplay moveDisplay;
        private bool supressMoveDisplay;
        private PartyMenu partyMenu;
        private ScrollingTextBox textBox;
        private HUD foeHUD;
        private HUD selfHUD;
        private FoeRenderer foeRenderer;
        private SelfRenderer selfRenderer;

        private ExtraHUD foeExtraHUD;
        private ExtraHUD selfExtraHUD;

        private SpecialEffectManager specialEffectManager;

        private Thread thread;
        private ManualResetEvent syncronize;

        private Trainer player;
        private Trainer rival;
        internal Battle battle;
        private BattleAction selectedAction;
        private bool canSelectMove;
        private Monster selectedMonster;
        private Vector2? screenOffset;
        private bool shifting;

        public PkMnGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            this.Window.Title = "Pokémon";
            graphics.PreferredBackBufferWidth = GameScreenWidth;
            graphics.PreferredBackBufferHeight = GameScreenHeight;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            //this.TargetElapsedTime = new System.TimeSpan(0, 0, 0, 0, 200);
            IsMouseVisible = true;
            running = true;
            supressMoveDisplay = false;
            shifting = false;
            renderTarget = new RenderTarget2D(GraphicsDevice, GameScreenWidth, GameScreenHeight);
            base.Initialize();
        }

        public bool HasFocus(Panel panel)
        {
            return hasFocus == panel;
        }

        public void SetScreenOffset(int x, int y)
        {
            screenOffset = new Vector2(x, y);
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            SpriteManager.LoadContent(this, Content, GraphicsDevice);

            LoadControls();
            
            BeginBattle();
        }

        protected override void UnloadContent()
        {
            SpriteManager.UnloadContent();

            moveMenu.UnloadContent();
            textBox.UnloadContent();
            battleMenu.UnloadContent();
            moveDisplay.UnloadContent();
            partyMenu.UnloadContent();

            if (thread != null && thread.ThreadState != ThreadState.Stopped)
                thread.Abort();

            if (syncronize != null)
                syncronize.Dispose();

            renderTarget.Dispose();
        }

        protected override void Update(GameTime gameTime)
        {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                //this.Exit();

            screenOffset = null;

            InputManager.Update();

            /*if (InputManager.KeyPressed(Microsoft.Xna.Framework.Input.Keys.D1))
                SpriteManager.SetPalette(ScreenPalette.Normal);
            else if (InputManager.KeyPressed(Microsoft.Xna.Framework.Input.Keys.D2))
                SpriteManager.SetPalette(ScreenPalette.Darker1);
            else if (InputManager.KeyPressed(Microsoft.Xna.Framework.Input.Keys.D3))
                SpriteManager.SetPalette(ScreenPalette.Darker2);
            else if (InputManager.KeyPressed(Microsoft.Xna.Framework.Input.Keys.D4))
                SpriteManager.SetPalette(ScreenPalette.Darkest);
            else if (InputManager.KeyPressed(Microsoft.Xna.Framework.Input.Keys.D5))
                SpriteManager.SetPalette(ScreenPalette.Lighter1);
            else if (InputManager.KeyPressed(Microsoft.Xna.Framework.Input.Keys.D6))
                SpriteManager.SetPalette(ScreenPalette.Lighter2);
            else if (InputManager.KeyPressed(Microsoft.Xna.Framework.Input.Keys.D7))
                SpriteManager.SetPalette(ScreenPalette.Lightest);
            else if (InputManager.KeyPressed(Microsoft.Xna.Framework.Input.Keys.D8))
                SpriteManager.SetPalette(ScreenPalette.DarkerAlt1);
            else if (InputManager.KeyPressed(Microsoft.Xna.Framework.Input.Keys.D9))
                SpriteManager.SetPalette(ScreenPalette.DarkerAlt2);
            else if (InputManager.KeyPressed(Microsoft.Xna.Framework.Input.Keys.D0))
                SpriteManager.SetPalette(ScreenPalette.Inverted);
            else if (InputManager.KeyPressed(Microsoft.Xna.Framework.Input.Keys.O))
                SpriteManager.BeginWavyShader();
            else if (InputManager.KeyPressed(Microsoft.Xna.Framework.Input.Keys.P))
                SpriteManager.EndWavyShader();
            else if (InputManager.KeyPressed(Microsoft.Xna.Framework.Input.Keys.K))
                this.TargetElapsedTime = new System.TimeSpan(0, 0, 0, 0, 500);
            else if (InputManager.KeyPressed(Microsoft.Xna.Framework.Input.Keys.L))
                this.TargetElapsedTime = new System.TimeSpan(166667);
            else*/ if (InputManager.KeyPressed(Microsoft.Xna.Framework.Input.Keys.W))
            {
                graphics.PreferredBackBufferWidth = WideWindowWidth;
                graphics.ApplyChanges();
            }
            else if (InputManager.KeyPressed(Microsoft.Xna.Framework.Input.Keys.E))
            {
                graphics.PreferredBackBufferWidth = GameScreenWidth;
                graphics.ApplyChanges();
            }

            if (!running)
                return;

            if (hasFocus != null)
            {
                hasFocus.HandleInput();
                hasFocus.Update(gameTime);
            }

            base.Update(gameTime);
        }

        private void SpriteBatchBegin(Matrix? transform)
        {
            if(transform != null)
                spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, (Matrix)transform);
            else
                spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(renderTarget);

            Matrix? transform = null;

            if(screenOffset != null)
                transform = Matrix.CreateTranslation(screenOffset.Value.X, screenOffset.Value.Y, 0);

            GraphicsDevice.Clear(SpriteManager.BackgroundColor);
            SpriteBatchBegin(transform);
            
            foeHUD.Draw(gameTime, GraphicsDevice, spriteBatch);
            selfHUD.Draw(gameTime, GraphicsDevice, spriteBatch);

            if(specialEffectManager.EffectNeedsToBeDrawnBeforeMon())
                specialEffectManager.Draw(gameTime, GraphicsDevice, spriteBatch);

            SpriteManager.SetShader(foeRenderer.Palette);
            foeRenderer.Draw(gameTime, GraphicsDevice, spriteBatch);
            spriteBatch.End();
            SpriteBatchBegin(transform);
            
            SpriteManager.SetShader(selfRenderer.Palette);
            selfRenderer.Draw(gameTime, GraphicsDevice, spriteBatch);
            spriteBatch.End();
            SpriteBatchBegin(transform);

            if (partyMenu.Visible)
                partyMenu.Draw(gameTime, GraphicsDevice, spriteBatch);

            textBox.Draw(gameTime, GraphicsDevice, spriteBatch);

            if (!specialEffectManager.EffectNeedsToBeDrawnBeforeMon())
            {
                if (specialEffectManager.EffectNeedsMonDefaultPalette(foeRenderer))
                {
                    SpriteManager.SetShader(foeRenderer.Palette, true);
                    specialEffectManager.Draw(gameTime, GraphicsDevice, spriteBatch);
                    spriteBatch.End();
                    SpriteBatchBegin(transform);
                }
                else if (specialEffectManager.EffectNeedsMonDefaultPalette(selfRenderer))
                {
                    SpriteManager.SetShader(selfRenderer.Palette, true);
                    specialEffectManager.Draw(gameTime, GraphicsDevice, spriteBatch);
                    spriteBatch.End();
                    SpriteBatchBegin(transform);
                    
                }
                else
                    specialEffectManager.Draw(gameTime, GraphicsDevice, spriteBatch);
            }
            
            if (battleMenu.Visible)
                battleMenu.Draw(gameTime, GraphicsDevice, spriteBatch);
            
            if (moveMenu.Visible)
                moveMenu.Draw(gameTime, GraphicsDevice, spriteBatch);

            if (moveDisplay.Visible)
                moveDisplay.Draw(gameTime, GraphicsDevice, spriteBatch);

            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Palettes.OffWhite);
            spriteBatch.Begin(SpriteSortMode.Immediate, null);
            SpriteManager.ApplyWavyShader(renderTarget.Width, renderTarget.Height, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            spriteBatch.Draw((Texture2D)renderTarget, new Rectangle(GraphicsDevice.Viewport.Width / 2 - renderTarget.Width / 2, 0, renderTarget.Width, renderTarget.Height), Color.White);
            spriteBatch.End();

            if (graphics.PreferredBackBufferWidth != renderTarget.Width)
            {
                spriteBatch.Begin(SpriteSortMode.Immediate, null);
                selfExtraHUD.Draw(gameTime, GraphicsDevice, spriteBatch);
                foeExtraHUD.Draw(gameTime, GraphicsDevice, spriteBatch);
                spriteBatch.End();
            }
            
            base.Draw(gameTime);
        }
    }
}
