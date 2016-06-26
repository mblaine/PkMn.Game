using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PkMn.Game.Enums;

namespace PkMn.Game.Controls
{
    public class Menu : Panel
    {
        protected List<MenuItem> items;

        public EventHandler OnSubmit;
        public EventHandler OnCancel;
        public EventHandler OnSelectedItemChanged;

        protected bool canCancel;

        public int SelectedIndex
        {
            get
            {
                for (int i = 0; i < items.Count; i++)
                    if (items[i].Selected)
                        return i;
                return -1;
            }
        }

        public string SelectedText
        {
            get
            {
                int i = SelectedIndex;
                if (i >= 0)
                    return items[i].Text;
                else
                    return null;
            }
        }

        public Menu(PkMnGame parent, int x, int y, int width, int height)
            : base(parent, x, y, width, height)
        {
            items = new List<MenuItem>();
        }

        public override void HandleInput()
        {
            if (InputManager.KeyPressed(Keys.Up) || InputManager.ButtonPressed(Buttons.LeftThumbstickUp) || InputManager.ButtonPressed(Buttons.DPadUp))
                MoveUp();
            if (InputManager.KeyPressed(Keys.Down) || InputManager.ButtonPressed(Buttons.LeftThumbstickDown) || InputManager.ButtonPressed(Buttons.DPadDown))
                MoveDown();

            if (InputManager.KeyPressed(Keys.Space) || InputManager.KeyPressed(Keys.Enter) || InputManager.ButtonPressed(Buttons.A))
                Submit();
            else if ((InputManager.KeyPressed(Keys.Escape) || InputManager.ButtonPressed(Buttons.B)) && canCancel)
                Cancel();
        }

        public virtual void AddMenuItem(string text)
        {
            items.Add(new MenuItem(parent, X + 25, Y + 25 + items.Count * (SpriteManager.CharHeight + 5) , Width - 50, SpriteManager.CharHeight, SpecialCharacters.ReplaceChars(text), items.Count == 0));
        }

        public override void Update(GameTime gameTime)
        {
            foreach (MenuItem item in items)
                item.Update(gameTime);
            base.Update(gameTime);
        }

        public virtual bool TrySelect(int index)
        {
            if (index < items.Count && index >= 0)
            {
                foreach (MenuItem item in items)
                    item.Selected = false;
                items[index].Selected = true;
                return true;
            }
            return false;
        }

        public void Clear()
        {
            items.Clear();
        }

        public void ResetSelected()
        {
            foreach (MenuItem item in items)
                item.Selected = false;
            if (items.Count > 0)
                items[0].Selected = true;
        }

        public void Open(bool canCancel = true)
        {
            state = State.WaitingForInput;
            this.canCancel = canCancel;
            if (OnSelectedItemChanged != null)
                OnSelectedItemChanged(this, null);
        }

        public virtual void MoveDown()
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Selected)
                {
                    if (i == items.Count - 1)
                    {
                        items[i].Selected = false;
                        items[0].Selected = true;
                    }
                    else if (i < items.Count - 1)
                    {
                        items[i].Selected = false;
                        items[i + 1].Selected = true;
                    }

                    if (OnSelectedItemChanged != null)
                        OnSelectedItemChanged(this, null);
                    
                    break;
                }
            }
        }

        public virtual void MoveUp()
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Selected)
                {
                    if (i == 0)
                    {
                        items[i].Selected = false;
                        items[items.Count - 1].Selected = true;
                    }
                    else if (i > 0)
                    {
                        items[i].Selected = false;
                        items[i - 1].Selected = true;
                    }

                    if (OnSelectedItemChanged != null)
                        OnSelectedItemChanged(this, null);
                    break;
                }
            }
        }

        public virtual void Submit()
        {
            state = State.ReadyForNext;
            if (OnSubmit != null)
                OnSubmit(this, null);
        }

        public virtual void Cancel()
        {
            state = State.ReadyForNext;
            if (OnCancel != null)
                OnCancel(this, null);
        }

        public override void UnloadContent()
        {
            foreach (MenuItem item in items)
                item.UnloadContent();
        }

        public override void Draw(GameTime gameTime, GraphicsDevice device, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, device, spriteBatch);

            foreach (MenuItem item in items)
                item.Draw(gameTime, device, spriteBatch, SpriteManager.Font);

            //spriteBatch.DrawString(font, "BELLSPROUT", new Vector2(this.X + 20, this.Y + 20), Color.Black); 
            
        }
    }
}
