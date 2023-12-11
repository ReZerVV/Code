namespace Word.Core.Syntax.Lexers
{
    public enum TokenType
    {
        NumberLiteral, // Ex. 12313
        StringLiteral, // Ex. hello
        
        Plus, // +
        Minus, // -
        Slash, // /
        Backslash, // \
        Asterisk, // *
        Equals, // =
        NumberSign, // #
        PerCentSign, // %
        Ampersand, // &
        ExclamationMark, // !
        QuestionMark, // ?
        Dollar, // $
        Caret, // ^
        Semicolon, // ;
        Colon, // :
        Comma, // ,
        Period, // .
        DoubleQuotes, // "
        SingleQuote, // '
        Underscore, // _
        GraveAccent, // `
        VerticalBar, // |
        Tilde, // ~

        OpenParen, // (
        CloseParen, // )
        OpenAngledBracket, // <
        CloseAngledBracket, // >
        OpenBracket, // [
        CloseBracket, // ]
        OpenBrace, // {
        CloseBrace, // }

        Whitespace, //  
        NewLine, // \n
        Tab, // \t
        EndOfFile, // \0,
    }
}