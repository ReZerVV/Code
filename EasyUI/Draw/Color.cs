namespace EasyUI.Draw
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

        public static Color White
        {
            get 
            {
                return new Color(255, 255, 255);
            }
        }

        public static Color Black
        {
            get
            {
                return new Color(0, 0, 0);
            }
        }

        public static Color Green
        {
            get
            {
                return new Color(24, 111, 101);
            }
        }

        public static Color Grey
        {
            get
            {
                return new Color(97, 103, 122);
            }
        }

        public static Color Blue
        {
            get
            {
                return new Color(100, 153, 23);
            }
        }

        public static Color Red
        {
            get
            {
                return new Color(255, 109, 96);
            }
        }

        public static Color Purple
        {
            get
            {
                return new Color(185, 49, 252);
            }
        }

        public static Color Pink
        {
            get
            {
                return new Color(255, 106, 194);
            }
        }

        public static Color Yellow
        {
            get
            {
                return new Color(247, 208, 96);
            }
        }
    }
}
