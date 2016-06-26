using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PkMn.Game.Enums;

namespace PkMn.Game.Controls
{
    public class ScrollingTextBox : Panel
    {
        public EventHandler OnClose;

        private Queue<string> queue;
        private Queue<string> currentParagraph;
        private string currentText;

        private int maxLineLength;
        private long frameCounter;
        private int animationCounter;

        private bool staticDisplayNext;

        public ScrollingTextBox(PkMnGame parent, int x, int y, int width, int height)
            : base(parent, x, y, width, height)
        {
            queue = new Queue<string>();
            frameCounter = 0;
            state = State.ReadyForNext;
            staticDisplayNext = false;
        }

        public override void HandleInput()
        {
            if (InputManager.KeyPressed(Keys.Space) || InputManager.KeyPressed(Keys.Enter) || InputManager.ButtonPressed(Buttons.A) || InputManager.ButtonPressed(Buttons.B))
                Advance();
        }

        public void QueueText(string text, params object[] parameters)
        {
            if (state == State.StaticDisplay)
                Clear();
            queue.Enqueue(string.Format(text, parameters));
            staticDisplayNext = false;
        }

        public void QueueStaticText(string text, params object[] parameters)
        {
            if (state == State.StaticDisplay)
                Clear();
            queue.Enqueue(string.Format(text, parameters));
            staticDisplayNext = true;
        }

        public void Clear()
        {
            queue.Clear();
            state = State.ReadyForNext;
            if (OnClose != null)
                OnClose(this, null);
        }

        public void Advance()
        {
            if (state == State.WaitingForInput)
            {
                if (currentParagraph.Count > 0)
                {
                    currentText = currentParagraph.Dequeue();
                    state = State.Animating;
                    animationCounter = 0;
                }
                else
                {
                    state = State.ReadyForNext;
                    if(queue.Count == 0 && OnClose != null)
                        OnClose(this, null);
                }
            }
        }

        private string[] FormatText(string text)
        {
            List<string> ret = new List<string>();
            string line = "";
            foreach (string token in text.Split(' '))
            {
                if (line.Length + 1 + token.Length <= maxLineLength)
                {
                    if (line.Length == 0)
                        line = token;
                    else
                        line += " " + token;
                }
                else
                {
                    ret.Add(line.Trim());
                    line = token;
                }
            }

            if (line.Length > 0)
                ret.Add(line.Trim());

            return ret.ToArray();
        }

        public override void LoadContent(GraphicsDevice device)
        {
            maxLineLength = (Width - 50) / SpriteManager.CharWidth;
        }

        public override void UnloadContent()
        {
        }

        public override void Update(GameTime gameTime)
        {
            frameCounter++;

            if (state == State.ReadyForNext && queue.Count > 0 && (currentParagraph == null || currentParagraph.Count == 0))
            {
                currentParagraph = new Queue<string>();
                string[] lines = FormatText(SpecialCharacters.ReplaceChars(queue.Dequeue()));
                for (int i = 0; i < lines.Length; i += 2)
                {
                    if (i < lines.Length - 1)
                        currentParagraph.Enqueue(lines[i] + Environment.NewLine + lines[i + 1]);
                    else
                        currentParagraph.Enqueue(lines[i]);
                }
                currentText = currentParagraph.Dequeue();
                state = State.Animating;
                animationCounter = 0;
            }            
        }

        public override void Draw(GameTime gameTime, GraphicsDevice device, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, device, spriteBatch);

            if (currentText != null)
            {
                string[] lines = currentText.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                if (state == State.Animating)
                {
                    animationCounter++;
                    int charactersToShow = (animationCounter / 3 + 1) * 3;

                    if (charactersToShow <= lines[0].Length)
                        lines = new string[] { lines[0].Substring(0, charactersToShow), "" };
                    else if (lines.Length > 1 && charactersToShow - lines[0].Length <= lines[1].Length)
                        lines = new string[] { lines[0], lines[1].Substring(0, charactersToShow - lines[0].Length) };
                    else
                    {
                        if (staticDisplayNext)
                        {
                            state = State.StaticDisplay;
                            if (OnClose != null)
                                OnClose(this, null);
                        }
                        else
                        {
                            state = State.WaitingForInput;
                        }
                    }
                }
                else
                {
                    if (parent.HasFocus(this) && frameCounter % 60 > 30 && state != State.StaticDisplay)
                    {
                        if (lines.Length > 1)
                            lines[1] = lines[1].PadRight(maxLineLength).Substring(0, maxLineLength - 1) + SpecialCharacters.DownArrow;
                        else
                            lines = new string[] { lines[0], new string(' ', maxLineLength - 1) + SpecialCharacters.DownArrow };
                    }
                }

                if(state != State.ReadyForNext)
                    spriteBatch.DrawString(SpriteManager.Font, string.Join(Environment.NewLine, lines), new Vector2(this.X + 25, this.Y + 52), SpriteManager.ForegroundColor);
            }

        }
    }
}
