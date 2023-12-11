using System.Text;
using Word.Core.Syntax.Styles;

namespace Word.Core.Syntax.Markers
{
    public class PlainTextMarker : IMarker
    {
        public string LangName => "Plain Text";
        public string LangExtension => ".txt";

        public Color ColorDefault => new Color(255, 255, 255);
        public Style StyleDefault => Style.StyleNone;

        public List<List<RichText>> Markup(List<string> lines)
        {
            List<List<RichText>> richTextLines = new List<List<RichText>>();
            
            foreach (string line in lines)
            {
                richTextLines.Add(new List<RichText>
                {
                    new RichText(
                        ColorDefault,
                        StyleDefault,
                        line)
                });
            }

            return richTextLines;
        }
    }
}
