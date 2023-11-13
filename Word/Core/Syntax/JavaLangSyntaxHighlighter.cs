using EasyUI.Draw;
using System.Text.RegularExpressions;

namespace Word.Core.Syntax
{
    public class JavaLangSyntaxHighlighter : IHighlighter
    {
        public string LangName => "Java Lang";
        public string LangExtension => @"java$";
        public Color Default => ApplicationCode.Theme.Foreground;
        
        private Regex regularExpressionKeywords;
        private Regex regularExpressionComments;
        private Regex regularExpressionStartComments;
        private Regex regularExpressionEndComments;

        private Dictionary<Token.TokenKind, Color> typeColor = new();
        private Dictionary<string, Color> keywordColor = new();

        public JavaLangSyntaxHighlighter()
        {
            regularExpressionKeywords = new Regex(@"\b(?:abstract|continue|for|new|switch|assert|default|goto|package|synchronized|boolean|do|if|private|this|break|double|implements|protected|throw|byte|else|import|public|throws|case|enum|instanceof|return|transient|catch|extends|int|short|try|char|final|interface|static|void|class|finally|long|strictfp|volatile|const|float|native|super|while)\b");
            regularExpressionComments = new Regex(@"\/\/.*$");
            regularExpressionStartComments = new Regex(@"\/\*");
            regularExpressionEndComments = new Regex(@"\*\/");

            typeColor.Add(Token.TokenKind.Identifier, Default);
            typeColor.Add(Token.TokenKind.Keyword, new Color(206, 123, 41));
            typeColor.Add(Token.TokenKind.String, new Color(70, 173, 255));
            typeColor.Add(Token.TokenKind.Number, new Color(138, 206, 255));
            typeColor.Add(Token.TokenKind.Operator, new Color(206, 123, 41));
            typeColor.Add(Token.TokenKind.Delimiter, Default);
            typeColor.Add(Token.TokenKind.Comment, new Color(127, 144, 158));
            typeColor.Add(Token.TokenKind.SpecialCharacter, Default);
            typeColor.Add(Token.TokenKind.Whitespace, Default);
            typeColor.Add(Token.TokenKind.Other, Default);

            keywordColor.Add("null", new Color(138, 206, 255));
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
