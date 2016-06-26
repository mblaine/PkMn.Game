using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PkMn.Game.Controls
{
    public class MenuItem : Panel
    {
        public string Text;
        public bool Selected;

        public MenuItem(PkMnGame parent, int x, int y, int width, int height, string text, bool selected)
            : base(parent, x, y, width, height)
        {
            Text = text;
            Selected = selected;
        }

        public virtual void Draw(GameTime gameTime, GraphicsDevice device, SpriteBatch spriteBatch, SpriteFont font)
        {
            spriteBatch.DrawString(font, Selected ? SpecialCharacters.RightArrow + Text : " " + Text, new Vector2(X, Y), SpriteManager.ForegroundColor);
        }
    }
}
