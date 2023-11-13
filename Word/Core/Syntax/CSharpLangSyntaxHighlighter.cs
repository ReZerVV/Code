using EasyUI.Draw;
using System.Text.RegularExpressions;

namespace Word.Core.Syntax
{
    public class CSharpLangSyntaxHighlighter : IHighlighter
    {
        public string LangName => "C# (CSharp) Lang";
        public string LangExtension => @"cs$";
        public Color Default => ApplicationCode.Theme.Foreground;

        private Regex regularExpressionKeywords;
        private Regex regularExpressionComments;
        private Regex regularExpressionStartComments;
        private Regex regularExpressionEndComments;

        private Dictionary<Token.TokenKind, Color> typeColor = new();
        private Dictionary<string, Color> keywordColor = new();

        public CSharpLangSyntaxHighlighter()
        {
            regularExpressionKeywords = new Regex(@"\b(?:abstract|as|base|bool|break|byte|case|catch|char|checked|class|const|continue|decimal|default|delegate|do|double|else|enum|event|explicit|extern|false|finally|fixed|float|for|foreach|goto|if|implicit|in|int|interface|internal|is|lock|long|namespace|new|null|object|operator|out|override|params|private|protected|public|readonly|ref|return|sbyte|sealed|short|sizeof|stackalloc|static|string|struct|switch|this|throw|true|try|typeof|uint|ulong|unchecked|unsafe|ushort|using|using static|virtual|void|volatile|while|yield)\b");
            regularExpressionComments = new Regex(@"\/\/.*$");
            regularExpressionStartComments = new Regex(@"\/\*");
            regularExpressionEndComments = new Regex(@"\*\/");

            typeColor.Add(Token.TokenKind.Identifier, new Color(219, 103, 112));
            typeColor.Add(Token.TokenKind.Keyword, new Color(191, 116, 219));
            typeColor.Add(Token.TokenKind.String, new Color(80, 166, 121));
            typeColor.Add(Token.TokenKind.Number, new Color(217, 156, 101));
            typeColor.Add(Token.TokenKind.Operator, Default);
            typeColor.Add(Token.TokenKind.Delimiter, Default);
            typeColor.Add(Token.TokenKind.Comment, new Color(139, 148, 158));
            typeColor.Add(Token.TokenKind.SpecialCharacter, Default);
            typeColor.Add(Token.TokenKind.Whitespace, Default);
            typeColor.Add(Token.TokenKind.Other, Default);
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
