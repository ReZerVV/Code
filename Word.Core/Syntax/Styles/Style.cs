namespace Word.Core.Syntax.Styles
{
    public class Style
    {
        public bool Bold = false;
        public bool Dim = false;
        public bool Italic = false;
        public bool Underline = false;
        public bool SlowBlink = false;
        public bool RapidBlink = false;
        public bool Overlined = false;

        public static Style StyleNone
            => new Style { };

        public static Style StyleBold
            => new Style { Bold = true };

        public static Style StyleDim
            => new Style { Dim = true };

        public static Style StyleItalic
            => new Style { Italic = true };

        public static Style StyleUnderline
            => new Style { Underline = true };

        public static Style StyleSlowBlink
            => new Style { SlowBlink = true };

        public static Style StyleRapidBlink
            => new Style { RapidBlink = true };

        public static Style StyleOverlined
            => new Style { Overlined = true };

        public override bool Equals(object? obj)
        {
            return obj is Style style &&
                   Bold == style.Bold &&
                   Dim == style.Dim &&
                   Italic == style.Italic &&
                   Underline == style.Underline &&
                   SlowBlink == style.SlowBlink &&
                   RapidBlink == style.RapidBlink;
        }
    }
}
