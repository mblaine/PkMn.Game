using Microsoft.Xna.Framework;

namespace PkMn.Game.Controls
{
    public class SelfRenderer : MonsterRenderer
    {
        public SelfRenderer(PkMnGame parent, int x, int y, int width, int height)
            : base(parent, x, y, width, height)
        {
            monSprite = SpriteManager.Back;
            MonSpriteRect = new Rectangle(X + SpriteManager.CharWidth * 2, Y + Height - SpriteManager.BackHeight + 16, SpriteManager.BackWidth, SpriteManager.BackHeight);

            FaceCoords = new Vector2(MonSpriteRect.X + MonSpriteRect.Width - 48, MonSpriteRect.Y + 72);
            BeamOriginCoords = new Vector2(MonSpriteRect.X + MonSpriteRect.Width / 2 + 2, MonSpriteRect.Y + MonSpriteRect.Height * 3 / 4);
            ScreenEdgeDirection = -1;
            MonSpriteBottom = MonSpriteRect.Y + MonSpriteRect.Height - 32;
        }

        protected override Rectangle GetSpriteSheetLocation()
        {
            int row = (monster.Species.Number - 1) / 16;
            int col = (monster.Species.Number - 1) % 16;
            return new Rectangle(SpriteManager.BackWidth * col, SpriteManager.BackHeight * row, SpriteManager.BackWidth, SpriteManager.BackHeight);
        }
    }
}
