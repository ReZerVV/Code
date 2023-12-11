namespace Word.Core.Syntax.Lexers
{
    public interface ILexer
    {
        IEnumerable<Token> Tokenize();
        Token? GetNextToken();
        Token? PeekToken();
        bool TryGetNextToken(out Token token);

        abstract static IEnumerable<Token> Tokenize(string text);
    }
}
