using System;
using Microsoft.Xna.Framework;
using PkMn.Game.Controls;
using PkMn.Game.Enums;

namespace PkMn.Game.SpecialEffects
{
    public class TransformSprite : ISpecialEffect
    {
        protected SpriteTransformation transformation;
        protected MonsterRenderer renderer;

        protected int frameCounter;

        protected bool skip;

        public TransformSprite(SpriteTransformation transformation, MonsterRenderer renderer)
        {
            this.transformation = transformation;
            this.renderer = renderer;
            skip = false;
        }

        public void Begin()
        {
            if (transformation == SpriteTransformation.SlideDown && renderer.Position == SpritePosition.Hidden)
            {
                skip = true;
                return;
            }

            if (transformation == SpriteTransformation.ShiftForward)
                renderer.Position = SpritePosition.MovedForward;
            else if (transformation == SpriteTransformation.ShowAsBall)
                renderer.Display = SpriteDisplay.Ball;
            else if (transformation == SpriteTransformation.ShowAsNormal)
                renderer.Display = SpriteDisplay.Normal;
            else if (transformation == SpriteTransformation.ShowAsMinimized)
                renderer.Display = SpriteDisplay.Minimized;
            else if (transformation == SpriteTransformation.ShowAsSubstitute)
                renderer.Display = SpriteDisplay.Substitute;
            else if (transformation != SpriteTransformation.Hide)
                renderer.Position = SpritePosition.Shown;
            else
                renderer.Position = SpritePosition.Hidden;
        }

        public void Step(int frameCounter)
        {
            if (skip)
                return;
            this.frameCounter = frameCounter;
            if (transformation == SpriteTransformation.Blink)
            {
                renderer.Position = frameCounter % 12 >= 6 ? SpritePosition.Shown : SpritePosition.Hidden;
            }
        }

        public void UpdateSpritePosition(MonsterRenderer renderer, ref Rectangle drawFrom, ref Rectangle drawTo)
        {
            if (this.renderer != renderer || skip)
                return;

            switch (transformation)
            {
                case SpriteTransformation.SlideDown:
                    drawFrom = new Rectangle(drawFrom.X, drawFrom.Y, drawFrom.Width, drawFrom.Height - (int)(((decimal)frameCounter) * 16));
                    drawTo = new Rectangle(drawTo.X, drawTo.Y + ((int)frameCounter) * 16, drawTo.Width, drawFrom.Height);
                    break;
                case SpriteTransformation.SlideUp:
                    drawFrom = new Rectangle(drawFrom.X, drawFrom.Y, drawFrom.Width, (int)(((decimal)frameCounter) * 16));
                    drawTo = new Rectangle(drawTo.X, drawTo.Y + drawTo.Height - ((int)frameCounter) * 16, drawTo.Width, drawFrom.Height);
                    break;
                case SpriteTransformation.Shrink:
                    drawTo = new Rectangle(drawTo.X + frameCounter * drawTo.Width / 32, drawTo.Y + frameCounter * drawTo.Height/ 16, drawTo.Width - frameCounter * drawTo.Width/ 16, drawTo.Height - frameCounter * drawTo.Height / 16);
                    break;
                case SpriteTransformation.Stretch:
                    drawTo = new Rectangle(drawTo.X + drawTo.Width / 2 - frameCounter * drawTo.Width / 32, drawTo.Y + drawTo.Height - frameCounter * drawTo.Height/ 16, frameCounter * drawTo.Width/ 16, frameCounter * drawTo.Height / 16);
                    break;
                case SpriteTransformation.SlideOff:
                    drawTo.Offset(frameCounter * 16 * renderer.ScreenEdgeDirection, 0);
                    break;
                case SpriteTransformation.ShiftBackward:
                    drawTo.Offset(frameCounter * 12 * renderer.ScreenEdgeDirection, 0);
                    break;
                case SpriteTransformation.Flatten:
                    drawTo = new Rectangle(drawTo.X + frameCounter * 12, drawTo.Y, drawTo.Width - frameCounter * 24, drawFrom.Height);
                    break;
                case SpriteTransformation.SlideOffTop:
                    drawFrom = new Rectangle(drawFrom.X, drawFrom.Y + Math.Max(0, frameCounter * 16 - 112), drawFrom.Width, drawFrom.Height - Math.Max(0, frameCounter * 16 - 112));
                    drawTo = new Rectangle(drawTo.X, drawTo.Y - Math.Min(frameCounter * 16, 112), drawTo.Width, drawFrom.Height);
                    break;
                case SpriteTransformation.SlideBackDown:
                    drawFrom = new Rectangle(drawFrom.X, drawFrom.Y + drawFrom.Height - Math.Min(drawFrom.Height, frameCounter * 16), drawFrom.Width, Math.Min(drawFrom.Height, frameCounter * 16));
                    drawTo = new Rectangle(drawTo.X, Math.Min(drawTo.Y,  drawTo.Y - 112 + Math.Min(112, Math.Max(0, frameCounter * 16 - drawTo.Height))), drawTo.Width, drawFrom.Height);
                    break;
                case SpriteTransformation.ShakeInPlace:
                    if (frameCounter % 6 < 3)
                        drawTo.Offset(64 * renderer.ScreenEdgeDirection, 0);
                    break;
            }
        }

        public bool IsOver(int frameCounter)
        {
            if (skip)
                return true;

            switch (transformation)
            {
                case SpriteTransformation.SlideDown:
                case SpriteTransformation.SlideUp:
                    if (frameCounter <= renderer.MonSpriteRect.Height / 16)
                        return false;
                    break;
                case SpriteTransformation.Shrink:
                case SpriteTransformation.Stretch:
                    if (frameCounter <= 16)
                        return false;
                    break;
                case SpriteTransformation.Flatten:
                    if (frameCounter <= 12)
                        return false;
                    break;
                case SpriteTransformation.Blink:
                    if (frameCounter <= 71)
                        return false;
                    break;
                case SpriteTransformation.SlideOff:
                    if (frameCounter <= 24)
                        return false;
                    break;
                case SpriteTransformation.ShiftBackward:
                    if (frameCounter <= 12)
                        return false;
                    break;
                case SpriteTransformation.SlideOffTop:
                case SpriteTransformation.SlideBackDown:
                    if (frameCounter <= 15)
                        return false;
                    break;
                case SpriteTransformation.ShakeInPlace:
                    if (frameCounter < 60)
                        return false;
                    break;
            }

            return true;

        }

        public void End()
        {
            if (skip)
                return;

            switch (transformation)
            {
                case SpriteTransformation.SlideDown:
                case SpriteTransformation.Shrink:
                case SpriteTransformation.SlideOff:
                case SpriteTransformation.Flatten:
                case SpriteTransformation.SlideOffTop:
                    renderer.Position = SpritePosition.Hidden;
                    break;
                case SpriteTransformation.ShiftBackward:
                    renderer.Position = SpritePosition.MovedBackward;
                    break;
                case SpriteTransformation.Blink:
                    renderer.Position = SpritePosition.Shown;
                    break;
            }
        }
    }
}
