using EasyUI.Draw;

namespace Word.Themes
{
    public class DefaultDarkTheme : ITheme
    {
        public string Name => "Default Dark Theme";

        public Color Foreground => new Color(201, 209, 217);

        public Color Background => new Color(13, 17, 23);

        public Color BackgroundDark => new Color(1, 4, 9);

        public Color StatusBarForeground => new Color(201, 209, 217);

        public Color StatusBarBackground => new Color(1, 4, 9);

        public Color CommandLineBarSelectColor => new Color(13, 17, 23);

        public Color NumberingStripForeground => new Color(201, 209, 217);

        public Color NumberingStripBackground => new Color(13, 17, 23);
    }
}
