using EasyUI.Draw;

namespace Word.Themes
{
    public interface ITheme
    {
        string Name { get; }
        Color Foreground { get; }
        Color Background { get; }
        Color BackgroundDark { get; }
        Color StatusBarForeground { get; }
        Color StatusBarBackground { get; }
        Color CommandLineBarSelectColor { get; }
        Color NumberingStripForeground { get; }
        Color NumberingStripBackground { get; }
    }
}
