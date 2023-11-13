using EasyUI.Draw;
using System.Text.RegularExpressions;

namespace Word.Core.Syntax
{
    public class JavaScriptLangSyntaxHighlighter : IHighlighter
    {
        public string LangName => "Java Script Lang";
        public string LangExtension => @"js$";
        public Color Default => ApplicationCode.Theme.Foreground;

        private Regex regularExpressionKeywords;
        private Regex regularExpressionComments;
        private Regex regularExpressionStartComments;
        private Regex regularExpressionEndComments;

        private Dictionary<Token.TokenKind, Color> typeColor = new();
        private Dictionary<string, Color> keywordColor = new();

        public JavaScriptLangSyntaxHighlighter()
        {
            regularExpressionKeywords = new Regex(@"\b(?:break|case|catch|class|const|continue|debugger|default|delete|do|else|export|extends|finally|for|function|if|import|in|instanceof|new|return|super|switch|this|throw|try|typeof|var|let|void|while|with|yield)\b");
            regularExpressionComments = new Regex(@"\/\/.*$");
            regularExpressionStartComments = new Regex(@"\/\*");
            regularExpressionEndComments = new Regex(@"\*\/");

            typeColor.Add(Token.TokenKind.Identifier, Default);
            typeColor.Add(Token.TokenKind.Keyword, new Color(255, 88, 52));
            typeColor.Add(Token.TokenKind.String, new Color(70, 173, 255));
            typeColor.Add(Token.TokenKind.Number, new Color(138, 206, 255));
            typeColor.Add(Token.TokenKind.Operator, new Color(255, 88, 52));
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
