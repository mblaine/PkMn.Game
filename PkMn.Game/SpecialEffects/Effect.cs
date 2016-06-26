using System;
using System.Linq;
using System.Xml;
using PkMn.Game.Enums;
using PkMn.Model;
using PkMn.Model.Enums;

namespace PkMn.Game.SpecialEffects
{
    public class Effect
    {
        public readonly SpecialEffectType Type;

        public readonly string SequenceName;
        public readonly ScreenPalette Palette;
        public readonly SpriteTransformation SpriteTransformation;
        public readonly ScreenTransformation ScreenTransformation;
        public readonly bool Parallel;
        public readonly Type SpriteAnimation;
        public readonly Who Who;
        public readonly int DelayFrames;
        public readonly int Offset;
        public readonly int Param;
        public readonly int StartAt;
        public readonly int StopEarly;

        public Sequence Sequence
        {
            get
            {
                if (Sequence.Sequences.ContainsKey(SequenceName))
                    return Sequence.Sequences[SequenceName];
                return null;
            }
        }

        public Effect(XmlNode node)
        {
            Type = (SpecialEffectType)Enum.Parse(typeof(SpecialEffectType), node.Attributes["type"].Value.Replace("-", ""), true);
            StartAt = node.Attributes.Contains("start-at") ? int.Parse(node.Attributes["start-at"].Value) : 0;
            StopEarly = node.Attributes.Contains("stop-early") ? int.Parse(node.Attributes["stop-early"].Value) : int.MaxValue;
            switch (Type)
            {
                case SpecialEffectType.PaletteSwap:
                    Palette = (ScreenPalette)Enum.Parse(typeof(ScreenPalette), node.Attributes["palette"].Value.Replace("-", ""), true);
                    break;
                case SpecialEffectType.Sequence:
                    SequenceName = node.Attributes["sequence"].Value;
                    break;
                case SpecialEffectType.Delay:
                    DelayFrames = int.Parse(node.Attributes["delay"].Value);
                    break;
                case SpecialEffectType.SpriteTransform:
                    SpriteTransformation = (SpriteTransformation)Enum.Parse(typeof(SpriteTransformation), node.Attributes["transform"].Value.Replace("-", ""), true);
                    Who = (Who)Enum.Parse(typeof(Who), node.Attributes["who"].Value, true);
                    break;
                case SpecialEffectType.ScreenTransform:
                    ScreenTransformation = (ScreenTransformation)Enum.Parse(typeof(ScreenTransformation), node.Attributes["transform"].Value.Replace("-", ""), true);
                    Offset = int.Parse(node.Attributes["offset"].Value);
                    Parallel = node.Attributes.Contains("parallel") ? bool.Parse(node.Attributes["parallel"].Value) : false;
                    break;
                case SpecialEffectType.Animation:
                    string animationName = string.Join("", node.Attributes["animation"].Value.Split('-').Select(s => s.Substring(0, 1).ToUpper() + s.Substring(1).ToLower()));
                    SpriteAnimation = System.Type.GetType("PkMn.Game.Animations." + animationName);
                    if (SpriteAnimation == null)
                        throw new Exception("Unable to get type: " + animationName);
                    Who = (Who)Enum.Parse(typeof(Who), node.Attributes["who"].Value, true);
                    DelayFrames = int.Parse(node.Attributes["delay"].Value);
                    Param = node.Attributes.Contains("param") ? int.Parse(node.Attributes["param"].Value) : 0;
                    break;
                case SpecialEffectType.Temporary:
                    break;
                default:
                    throw new Exception("Unhandled SpecialEffectType: " + Type.ToString());
            }
        }

        public override string ToString()
        {
            switch (Type)
            {
                case SpecialEffectType.PaletteSwap:
                    return string.Format("{0}: {1}", Type, Palette);
                case SpecialEffectType.Sequence:
                    return string.Format("{0}: {1}", Type, SequenceName);
                case SpecialEffectType.Delay:
                    return string.Format("{0}: {1}", Type, DelayFrames);
                case SpecialEffectType.SpriteTransform:
                    return string.Format("{0}: {1}", Type, SpriteTransformation);
                case SpecialEffectType.ScreenTransform:
                    return string.Format("{0}: {1}", Type, ScreenTransformation);
                default:
                    throw new Exception("Unhandled SpecialEffectType: " + Type.ToString());
            }
        }
    }
}
