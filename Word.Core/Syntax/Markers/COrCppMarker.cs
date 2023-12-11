using System.Text;
using Word.Core.Syntax.Lexers;
using Word.Core.Syntax.Styles;

namespace Word.Core.Syntax.Markers
{
    public class COrCppMarker : IMarker
    {
        public string LangName => "C/C++ Lang";
        public string LangExtension => ".c";


        public Dictionary<string, Color> KeywordsColor;

        public Color KeywordColor => new Color(249, 38, 114);
        public Color FunctionColor => new Color(255, 255, 255);
        public Color IdentifierColor => new Color(255, 255, 255);
        public Color StringColor => new Color(230, 219, 90);
        public Color NumberColor => new Color(255, 255, 255);
        public Color CommentColor => new Color(117, 113, 94);
        public Color ColorDefault => new Color(255, 255, 255);

        public Style StyleDefault => Style.StyleNone;

        public COrCppMarker()
        {
            // Init keywords colors
            KeywordsColor = new Dictionary<string, Color>();
            KeywordsColor.Add("asm", KeywordColor);
            KeywordsColor.Add("double", new Color(102, 217, 239));
            KeywordsColor.Add("new", KeywordColor);
            KeywordsColor.Add("switch", KeywordColor);
            KeywordsColor.Add("auto", KeywordColor);
            KeywordsColor.Add("else", KeywordColor);
            KeywordsColor.Add("namespace", KeywordColor);
            KeywordsColor.Add("operator", KeywordColor);
            KeywordsColor.Add("template", KeywordColor);
            KeywordsColor.Add("break", KeywordColor);
            KeywordsColor.Add("enum", KeywordColor);
            KeywordsColor.Add("private", KeywordColor);
            KeywordsColor.Add("this", KeywordColor);
            KeywordsColor.Add("case", KeywordColor);
            KeywordsColor.Add("extern", KeywordColor);
            KeywordsColor.Add("protected", KeywordColor);
            KeywordsColor.Add("throw", KeywordColor);
            KeywordsColor.Add("catch", KeywordColor);
            KeywordsColor.Add("float", new Color(102, 217, 239));
            KeywordsColor.Add("public", KeywordColor);
            KeywordsColor.Add("try", KeywordColor);
            KeywordsColor.Add("char", KeywordColor);
            KeywordsColor.Add("for", KeywordColor);
            KeywordsColor.Add("register", KeywordColor);
            KeywordsColor.Add("typedef", KeywordColor);
            KeywordsColor.Add("class", KeywordColor);
            KeywordsColor.Add("friend", KeywordColor);
            KeywordsColor.Add("return", KeywordColor);
            KeywordsColor.Add("union", KeywordColor);
            KeywordsColor.Add("const", KeywordColor);
            KeywordsColor.Add("goto", KeywordColor);
            KeywordsColor.Add("short", new Color(102, 217, 239));
            KeywordsColor.Add("unsigned", new Color(102, 217, 239));
            KeywordsColor.Add("using", KeywordColor);
            KeywordsColor.Add("continue", KeywordColor);
            KeywordsColor.Add("if", KeywordColor);
            KeywordsColor.Add("signed", new Color(102, 217, 239));
            KeywordsColor.Add("virtual", KeywordColor);
            KeywordsColor.Add("default", KeywordColor);
            KeywordsColor.Add("inline", KeywordColor);
            KeywordsColor.Add("sizeof", KeywordColor);
            KeywordsColor.Add("void", new Color(102, 217, 239));
            KeywordsColor.Add("delete", KeywordColor);
            KeywordsColor.Add("int", new Color(102, 217, 239));
            KeywordsColor.Add("static", KeywordColor);
            KeywordsColor.Add("volatile", KeywordColor);
            KeywordsColor.Add("do", KeywordColor);
            KeywordsColor.Add("long", new Color(102, 217, 239));
            KeywordsColor.Add("struct", new Color(102, 217, 239));
            KeywordsColor.Add("while", KeywordColor);
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
                    case TokenType.Slash:
                        {
                            if (lexer.PeekToken().Type == TokenType.Slash)
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
                            else
                            {
                                richTextLines[lineIndex].Add(
                                    new RichText(
                                        ColorDefault,
                                        StyleDefault,
                                        token.Value));
                            }
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
