using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PkMn.Instance;

namespace PkMn.Game.Controls
{
    public class PartyMenu : Menu
    {
        private int itemHeight;

        public PartyMenu(PkMnGame parent, int x, int y, int width, int height)
            : base(parent, x, y, width, height)
        {
            itemHeight = Height / 6;
        }

        public void AddMenuItem(Monster monster)
        {
            items.Add(new PartyMenuItem(parent, X, Y + items.Count * itemHeight, Width, itemHeight, monster, items.Count == 0));
        }

        public override void Draw(GameTime gameTime, GraphicsDevice device, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(SpriteManager.White, Bounds, SpriteManager.BackgroundColor);

            foreach (MenuItem item in items)
                item.Draw(gameTime, device, spriteBatch, SpriteManager.Font);
        }
    }
}
