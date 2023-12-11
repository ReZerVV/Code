using System.Text;
using Word.Core.Syntax.Lexers;
using Word.Core.Syntax.Styles;

namespace Word.Core.Syntax.Markers
{
    public class HtmlMarker : IMarker
    {
        public string LangName => "HTML Lang";
        public string LangExtension => ".html";

        public Dictionary<string, Color> KeywordsColor;

        public Color KeywordColor => new Color(240, 153, 153);
        public Color FunctionColor => new Color(255, 255, 255);
        public Color IdentifierColor => new Color(121, 201, 125);
        public Color StringColor => new Color(230, 219, 116);
        public Color NumberColor => new Color(255, 255, 255);
        public Color CommentColor => new Color(255, 255, 255);
        public Color ColorDefault => new Color(255, 255, 255);

        public Style StyleDefault => Style.StyleNone;

        public HtmlMarker()
        {
            KeywordsColor = new Dictionary<string, Color>();
            KeywordsColor.Add("html", KeywordColor);
            KeywordsColor.Add("head", KeywordColor);
            KeywordsColor.Add("title", KeywordColor);
            KeywordsColor.Add("meta", KeywordColor);
            KeywordsColor.Add("body", KeywordColor);
            KeywordsColor.Add("div", KeywordColor);
            KeywordsColor.Add("p", KeywordColor);
            KeywordsColor.Add("h1", KeywordColor);
            KeywordsColor.Add("h2", KeywordColor);
            KeywordsColor.Add("h3", KeywordColor);
            KeywordsColor.Add("h4", KeywordColor);
            KeywordsColor.Add("h5", KeywordColor);
            KeywordsColor.Add("h6", KeywordColor);
            KeywordsColor.Add("a", KeywordColor);
            KeywordsColor.Add("img", KeywordColor);
            KeywordsColor.Add("ul", KeywordColor);
            KeywordsColor.Add("ol", KeywordColor);
            KeywordsColor.Add("li", KeywordColor);
            KeywordsColor.Add("table", KeywordColor);
            KeywordsColor.Add("tr", KeywordColor);
            KeywordsColor.Add("td", KeywordColor);
        }

        public List<List<RichText>> Markup(List<string> lines)
        {
            List<List<RichText>> richTextLines = new List<List<RichText>>();
            ILexer lexer = new DefaultLexer(DocumentCompressor.СompressingLinesIntoLineAndFixingNewLine(lines));

            int lineIndex = 0;
            richTextLines.Add(new List<RichText> { });
            while (lexer.TryGetNextToken(out Token token))
            {
                switch (token.Type)
                {
                    case TokenType.NewLine:
                        {
                            lineIndex++;
                            richTextLines.Add(new List<RichText> { });
                        }
                        break;

                    case TokenType.EndOfFile:
                        {
                        }
                        break;

                    case TokenType.Tab:
                        {
                            richTextLines[lineIndex].Add(
                                new RichText(
                                    NumberColor,
                                    StyleDefault,
                                    "    "));
                        }
                        break;

                    // Parse string
                    case TokenType.DoubleQuotes:
                        {
                            StringBuilder stringLiteralBuilder = new StringBuilder();
                            do
                            {
                                if (token.Type == TokenType.Tab)
                                {
                                    stringLiteralBuilder.Append("    ");
                                    continue;
                                }
                                if (token.Type == TokenType.NewLine)
                                {
                                    richTextLines[lineIndex].Add(
                                        new RichText(
                                            StringColor,
                                            StyleDefault,
                                            stringLiteralBuilder.ToString()));
                                    stringLiteralBuilder.Clear();

                                    richTextLines.Add(new List<RichText> { });
                                    lineIndex++;
                                    continue;
                                }
                                stringLiteralBuilder.Append(token.Value);
                            } while (lexer.TryGetNextToken(out token) &&
                                token.Type != TokenType.DoubleQuotes);
                            if (token != null)
                            {
                                stringLiteralBuilder.Append(token.Value);
                            }
                            richTextLines[lineIndex].Add(
                                new RichText(
                                    StringColor,
                                    StyleDefault,
                                    stringLiteralBuilder.ToString()));
                        }
                        break;
                    case TokenType.SingleQuote:
                        {
                            StringBuilder stringLiteralBuilder = new StringBuilder();
                            do
                            {
                                if (token.Type == TokenType.Tab)
                                {
                                    stringLiteralBuilder.Append("    ");
                                    continue;
                                }
                                if (token.Type == TokenType.NewLine)
                                {
                                    richTextLines[lineIndex].Add(
                                        new RichText(
                                            StringColor,
                                            StyleDefault,
                                            stringLiteralBuilder.ToString()));
                                    stringLiteralBuilder.Clear();

                                    richTextLines.Add(new List<RichText> { });
                                    lineIndex++;
                                    continue;
                                }
                                stringLiteralBuilder.Append(token.Value);
                            } while (lexer.TryGetNextToken(out token) &&
                                token.Type != TokenType.SingleQuote);
                            if (token != null)
                            {
                                stringLiteralBuilder.Append(token.Value);
                            }
                            richTextLines[lineIndex].Add(
                                new RichText(
                                    StringColor,
                                    StyleDefault,
                                    stringLiteralBuilder.ToString()));
                        }
                        break;

                    case TokenType.StringLiteral:
                        {
                            if (KeywordsColor.TryGetValue(token.Value, out Color color))
                            {
                                richTextLines[lineIndex].Add(
                                    new RichText(
                                        color,
                                        StyleDefault,
                                        token.Value));
                            }
                            else if (lexer.PeekToken().Type == TokenType.OpenParen)
                            {
                                richTextLines[lineIndex].Add(
                                    new RichText(
                                        FunctionColor,
                                        StyleDefault,
                                        token.Value));
                            }
                            else
                            {
                                richTextLines[lineIndex].Add(
                                    new RichText(
                                        IdentifierColor,
                                        StyleDefault,
                                        token.Value));
                            }
                        }
                        break;

                    case TokenType.NumberLiteral:
                        {
                            richTextLines[lineIndex].Add(
                                new RichText(
                                    NumberColor,
                                    StyleDefault,
                                    token.Value));
                        }
                        break;

                    default:
                        {
                            richTextLines[lineIndex].Add(
                                new RichText(
                                    ColorDefault,
                                    StyleDefault,
                                    token.Value));
                        }
                        break;
                }
            }
            return richTextLines;
        }
    }
}
