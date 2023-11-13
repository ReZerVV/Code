using EasyUI.Draw;

namespace Word.Core.Syntax
{
    public class TextFragmentInfo
    {
        public Color Color { get; set; }
        public PixelStyle Style { get; set; }
        public string Text { get; set; }

        public TextFragmentInfo(string text, Color color, PixelStyle style)
        {
            Color = color;
            Style = style;
            Text = text;
        }
    }
}
