using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PkMn.Game.Enums;
using PkMn.Instance;
using PkMn.Model.Enums;

namespace PkMn.Game.Controls
{
    public abstract class MonsterRenderer : Panel
    {
        public delegate void TransformMonster(MonsterRenderer renderer, ref Rectangle drawFrom, ref Rectangle drawTo);

        public TransformMonster OnTransformMonster;

        protected ActiveMonster monster;

        protected Texture2D monSprite;
        public Rectangle MonSpriteRect { get; protected set; }

        public SpritePosition Position;
        public SpriteDisplay Display;

        public Vector2 FaceCoords { get; protected set; }
        public Vector2 BeamOriginCoords { get; protected set; }
        public int ScreenEdgeDirection { get; protected set; }
        public int MonSpriteBottom { get; protected set; }

        public Palette Palette
        {
            get
            {
                if (monster != null && monster.Monster != null)
                    return monster.Monster.Species.Palette;
                else
                    return Palette.None;
            }
        }

        public MonsterRenderer(PkMnGame parent, int x, int y, int width, int height)
            : base(parent, x, y, width, height)
        {
            Position = SpritePosition.Hidden;
            Display = SpriteDisplay.Normal;
        }

        protected abstract Rectangle GetSpriteSheetLocation();

        public void SetMonster(ActiveMonster monster)
        {
            this.monster = monster;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, GraphicsDevice device, SpriteBatch spriteBatch)
        {
            if (monster.Monster == null || Position == SpritePosition.Hidden)
                return;

            try
            {
                Rectangle spriteSheetRect = GetSpriteSheetLocation();

                Rectangle drawFrom = spriteSheetRect;
                Rectangle drawTo = MonSpriteRect;

                Texture2D sprite = monSprite;

                SpriteEffects flip = SpriteEffects.None;

                if (Display == SpriteDisplay.Ball)
                {
                    sprite = SpriteManager.EffectSprites["ball-large"];
                    if (ScreenEdgeDirection < 0)
                        flip = SpriteEffects.FlipHorizontally;
                    drawTo = new Rectangle(drawTo.X + drawTo.Width / 2 - sprite.Width / 2, drawTo.Y + drawTo.Height / 2 - sprite.Height / 2, sprite.Width, sprite.Height);
                    drawFrom = new Rectangle(0, 0, sprite.Width, sprite.Height);
                }
                else if (Display == SpriteDisplay.Minimized)
                {
                    sprite = SpriteManager.EffectSprites["minimized"];
                    drawTo = new Rectangle(drawTo.X + drawTo.Width / 2 - sprite.Width / 2, drawTo.Y + drawTo.Height / 2 - sprite.Height / 2, sprite.Width, sprite.Height);
                    drawFrom = new Rectangle(0, 0, sprite.Width, sprite.Height);
                }
                else if (Display == SpriteDisplay.Substitute)
                {
                    sprite = SpriteManager.EffectSprites["substitute"];
                    drawTo = new Rectangle(drawTo.X + drawTo.Width / 2 - sprite.Height / 2, drawTo.Y + drawTo.Height / 2 - sprite.Height / 2, sprite.Height, sprite.Height);
                    drawFrom = new Rectangle(ScreenEdgeDirection > 0 ? 0 : sprite.Height, 0, sprite.Height, sprite.Height);
                }

                if (Position == SpritePosition.MovedForward)
                    drawTo.Offset(-32 * ScreenEdgeDirection, 0);
                else if (Position == SpritePosition.MovedBackward)
                    drawTo.Offset(144 * ScreenEdgeDirection, 0);

                if (OnTransformMonster != null)
                    OnTransformMonster(this, ref drawFrom, ref drawTo);

                spriteBatch.Draw(sprite, drawTo, drawFrom, Color.White, 0f, new Vector2(), flip, 0f);

            }
            catch (NullReferenceException)
            {
            }
        }
    }
}
