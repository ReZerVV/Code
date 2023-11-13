namespace Word.Core.Syntax
{
    public class Token
    {
        public enum TokenKind
        {
            Identifier = 0,
            Keyword,
            String,
            Number,
            Operator,
            Delimiter,
            Comment,
            SpecialCharacter,
            Whitespace,
            Other,
            EndLine,
        }

        public string Content { get; set; }
        public TokenKind Type { get; set; }
        public int StartPosition { get; set; }
        public int EndPosition => StartPosition + Content.Length - 1;

        public Token(string content, TokenKind type, int startPosition)
        {
            Content = content;
            Type = type;
            StartPosition = startPosition;
        }
    }
}
