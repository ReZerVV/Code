using EasyUI.Draw;

namespace Word.Themes
{
    public class DefaultLightTheme : ITheme
    {
        public string Name => "Default Light Theme";

        public Color Foreground => new Color(14, 17, 22);

        public Color Background => new Color(255, 255, 255);

        public Color BackgroundDark => new Color(217, 217, 217);

        public Color StatusBarForeground => new Color(45, 51, 52);

        public Color StatusBarBackground => new Color(255, 255, 255);

        public Color CommandLineBarSelectColor => new Color(255, 255, 255);

        public Color NumberingStripForeground => new Color(45, 51, 52);

        public Color NumberingStripBackground => new Color(255, 255, 255);
    }
}
