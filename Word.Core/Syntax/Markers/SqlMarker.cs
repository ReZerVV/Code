using System.Text;
using Word.Core.Syntax.Lexers;
using Word.Core.Syntax.Styles;

namespace Word.Core.Syntax.Markers
{
    public class SqlMarker : IMarker
    {
        public string LangName => "SQL Lang";
        public string LangExtension => ".sql";

        public Dictionary<string, Color> KeywordsColor;

        public Color KeywordColor => new Color(128, 182, 255);
        public Color FunctionColor => new Color(255, 255, 255);
        public Color IdentifierColor => new Color(255, 255, 255);
        public Color StringColor => new Color(136, 201, 153);
        public Color NumberColor => new Color(217, 153, 153);
        public Color CommentColor => new Color(153, 153, 153);
        public Color ColorDefault => new Color(255, 255, 255);

        public Style StyleDefault => Style.StyleNone;

        public SqlMarker()
        {
            KeywordsColor = new Dictionary<string, Color>();
            KeywordsColor.Add("ADD", KeywordColor);
            KeywordsColor.Add("CONSTRAINT", KeywordColor);
            KeywordsColor.Add("ALL", KeywordColor);
            KeywordsColor.Add("ALTER", KeywordColor);
            KeywordsColor.Add("COLUMN", KeywordColor);
            KeywordsColor.Add("TABLE", KeywordColor);
            KeywordsColor.Add("AND", KeywordColor);
            KeywordsColor.Add("ANY", KeywordColor);
            KeywordsColor.Add("AS", KeywordColor);
            KeywordsColor.Add("ASC", KeywordColor);
            KeywordsColor.Add("BACKUP", KeywordColor);
            KeywordsColor.Add("DATABASE", KeywordColor);
            KeywordsColor.Add("BETWEEN", KeywordColor);
            KeywordsColor.Add("CASE", KeywordColor);
            KeywordsColor.Add("CHECK", KeywordColor);
            KeywordsColor.Add("CREATE", KeywordColor);
            KeywordsColor.Add("INDEX", KeywordColor);
            KeywordsColor.Add("OR", KeywordColor);
            KeywordsColor.Add("REPLACE", KeywordColor);
            KeywordsColor.Add("VIEW", KeywordColor);
            KeywordsColor.Add("PROCEDURE", KeywordColor);
            KeywordsColor.Add("UNIQUE", KeywordColor);
            KeywordsColor.Add("DEFAULT", KeywordColor);
            KeywordsColor.Add("DELETE", KeywordColor);
            KeywordsColor.Add("DESC", KeywordColor);
            KeywordsColor.Add("DISTINCT", KeywordColor);
            KeywordsColor.Add("DROP", KeywordColor);
            KeywordsColor.Add("EXEC", KeywordColor);
            KeywordsColor.Add("EXISTS", KeywordColor);
            KeywordsColor.Add("FOREIGN", KeywordColor);
            KeywordsColor.Add("KEY", KeywordColor);
            KeywordsColor.Add("FROM", KeywordColor);
            KeywordsColor.Add("FULL", KeywordColor);
            KeywordsColor.Add("OUTER", KeywordColor);
            KeywordsColor.Add("JOIN", KeywordColor);
            KeywordsColor.Add("GROUP", KeywordColor);
            KeywordsColor.Add("BY", KeywordColor);
            KeywordsColor.Add("HAVING", KeywordColor);
            KeywordsColor.Add("IN", KeywordColor);
            KeywordsColor.Add("INNER", KeywordColor);
            KeywordsColor.Add("INSERT", KeywordColor);
            KeywordsColor.Add("INTO", KeywordColor);
            KeywordsColor.Add("IS", KeywordColor);
            KeywordsColor.Add("NULL", KeywordColor);
            KeywordsColor.Add("NOT", KeywordColor);
            KeywordsColor.Add("LEFT", KeywordColor);
            KeywordsColor.Add("LIKE", KeywordColor);
            KeywordsColor.Add("LIMIT", KeywordColor);
            KeywordsColor.Add("ORDER", KeywordColor);
            KeywordsColor.Add("PRIMARY", KeywordColor);
            KeywordsColor.Add("RIGHT", KeywordColor);
            KeywordsColor.Add("ROWNUM", KeywordColor);
            KeywordsColor.Add("SELECT", KeywordColor);
            KeywordsColor.Add("TOP", KeywordColor);
            KeywordsColor.Add("SET", KeywordColor);
            KeywordsColor.Add("TRUNCATE", KeywordColor);
            KeywordsColor.Add("UNION", KeywordColor);
            KeywordsColor.Add("UPDATE", KeywordColor);
            KeywordsColor.Add("VALUES", KeywordColor);
            KeywordsColor.Add("WHERE", KeywordColor);
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
                            if (KeywordsColor.TryGetValue(token.Value.ToUpper(), out Color color))
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
                                        Style.StyleItalic,
                                        token.Value));
                            }
                            else
                            {
                                richTextLines[lineIndex].Add(
                                    new RichText(
                                        IdentifierColor,
                                        Style.StyleItalic,
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
                    case TokenType.Minus:
                        {
                            if (lexer.PeekToken().Type == TokenType.Minus)
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
