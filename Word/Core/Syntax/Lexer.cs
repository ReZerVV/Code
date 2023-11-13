using System.Text;
using System.Text.RegularExpressions;

namespace Word.Core.Syntax
{
    public class Lexer
    {
        private string text;
        private Regex regularExpressionKeywords;
        private Regex regularExpressionComments;
        private Regex regularExpressionStartComments;
        private Regex regularExpressionEndComments;

        public Lexer(
            string text,
            Regex regularExpressionKeyWords, 
            Regex regularExpressionComments,
            Regex regularExpressionStartComments,
            Regex regularExpressionEndComments)
        {
            this.text = text;
            this.regularExpressionKeywords = regularExpressionKeyWords;
            this.regularExpressionComments = regularExpressionComments;
            this.regularExpressionStartComments = regularExpressionStartComments;
            this.regularExpressionEndComments = regularExpressionEndComments;
        }

        public static List<Token> Tokenize(
            string text,
            Regex regularExpressionKeyWords,
            Regex regularExpressionComments,
            Regex regularExpressionStartComments,
            Regex regularExpressionEndComments)
        {
            Lexer lexer = new Lexer(
                text,
                regularExpressionKeyWords,
                regularExpressionComments,
                regularExpressionStartComments,
                regularExpressionEndComments);
            return lexer.Tokenize();
        }

        public List<Token> Tokenize()
        {
            List<Token> tokens = new List<Token>();
            int tokenStartPosition = 0;
            StringBuilder tokenContent = new StringBuilder();

            for (int symbolIndex = 0; symbolIndex < text.Length; symbolIndex++)
            {
                // Parse whitespase
                if (char.IsWhiteSpace(text[symbolIndex]))
                {
                    do
                    {
                        tokenContent.Append(text[symbolIndex]);
                        symbolIndex++;
                    }
                    while (symbolIndex < text.Length && 
                           char.IsWhiteSpace(text[symbolIndex]));
                    symbolIndex--;

                    tokens.Add(new Token(tokenContent.ToString(), Token.TokenKind.Whitespace, tokenStartPosition));
                }

                // Parse keyword or identifier
                else if (char.IsLetter(text[symbolIndex]))
                {
                    do
                    {
                        tokenContent.Append(text[symbolIndex]);
                        symbolIndex++;
                    }
                    while (symbolIndex < text.Length &&
                           !char.IsWhiteSpace(text[symbolIndex]) &&
                           (char.IsLetterOrDigit(text[symbolIndex]) ||
                            char.Equals(text[symbolIndex], '-')));
                    symbolIndex--;

                    if (regularExpressionKeywords.IsMatch(tokenContent.ToString()))
                    {
                        tokens.Add(new Token(tokenContent.ToString(), Token.TokenKind.Keyword, tokenStartPosition));
                    }
                    else
                    {
                        tokens.Add(new Token(tokenContent.ToString(), Token.TokenKind.Identifier, tokenStartPosition));
                    }
                }

                // Parsing a numeric literal
                else if (char.IsDigit(text[symbolIndex]))
                {
                    do
                    {
                        tokenContent.Append(text[symbolIndex]);
                        symbolIndex++;
                    }
                    while (symbolIndex < text.Length && 
                           char.IsDigit(text[symbolIndex]));
                    symbolIndex--;

                    tokens.Add(new Token(tokenContent.ToString(), Token.TokenKind.Number, tokenStartPosition));
                }

                // Parsing a string literal
                else if (char.Equals(text[symbolIndex], '"'))
                {
                    do
                    {
                        tokenContent.Append(text[symbolIndex]);
                        symbolIndex++;
                    } while (symbolIndex < text.Length &&
                             !char.Equals(text[symbolIndex], '"'));
                    if (symbolIndex < text.Length && 
                        char.Equals(text[symbolIndex], '"'))
                    {
                        tokenContent.Append(text[symbolIndex]);
                    }
                    else
                    {
                        symbolIndex--;
                    }

                    tokens.Add(new Token(tokenContent.ToString(), Token.TokenKind.String, tokenStartPosition));
                }

                // Parsing a string literal
                else if (char.Equals(text[symbolIndex], '\''))
                {
                    do
                    {
                        tokenContent.Append(text[symbolIndex]);
                        symbolIndex++;
                    } while (symbolIndex < text.Length &&
                             !char.Equals(text[symbolIndex], '\''));
                    if (symbolIndex < text.Length &&
                        char.Equals(text[symbolIndex], '\''))
                    {
                        tokenContent.Append(text[symbolIndex]);
                    }
                    else
                    {
                        symbolIndex--;
                    }

                    tokens.Add(new Token(tokenContent.ToString(), Token.TokenKind.String, tokenStartPosition));
                }

                // Parse operators
                else if (Regex.IsMatch(text[symbolIndex].ToString(), @"[%\^&*\-+=!~|\\]"))
                {
                    do
                    {
                        tokenContent.Append(text[symbolIndex]);
                        symbolIndex++;
                    } while (symbolIndex < text.Length &&
                             Regex.IsMatch(text[symbolIndex].ToString(), @"[%\^&*\-+=!~|\\]"));
                    symbolIndex--;
                    tokens.Add(new Token(tokenContent.ToString(), Token.TokenKind.Operator, tokenStartPosition));
                }

                // Parse delimiters
                else if (Regex.IsMatch(text[symbolIndex].ToString(), @"[.,;:]"))
                {
                    do
                    {
                        tokenContent.Append(text[symbolIndex]);
                        symbolIndex++;
                    } while (symbolIndex < text.Length &&
                             Regex.IsMatch(text[symbolIndex].ToString(), @"[.,;:]"));
                    symbolIndex--;
                    tokens.Add(new Token(tokenContent.ToString(), Token.TokenKind.Delimiter, tokenStartPosition));
                }

                // Parse special characters
                else if (Regex.IsMatch(text[symbolIndex].ToString(), @"[^a-zA-Z0-9\s.,;:'""%^&*\-+=!~|\\]"))
                {
                    do
                    {
                        tokenContent.Append(text[symbolIndex]);
                        symbolIndex++;
                    } while (symbolIndex < text.Length &&
                             Regex.IsMatch(text[symbolIndex].ToString(), @"[^A-Za-z0-9]"));
                    symbolIndex--;

                    if (regularExpressionComments.IsMatch(text[symbolIndex].ToString()))
                    {
                        tokenContent.Append(text.Substring(symbolIndex));
                        symbolIndex = text.Length - 1;
                        tokens.Add(new Token(tokenContent.ToString(), Token.TokenKind.Comment, tokenStartPosition));
                    }
                    else if (regularExpressionStartComments.IsMatch(text[symbolIndex].ToString()))
                    {
                        do
                        {
                            tokenContent.Append(text[symbolIndex]);
                            symbolIndex++;
                        }
                        while (symbolIndex < text.Length &&
                             regularExpressionEndComments.IsMatch(text[symbolIndex].ToString()));
                        symbolIndex--;
                        tokens.Add(new Token(tokenContent.ToString(), Token.TokenKind.Comment, tokenStartPosition));
                    }
                    else
                    {
                        tokens.Add(new Token(tokenContent.ToString(), Token.TokenKind.SpecialCharacter, tokenStartPosition));
                    }
                }

                // Parse others
                else 
                { 
                    tokens.Add(new Token(tokenContent.ToString(), Token.TokenKind.Other, tokenStartPosition));
                }
                
                tokenContent.Clear();
                tokenStartPosition = symbolIndex;
            }

            return tokens;
        }
    }
}
