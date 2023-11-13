using EasyUI.Draw;

namespace Word.Core.Syntax
{
    public class TextPlainSyntaxHighlighter : IHighlighter
    {
        public string LangName => "Text Plain";
        public string LangExtension => @"txt$";
        public Color Default => ApplicationCode.Theme.Foreground;

        public List<TextFragmentInfo> Highlight(string text)
        {
            return new() { new TextFragmentInfo(text, Default, PixelStyle.StyleNone) };
        }
    }
}
