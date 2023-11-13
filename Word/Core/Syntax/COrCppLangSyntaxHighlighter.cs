using EasyUI.Draw;
using System.Text.RegularExpressions;

namespace Word.Core.Syntax
{
    public class COrCppLangSyntaxHighlighter : IHighlighter
    {
        public string LangName => "C/C++ Lang";
        public string LangExtension => @"(c|cpp)$";
        public Color Default => ApplicationCode.Theme.Foreground;

        private Regex regularExpressionKeywords;
        private Regex regularExpressionComments;
        private Regex regularExpressionStartComments;
        private Regex regularExpressionEndComments;

        private Dictionary<Token.TokenKind, Color> typeColor = new();
        private Dictionary<string, Color> keywordColor = new();

        public COrCppLangSyntaxHighlighter()
        {
            regularExpressionKeywords = new Regex(@"\b(?:auto|break|case|char|const|continue|default|do|double|else|enum|extern|float|for|goto|if|int|long|register|return|short|signed|sizeof|static|struct|switch|typedef|union|unsigned|void|volatile|while|alignas|alignof|and|and_eq|asm|bitand|bitor|bool|compl|concept|const_cast|co_await|co_return|co_yield|decltype|delete|dynamic_cast|explicit|export|false|friend|inline|mutable|namespace|new|noexcept|not|not_eq|nullptr|operator|or|or_eq|private|protected|public|reinterpret_cast|requires|static_assert|static_cast|template|this|thread_local|throw|true|try|typeid|typename|using|virtual|wchar_t|xor|xor_eq)\b");
            regularExpressionComments = new Regex(@"\/\/.*$");
            regularExpressionStartComments = new Regex(@"\/\*");
            regularExpressionEndComments = new Regex(@"\*\/");

            typeColor.Add(Token.TokenKind.Identifier, Default);
            typeColor.Add(Token.TokenKind.Keyword, new Color(158, 119, 192));
            typeColor.Add(Token.TokenKind.String, new Color(202, 138, 105));
            typeColor.Add(Token.TokenKind.Number, new Color(217, 156, 101));
            typeColor.Add(Token.TokenKind.Operator, Default);
            typeColor.Add(Token.TokenKind.Delimiter, Default);
            typeColor.Add(Token.TokenKind.Comment, new Color(139, 148, 158));
            typeColor.Add(Token.TokenKind.SpecialCharacter, Default);
            typeColor.Add(Token.TokenKind.Whitespace, Default);
            typeColor.Add(Token.TokenKind.Other, Default);

            keywordColor.Add("namespace", new Color(46, 111, 200));
            keywordColor.Add("int", new Color(46, 111, 200));
        }

        public List<TextFragmentInfo> Highlight(string text)
        {
            var tokens = Lexer.Tokenize(
                text,
                regularExpressionKeywords,
                regularExpressionComments,
                regularExpressionStartComments,
                regularExpressionEndComments);

            List<TextFragmentInfo> result = new();
            for (int tokenIndex = 0; tokenIndex < tokens.Count; tokenIndex++)
            {
                Color color = null;
                if (tokens[tokenIndex].Type == Token.TokenKind.Keyword)
                {
                    keywordColor.TryGetValue(tokens[tokenIndex].Content, out color);
                }
                if (color == null)
                {
                    color = typeColor[tokens[tokenIndex].Type];
                }
                result.Add(new TextFragmentInfo(tokens[tokenIndex].Content, color, PixelStyle.StyleNone));
            }
            return result;
        }
    }
}
