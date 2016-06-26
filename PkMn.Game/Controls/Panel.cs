using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PkMn.Game.Enums;

namespace PkMn.Game.Controls
{
    public abstract class Panel
    {
        public readonly int X;
        public readonly int Y;
        public readonly int Width;
        public readonly int Height;
        protected State state;
        protected PkMnGame parent;

        public virtual bool Visible
        {
            get { return state == State.WaitingForInput || state == State.Animating || state == State.StaticDisplay; }
        }

        public Rectangle Bounds
        {
            get { return new Rectangle(X, Y, Width, Height); }
        }

        public Panel(PkMnGame parent, int x, int y, int width, int height)
        {
            this.parent = parent;
            X = x;
            Y = y;
            Width = width;
            Height = height;
            state = State.ReadyForNext;
        }

        public virtual void LoadContent(GraphicsDevice device)
        {
        }

        public virtual void UnloadContent()
        {
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void HandleInput()
        {
        }

        public virtual void Draw(GameTime gameTime, GraphicsDevice device, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(SpriteManager.White, Bounds, SpriteManager.ForegroundColor);

            Rectangle interior = Bounds;
            interior.Inflate(-4, -4);
            spriteBatch.Draw(SpriteManager.White, interior, SpriteManager.BackgroundColor);

            interior.Inflate(-4, -4);
            spriteBatch.Draw(SpriteManager.White, interior, SpriteManager.ForegroundColor);

            interior.Inflate(-4, -4);
            spriteBatch.Draw(SpriteManager.White, interior, SpriteManager.BackgroundColor);
        }

    }
}
