namespace PkMn.Game
{
    public static class SpecialCharacters
    {
        public const string Pk = "#";
        public const string Mn = "%";
        public const string PkMn = "#%";
        public const string ColonL = "\"";
        public const string MultiplicationX = "&";
        public const string MaleSymbol = "*";
        public const string FemaleSymbol = "+";
        public const string RightArrow = ">";
        public const string HollowRightArrow = "<";
        public const string EAcute = "@";
        public const string DownArrow = "^";

        public const string ApostropheD = "\\";
        public const string ApostropheL = "_";
        public const string ApostropheM = "`";
        public const string ApostropheR = "{";
        public const string ApostropheS = "|";
        public const string ApostropheT = "}";
        public const string ApostropheV = "~";

        public static string ReplaceChars(string text)
        {
            text = text.Replace("é", EAcute);
            text = text.Replace("♀", FemaleSymbol);
            text = text.Replace("♂", MaleSymbol);
            text = text.Replace("×", MultiplicationX);
            text = text.Replace("'d", ApostropheD);
            text = text.Replace("'l", ApostropheL);
            text = text.Replace("'m", ApostropheM);
            text = text.Replace("'r", ApostropheR);
            text = text.Replace("'s", ApostropheS);
            text = text.Replace("'t", ApostropheT);
            text = text.Replace("'v", ApostropheV);
            text = text.Replace("PkMn", PkMn);

            return text;
        }
        
    }
}
