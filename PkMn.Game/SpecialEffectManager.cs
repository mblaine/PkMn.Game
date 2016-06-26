using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PkMn.Game.Controls;
using PkMn.Game.Enums;
using PkMn.Game.SpecialEffects;
using PkMn.Model.Enums;

namespace PkMn.Game
{
    public class SpecialEffectManager : Panel
    {
        private Queue<ISpecialEffect> queue;
        private ISpecialEffect currentEffect;
        private TransformScreen parallelEffect;
        private int frameCounter;
        private int parallelFrameCounter;

        public EventHandler OnEffectsEnd;

        public SpecialEffectManager(PkMnGame parent)
            : base(parent, 0, 0, 0, 0)
        {
            queue = new Queue<ISpecialEffect>();
        }

        public void QueueSpecialEffect(ISpecialEffect effect)
        {
            queue.Enqueue(effect);
        }

        public void QueueSpecialEffects(Sequence sequence, MonsterRenderer self, MonsterRenderer foe)
        {
            foreach (SpecialEffects.Effect effect in sequence.Effects)
            {
                switch (effect.Type)
                {
                    case SpecialEffectType.Sequence:
                        QueueSpecialEffects(effect.Sequence, self, foe);
                        break;
                    case SpecialEffectType.ScreenTransform:
                        queue.Enqueue(new TransformScreen(parent, effect.ScreenTransformation, effect.Offset, effect.Parallel));
                        break;
                    case SpecialEffectType.SpriteTransform:
                        queue.Enqueue(new TransformSprite(effect.SpriteTransformation, effect.Who == Who.Self ? self : foe));
                        break;
                    case SpecialEffectType.PaletteSwap:
                        queue.Enqueue(new SwapPalette(effect.Palette));
                        break;
                    case SpecialEffectType.Delay:
                        queue.Enqueue(new Delay(effect.DelayFrames));
                        break;
                    case SpecialEffectType.Animation:
                        ConstructorInfo constructor = effect.SpriteAnimation.GetConstructor(new Type[] { typeof(MonsterRenderer), typeof(int), typeof(int), typeof(int), typeof(int) });
                        queue.Enqueue((Animation)constructor.Invoke(new object[] { effect.Who == Who.Self ? self : foe, effect.DelayFrames, effect.Param, effect.StartAt, effect.StopEarly }));
                        break;
                    case SpecialEffectType.Temporary:
                        break;
                }
            }
        }

        public void TransformMonster(MonsterRenderer renderer, ref Rectangle drawFrom, ref Rectangle drawTo)
        {
            if (currentEffect != null && currentEffect is TransformSprite)
            {
                ((TransformSprite)currentEffect).UpdateSpritePosition(renderer, ref drawFrom, ref drawTo);
            }
        }

        public bool EffectNeedsMonDefaultPalette(MonsterRenderer renderer)
        {
            if (currentEffect != null && currentEffect is Animation)
            {
                return ((Animation)currentEffect).UseMonDefaultPalette(renderer);
            }

            return false;
        }

        public bool EffectNeedsToBeDrawnBeforeMon()
        {
            if (currentEffect != null && currentEffect is Animation)
            {
                return ((Animation)currentEffect).DrawBeforeMon(frameCounter);
            }

            return false;
        }

        public override void Update(GameTime gameTime)
        {
            frameCounter++;
            if(parallelEffect != null)
                parallelFrameCounter++;

            if (currentEffect != null)
            {
                currentEffect.Step(frameCounter);
                if (currentEffect.IsOver(frameCounter) || (currentEffect is Animation && frameCounter >= ((Animation)currentEffect).StopEarly))
                {
                    currentEffect.End();
                    currentEffect = null;
                    if (queue.Count <= 0 && OnEffectsEnd != null)
                        OnEffectsEnd(this, null);
                }
            }

            //So, most of the special effects happen one after the other but for a couple of moves 
            //there's more than one going on at the same time.
            //Most of this is handled by the start-at and stop-early properties on the Animation class
            //that makes it so other effects can be inserted in the middle of an ongoing animation
            //by splitting the animation up.
            //But this setup couldn't handle the mid-animation screen shaking in Rock Slide.
            //So, this code is pretty much just for that.
            if (parallelEffect != null)
            {
                parallelEffect.Step(parallelFrameCounter);
                if (parallelEffect.IsOver(parallelFrameCounter))
                {
                    parallelEffect.End();
                    parallelEffect = null;
                }
            }

            if (currentEffect == null && queue.Count > 0)
            {
                currentEffect = queue.Dequeue();

                currentEffect.Begin();
                frameCounter = currentEffect is Animation ? ((Animation)currentEffect).StartAt : 0;

                if (currentEffect is TransformScreen && ((TransformScreen)currentEffect).Parallel)
                {
                    parallelEffect = (TransformScreen)currentEffect;
                    parallelFrameCounter = frameCounter;
                    currentEffect = null;
                    if(queue.Count > 0)
                    {
                        currentEffect = queue.Dequeue();
                        currentEffect.Begin();
                        frameCounter = currentEffect is Animation ? ((Animation)currentEffect).StartAt : 0;
                    }
                }
            }
        }

        public override void Draw(GameTime gameTime, GraphicsDevice device, SpriteBatch spriteBatch)
        {
            if (currentEffect != null && currentEffect is Animation)
            {
                ((Animation)currentEffect).Draw(gameTime, device, spriteBatch, frameCounter);
            }
        }
    }
}
