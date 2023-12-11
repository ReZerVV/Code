namespace Word.Core.Syntax.Styles
{
    public class Color
    {
        public byte R;
        public byte G;
        public byte B;

        public Color(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }

        public override bool Equals(object? obj)
        {
            return obj is Color color &&
                   R == color.R &&
                   G == color.G &&
                   B == color.B;
        }
    }
}
