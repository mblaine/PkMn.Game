using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using PkMn.Game.Enums;

namespace PkMn.Game.SpecialEffects
{
    public class Sequence
    {
        protected static string XmlPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Generation-I", "special-effects.xml");

        public static readonly Dictionary<string, Sequence> Sequences = Load();

        public readonly string Name;
        public readonly SequenceType Type;
        public readonly List<Effect> Effects;

        protected static Dictionary<string, Sequence> Load()
        {
            Dictionary<string, Sequence> t = new Dictionary<string, Sequence>();

            XmlDocument doc = new XmlDocument();
            doc.Load(XmlPath);

            foreach (XmlNode node in doc.GetElementsByTagName("sequence"))
            {
                Sequence sequence = new Sequence(node);
                t[sequence.Name] = sequence;
            }

            return t;
        }

        protected Sequence(XmlNode node)
        {
            Name = node.Attributes["name"].Value;
            Type = (SequenceType)Enum.Parse(typeof(SequenceType), node.Attributes["type"].Value.Replace("-", ""), true);

            Effects = new List<Effect>();

            foreach (XmlNode effect in node.Cast<XmlNode>().Where(n => n.Name == "effect"))
            {
                Effects.Add(new Effect(effect));
            }
        }
    }
}
