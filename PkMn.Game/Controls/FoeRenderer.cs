using Microsoft.Xna.Framework;

namespace PkMn.Game.Controls
{
    public class FoeRenderer : MonsterRenderer
    {
        public FoeRenderer(PkMnGame parent, int x, int y, int width, int height)
            : base(parent, x, y, width, height)
        {
            monSprite = SpriteManager.Front;
            MonSpriteRect = new Rectangle(X + Width - SpriteManager.FrontWidth - SpriteManager.CharWidth * 2, Y + 10, SpriteManager.FrontWidth, SpriteManager.FrontHeight);

            FaceCoords = new Vector2(MonSpriteRect.X + 48, MonSpriteRect.Y + 72);
            BeamOriginCoords = new Vector2(MonSpriteRect.X + MonSpriteRect.Width / 2, MonSpriteRect.Y + MonSpriteRect.Height);
            ScreenEdgeDirection = 1;
            MonSpriteBottom = MonSpriteRect.Y + MonSpriteRect.Height;
        }

        protected override Rectangle GetSpriteSheetLocation()
        {
            int row = (monster.Species.Number - 1) / 16;
            int col = (monster.Species.Number - 1) % 16;
            return new Rectangle(SpriteManager.FrontWidth * col, SpriteManager.FrontHeight * row, SpriteManager.FrontWidth, SpriteManager.FrontHeight);
        }
    }
}
