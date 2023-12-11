using System.Text;
using Word.Core.Syntax.Lexers;
using Word.Core.Syntax.Styles;

namespace Word.Core.Syntax.Markers
{
    public class JavaScriptMarker : IMarker
    {
        public string LangName => "Java Script Lang";
        public string LangExtension => ".js";

        public Dictionary<string, Color> KeywordsColor;

        public Color KeywordColor => new Color(197, 165, 197);
        public Color FunctionColor => new Color(197, 165, 197);
        public Color IdentifierColor => new Color(255, 255, 255);
        public Color StringColor => new Color(121, 201, 153);
        public Color NumberColor => new Color(100, 182, 255);
        public Color CommentColor => new Color(153, 153, 153);
        public Color ColorDefault => new Color(255, 255, 255);
        
        public Style StyleDefault => Style.StyleNone;

        public JavaScriptMarker()
        {
            // Init keywords colors
            KeywordsColor = new Dictionary<string, Color>();
            KeywordsColor.Add("abstract", KeywordColor);
            KeywordsColor.Add("arguments", KeywordColor);
            KeywordsColor.Add("await", KeywordColor);
            KeywordsColor.Add("boolean", KeywordColor);
            KeywordsColor.Add("break", KeywordColor);
            KeywordsColor.Add("byte", KeywordColor);
            KeywordsColor.Add("case", KeywordColor);
            KeywordsColor.Add("catch", KeywordColor);
            KeywordsColor.Add("char", KeywordColor);
            KeywordsColor.Add("class", KeywordColor);
            KeywordsColor.Add("const", KeywordColor);
            KeywordsColor.Add("continue", KeywordColor);
            KeywordsColor.Add("debugger", KeywordColor);
            KeywordsColor.Add("default", KeywordColor);
            KeywordsColor.Add("delete", KeywordColor);
            KeywordsColor.Add("do", KeywordColor);
            KeywordsColor.Add("double", KeywordColor);
            KeywordsColor.Add("else", KeywordColor);
            KeywordsColor.Add("enum", KeywordColor);
            KeywordsColor.Add("eval", KeywordColor);
            KeywordsColor.Add("export", KeywordColor);
            KeywordsColor.Add("extends", KeywordColor);
            KeywordsColor.Add("false", KeywordColor);
            KeywordsColor.Add("final", KeywordColor);
            KeywordsColor.Add("finally", KeywordColor);
            KeywordsColor.Add("float", KeywordColor);
            KeywordsColor.Add("for", KeywordColor);
            KeywordsColor.Add("goto", KeywordColor);
            KeywordsColor.Add("if", KeywordColor);
            KeywordsColor.Add("implements", KeywordColor);
            KeywordsColor.Add("import", KeywordColor);
            KeywordsColor.Add("in", KeywordColor);
            KeywordsColor.Add("instanceof", KeywordColor);
            KeywordsColor.Add("int", KeywordColor);
            KeywordsColor.Add("interface", KeywordColor);
            KeywordsColor.Add("let", KeywordColor);
            KeywordsColor.Add("long", KeywordColor);
            KeywordsColor.Add("native", KeywordColor);
            KeywordsColor.Add("new", KeywordColor);
            KeywordsColor.Add("null", KeywordColor);
            KeywordsColor.Add("package", KeywordColor);
            KeywordsColor.Add("private", KeywordColor);
            KeywordsColor.Add("protected", KeywordColor);
            KeywordsColor.Add("public", KeywordColor);
            KeywordsColor.Add("return", KeywordColor);
            KeywordsColor.Add("short", KeywordColor);
            KeywordsColor.Add("static", KeywordColor);
            KeywordsColor.Add("super", KeywordColor);
            KeywordsColor.Add("switch", KeywordColor);
            KeywordsColor.Add("synchronized", KeywordColor);
            KeywordsColor.Add("this", KeywordColor);
            KeywordsColor.Add("throw", KeywordColor);
            KeywordsColor.Add("throws", KeywordColor);
            KeywordsColor.Add("transient", KeywordColor);
            KeywordsColor.Add("true", KeywordColor);
            KeywordsColor.Add("try", KeywordColor);
            KeywordsColor.Add("var", KeywordColor);
            KeywordsColor.Add("void", KeywordColor);
            KeywordsColor.Add("volatile", KeywordColor);
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
