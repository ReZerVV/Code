using System.Text;
using Word.Core.Syntax.Lexers;
using Word.Core.Syntax.Styles;

namespace Word.Core.Syntax.Markers
{
    public class PythonMarker : IMarker
    {
        public string LangName => "Python Lang";
        public string LangExtension => ".py";

        public Dictionary<string, Color> KeywordsColor;

        public Color KeywordColor => new Color(249, 38, 114);
        public Color FunctionColor => new Color(92, 217, 239);
        public Color IdentifierColor => new Color(255, 255, 255);
        public Color StringColor => new Color(230, 219, 116);
        public Color NumberColor => new Color(174, 129, 219);
        public Color CommentColor => new Color(117, 113, 94);
        public Color ColorDefault => new Color(255, 255, 255);

        public Style StyleDefault => Style.StyleNone;

        public PythonMarker()
        {
            KeywordsColor = new Dictionary<string, Color>();
            KeywordsColor.Add("False", KeywordColor);
            KeywordsColor.Add("None", KeywordColor);
            KeywordsColor.Add("True", KeywordColor);
            KeywordsColor.Add("and", KeywordColor);
            KeywordsColor.Add("as", KeywordColor);
            KeywordsColor.Add("assert", KeywordColor);
            KeywordsColor.Add("async", KeywordColor);
            KeywordsColor.Add("await", KeywordColor);
            KeywordsColor.Add("break", KeywordColor);
            KeywordsColor.Add("class", KeywordColor);
            KeywordsColor.Add("continue", KeywordColor);
            KeywordsColor.Add("def", KeywordColor);
            KeywordsColor.Add("del", KeywordColor);
            KeywordsColor.Add("elif", KeywordColor);
            KeywordsColor.Add("else", KeywordColor);
            KeywordsColor.Add("except", KeywordColor);
            KeywordsColor.Add("finally", KeywordColor);
            KeywordsColor.Add("for", KeywordColor);
            KeywordsColor.Add("from", KeywordColor);
            KeywordsColor.Add("global", KeywordColor);
            KeywordsColor.Add("if", KeywordColor);
            KeywordsColor.Add("import", KeywordColor);
            KeywordsColor.Add("in", KeywordColor);
            KeywordsColor.Add("is", KeywordColor);
            KeywordsColor.Add("lambda", KeywordColor);
            KeywordsColor.Add("nonlocal", KeywordColor);
            KeywordsColor.Add("not", KeywordColor);
            KeywordsColor.Add("or", KeywordColor);
            KeywordsColor.Add("pass", KeywordColor);
            KeywordsColor.Add("raise", KeywordColor);
            KeywordsColor.Add("return", KeywordColor);
            KeywordsColor.Add("try", KeywordColor);
            KeywordsColor.Add("while", KeywordColor);
            KeywordsColor.Add("with", KeywordColor);
            KeywordsColor.Add("yield", KeywordColor);
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

                    // Parse comments
                    case TokenType.NumberSign:
                        {
                            StringBuilder commentBuilder = new StringBuilder();
                            commentBuilder.Append(token.Value);
                            while (lexer.TryGetNextToken(out token) &&
                                token.Type != TokenType.NewLine)
                            {
                                commentBuilder.Append(token.Value);
                            }
                            richTextLines[lineIndex].Add(
                                new RichText(
                                    CommentColor,
                                    StyleDefault,
                                    commentBuilder.ToString()));
                            richTextLines.Add(new List<RichText> { });
                            lineIndex++;
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
