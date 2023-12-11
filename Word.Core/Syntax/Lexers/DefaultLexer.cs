using System.Text;

namespace Word.Core.Syntax.Lexers
{
    public class DefaultLexer : ILexer
    {
        public const char TabSymbol = '\t';
        public const char NewLineSymbol = '\n';
        public const char EndOfFileSymbol = '\0';

        public string Text { get; private set; }
        public int CurrentIndex { get; private set; }
        public char CurrentSymbol { get; private set; }
        public bool IsFinish { get; set; }

        public DefaultLexer(string text)
        {
            SetText(text);
        }

        public void SetText(string text)
        {
            IsFinish = false;
            Text = text;
            CurrentIndex = 0;
            CurrentSymbol = Text[CurrentIndex];
        }

        public IEnumerable<Token> Tokenize()
        {
            List<Token> tokens = new List<Token>();
            while (TryGetNextToken(out Token token))
            {
                tokens.Add(token);
                if (token.Type == TokenType.EndOfFile)
                {
                    break;
                }
            }
            return tokens;
        }
        
        public static IEnumerable<Token> Tokenize(string text)
        {
            ILexer lexer = new DefaultLexer(text);
            return lexer.Tokenize();
        }

        public bool TryGetNextToken(out Token token)
        {
            return (token = GetNextToken()) != null;
        }

        public Token? GetNextToken()
        {
            while (!IsFinish)
            {
                if (char.IsNumber(CurrentSymbol)) return AdvanceNumberLiteral();
                if (char.IsLetter(CurrentSymbol)) return AdvanceStringLiteral();
                switch (CurrentSymbol)
                {
                    case ' ': return AdvanceWith(new Token(TokenType.Whitespace, CurrentSymbol.ToString()));
                    case '\t': return AdvanceWith(new Token(TokenType.Tab, CurrentSymbol.ToString()));
                    case '\n': return AdvanceWith(new Token(TokenType.NewLine, CurrentSymbol.ToString()));
                    case '\0': return AdvanceWith(new Token(TokenType.EndOfFile, "\0"));

                    case '+': return AdvanceWith(new Token(TokenType.Plus, CurrentSymbol.ToString()));
                    case '-': return AdvanceWith(new Token(TokenType.Minus, CurrentSymbol.ToString()));
                    case '/': return AdvanceWith(new Token(TokenType.Slash, CurrentSymbol.ToString()));
                    case '\\': return AdvanceWith(new Token(TokenType.Backslash, CurrentSymbol.ToString()));
                    case '*': return AdvanceWith(new Token(TokenType.Asterisk, CurrentSymbol.ToString()));
                    case '=': return AdvanceWith(new Token(TokenType.Equals, CurrentSymbol.ToString()));
                    case '#': return AdvanceWith(new Token(TokenType.NumberSign, CurrentSymbol.ToString()));
                    case '%': return AdvanceWith(new Token(TokenType.PerCentSign, CurrentSymbol.ToString()));
                    case '&': return AdvanceWith(new Token(TokenType.Ampersand, CurrentSymbol.ToString()));
                    case '!': return AdvanceWith(new Token(TokenType.ExclamationMark, CurrentSymbol.ToString()));
                    case '?': return AdvanceWith(new Token(TokenType.QuestionMark, CurrentSymbol.ToString()));
                    case '$': return AdvanceWith(new Token(TokenType.Dollar, CurrentSymbol.ToString()));
                    case '^': return AdvanceWith(new Token(TokenType.Caret, CurrentSymbol.ToString()));
                    case ';': return AdvanceWith(new Token(TokenType.Semicolon, CurrentSymbol.ToString()));
                    case ':': return AdvanceWith(new Token(TokenType.Colon, CurrentSymbol.ToString()));
                    case ',': return AdvanceWith(new Token(TokenType.Comma, CurrentSymbol.ToString()));
                    case '.': return AdvanceWith(new Token(TokenType.Period, CurrentSymbol.ToString()));
                    case '"': return AdvanceWith(new Token(TokenType.DoubleQuotes, CurrentSymbol.ToString()));
                    case '\'': return AdvanceWith(new Token(TokenType.SingleQuote, CurrentSymbol.ToString()));
                    case '_': return AdvanceWith(new Token(TokenType.Underscore, CurrentSymbol.ToString()));
                    case '`': return AdvanceWith(new Token(TokenType.GraveAccent, CurrentSymbol.ToString()));
                    case '|': return AdvanceWith(new Token(TokenType.VerticalBar, CurrentSymbol.ToString()));
                    case '~': return AdvanceWith(new Token(TokenType.Tilde, CurrentSymbol.ToString()));
                    
                    case '(': return AdvanceWith(new Token(TokenType.OpenParen, CurrentSymbol.ToString()));
                    case ')': return AdvanceWith(new Token(TokenType.CloseParen, CurrentSymbol.ToString()));
                    case '<': return AdvanceWith(new Token(TokenType.OpenAngledBracket, CurrentSymbol.ToString()));
                    case '>': return AdvanceWith(new Token(TokenType.CloseAngledBracket, CurrentSymbol.ToString()));
                    case '[': return AdvanceWith(new Token(TokenType.OpenBracket, CurrentSymbol.ToString()));
                    case ']': return AdvanceWith(new Token(TokenType.CloseBracket, CurrentSymbol.ToString()));
                    case '{': return AdvanceWith(new Token(TokenType.OpenBrace, CurrentSymbol.ToString()));
                    case '}': return AdvanceWith(new Token(TokenType.CloseBrace, CurrentSymbol.ToString()));

                    default: return null;
                }
            }
            return null;
        }

        public Token? PeekToken()
        {
            Token token = GetNextToken();
            CurrentIndex -= token.Value.Length;
            CurrentSymbol = Text[CurrentIndex];
            return token;
        }

        private Token AdvanceStringLiteral()
        {
            StringBuilder stringLiteralBuilder = new StringBuilder();
            
            while (!IsFinish &&
                char.IsLetterOrDigit(CurrentSymbol))
            {
                stringLiteralBuilder.Append(CurrentSymbol);
                Advance();
            }

            return new Token(TokenType.StringLiteral, stringLiteralBuilder.ToString());
        }

        private Token AdvanceNumberLiteral()
        {
            StringBuilder stringLiteralBuilder = new StringBuilder();

            while (!IsFinish && 
                char.IsDigit(CurrentSymbol))
            {
                stringLiteralBuilder.Append(CurrentSymbol);
                Advance();
            }

            return new Token(TokenType.NumberLiteral, stringLiteralBuilder.ToString());
        }

        private void Advance()
        {
            if (CurrentIndex + 1 < Text.Length)
            {
                CurrentIndex++;
                CurrentSymbol = Text[CurrentIndex];
            }
            else
            {
                IsFinish = true;
            }
        }

        private Token AdvanceWith(Token token)
        {
            Advance();
            return token;
        }
    }
}
