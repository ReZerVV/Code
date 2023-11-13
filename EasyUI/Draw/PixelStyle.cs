namespace EasyUI.Draw
{
    public class PixelStyle
    {
        public bool Bold = false;
        public bool Dim = false;
        public bool Italic = false;
        public bool Underline = false;
        public bool SlowBlink = false;
        public bool RapidBlink = false;
        public bool Overlined = false;

        public static PixelStyle StyleNone 
            => new PixelStyle { };

        public static PixelStyle StyleBold
            => new PixelStyle { Bold = true };

        public static PixelStyle StyleDim
            => new PixelStyle { Dim = true };

        public static PixelStyle StyleItalic
            => new PixelStyle { Italic = true };

        public static PixelStyle StyleUnderline
            => new PixelStyle { Underline = true };

        public static PixelStyle StyleSlowBlink
            => new PixelStyle { SlowBlink = true };

        public static PixelStyle StyleRapidBlink
            => new PixelStyle { RapidBlink = true };

        public static PixelStyle StyleOverlined
            => new PixelStyle { Overlined = true };

        public override bool Equals(object? obj)
        {
            return obj is PixelStyle style &&
                   Bold == style.Bold &&
                   Dim == style.Dim &&
                   Italic == style.Italic &&
                   Underline == style.Underline &&
                   SlowBlink == style.SlowBlink &&
                   RapidBlink == style.RapidBlink;
        }

        public string? ToEscapeCodeString()
        {
            string stringStyle = "0";
            if (Bold)
            {
                stringStyle += ";1";
            }
            if (Dim)
            {
                stringStyle += ";2";
            }
            if (Italic)
            {
                stringStyle += ";3";
            }
            if (Underline)
            {
                stringStyle += ";4";
            }
            if (SlowBlink)
            {
                stringStyle += ";5";
            }
            if (RapidBlink)
            {
                stringStyle += ";6";
            }
            if (Overlined)
            {
                stringStyle += ";53";
            }
            return stringStyle;
        }
    }
}
