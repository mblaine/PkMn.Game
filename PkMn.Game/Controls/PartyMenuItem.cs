using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PkMn.Instance;

namespace PkMn.Game.Controls
{
    public class PartyMenuItem : MenuItem
    {
        private Monster monster;
        private long frameCounter;

        public PartyMenuItem(PkMnGame parent, int x, int y, int width, int height, Monster monster, bool selected)
            : base(parent, x, y, width, height, monster.Name, selected)
        {
            this.monster = monster;
            frameCounter = 0;
        }

        public override void Update(GameTime gameTime)
        {
            frameCounter++;
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, GraphicsDevice device, SpriteBatch spriteBatch, SpriteFont font)
        {
            if (Selected)
                spriteBatch.DrawString(font, SpecialCharacters.RightArrow, new Vector2(X, Y + Height - SpriteManager.CharHeight - 20), SpriteManager.ForegroundColor);
            
            decimal hpPercent = ((decimal)monster.CurrentHP) / monster.Stats.HP;
            
            bool onFrame2 = false;
            if (hpPercent > 0.5m)
                onFrame2 = frameCounter % 10 > 5;
            else if (hpPercent > 0.2m)
                onFrame2 = frameCounter % 30 > 15;
            else
                onFrame2 = frameCounter % 60 > 30;

            Rectangle iconSource;
            if(Selected && onFrame2)
                iconSource = new Rectangle(SpriteManager.PartyIconWidth, SpriteManager.PartyIconHeight * (int)monster.Species.BodyType, SpriteManager.PartyIconWidth, SpriteManager.PartyIconHeight);
            else
                iconSource = new Rectangle(0, SpriteManager.PartyIconHeight * (int)monster.Species.BodyType, SpriteManager.PartyIconWidth, SpriteManager.PartyIconHeight);

            spriteBatch.Draw(SpriteManager.PartyIcons, new Rectangle(X + SpriteManager.CharWidth, Y, SpriteManager.PartyIconWidth, SpriteManager.PartyIconHeight), iconSource, Color.White);

            spriteBatch.DrawString(font, SpecialCharacters.ReplaceChars(monster.Name).PadRight(10, ' ') + string.Format("{0}{1}", SpecialCharacters.ColonL, monster.Level).PadRight(4, ' ') + monster.StatusText, new Vector2(X + 100, Y), SpriteManager.ForegroundColor);
            spriteBatch.DrawString(font, string.Format("{0,3}/{1,3}", monster.CurrentHP, monster.Stats.HP).PadLeft(17, ' '), new Vector2(X + 100, Y + SpriteManager.CharHeight), SpriteManager.ForegroundColor);

            spriteBatch.Draw(SpriteManager.HPBar, new Rectangle(X + 136, Y + SpriteManager.CharHeight + 8, SpriteManager.HPBar.Width, SpriteManager.HPBar.Height), SpriteManager.ForegroundColor);

            Color color = hpPercent > 0.5m ? SpriteManager.HPBarHighColor : hpPercent > 0.2m ? SpriteManager.HPBarMediumColor : SpriteManager.HPBarLowColor;
            spriteBatch.Draw(SpriteManager.White, new Rectangle(X + 196, Y + SpriteManager.CharHeight + 12, (int)(192 * hpPercent), 8), color);

        }
    }
}
