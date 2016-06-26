using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PkMn.Game.Enums;

namespace PkMn.Game.Controls
{
    public class MoveDisplay : Panel
    {

        private string type;
        private int currentPP;
        private int maxPP;
        private bool disabled;

        public MoveDisplay(PkMnGame parent, int x, int y, int width, int height)
            : base(parent, x, y, width, height)
        {
        }

        public void Show(string type, int currentPP, int maxPP, bool disabled = false)
        {
            this.type = type;
            this.currentPP = currentPP;
            this.maxPP = maxPP;
            this.disabled = disabled;
            this.state = State.StaticDisplay;
        }

        public void Hide()
        {
            state = State.ReadyForNext;
        }

        public override void Draw(GameTime gameTime, GraphicsDevice device, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, device, spriteBatch);
            spriteBatch.DrawString(SpriteManager.Font, "TYPE/", new Vector2(this.X + 15, this.Y + 17), SpriteManager.ForegroundColor);
            spriteBatch.DrawString(SpriteManager.Font, " " + type, new Vector2(this.X + 15, this.Y + 17 + SpriteManager.CharHeight), SpriteManager.ForegroundColor);
            if (disabled)
                spriteBatch.DrawString(SpriteManager.Font, " disabled", new Vector2(this.X + 15, this.Y + 17 + SpriteManager.CharHeight * 2), SpriteManager.ForegroundColor);
            else
                spriteBatch.DrawString(SpriteManager.Font, string.Format("{0,2}/{1,2}", currentPP, maxPP).PadLeft(9, ' '), new Vector2(this.X + 15, this.Y + 17 + SpriteManager.CharHeight * 2), SpriteManager.ForegroundColor);
        }


    }
}
