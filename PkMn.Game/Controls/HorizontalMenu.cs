using Microsoft.Xna.Framework.Input;

namespace PkMn.Game.Controls
{
    public class HorizontalMenu : Menu
    {

        public HorizontalMenu(PkMnGame parent, int x, int y, int width, int height)
            : base(parent, x, y, width, height)
        {
        }

        public override void AddMenuItem(string text)
        {
            if(items.Count % 2 == 0)
                items.Add(new MenuItem(parent, X + 25, Y + 50 + (items.Count / 2) * (SpriteManager.CharHeight + 30), (Width - 50) * 55 / 100, SpriteManager.CharHeight, text, items.Count == 0));
            else
                items.Add(new MenuItem(parent, items[0].X + items[0].Width + 20, Y + 50 + (items.Count / 2) * (SpriteManager.CharHeight + 30), (Width - 50) * 45 / 100, SpriteManager.CharHeight, SpecialCharacters.ReplaceChars(text), items.Count == 0));
        }

        public override void HandleInput()
        {
            if (InputManager.KeyPressed(Keys.Left) || InputManager.ButtonPressed(Buttons.LeftThumbstickLeft) || InputManager.ButtonPressed(Buttons.DPadLeft))
                MoveLeft();
            if (InputManager.KeyPressed(Keys.Right) || InputManager.ButtonPressed(Buttons.LeftThumbstickRight) || InputManager.ButtonPressed(Buttons.DPadRight))
                MoveRight();
            base.HandleInput();
        }

        public override void MoveUp()
        {
            int i = SelectedIndex;
            if (i > 1)
            {
                items[i].Selected = false;
                items[i - 2].Selected = true;
                if (OnSelectedItemChanged != null)
                    OnSelectedItemChanged(this, null);
            }
        }

        public override void MoveDown()
        {
            int i = SelectedIndex;
            if (i < items.Count - 2)
            {
                items[i].Selected = false;
                items[i + 2].Selected = true;
                if (OnSelectedItemChanged != null)
                    OnSelectedItemChanged(this, null);
            }

        }

        public void MoveLeft()
        {
            int i = SelectedIndex;
            if (i % 2 == 1)
            {
                items[i].Selected = false;
                items[i - 1].Selected = true;
                if (OnSelectedItemChanged != null)
                    OnSelectedItemChanged(this, null);
            }

        }

        public void MoveRight()
        {
            int i = SelectedIndex;
            if (i % 2 == 0 && i < items.Count - 1)
            {
                items[i].Selected = false;
                items[i + 1].Selected = true;
                if (OnSelectedItemChanged != null)
                    OnSelectedItemChanged(this, null);
            }
        }

        public override void Cancel()
        {
            //can't cancel
        }
    }
}
