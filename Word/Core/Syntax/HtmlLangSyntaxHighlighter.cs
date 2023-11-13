using EasyUI.Draw;
using System.Text.RegularExpressions;

namespace Word.Core.Syntax
{
    public class HtmlLangSyntaxHighlighter : IHighlighter
    {
        public string LangName => "HTML Lang";
        public string LangExtension => @"html$";
        public Color Default => ApplicationCode.Theme.Foreground;

        private Regex regularExpressionKeywords;
        private Regex regularExpressionComments;
        private Regex regularExpressionStartComments;
        private Regex regularExpressionEndComments;

        private Dictionary<Token.TokenKind, Color> typeColor = new();
        private Dictionary<string, Color> keywordColor = new();

        public HtmlLangSyntaxHighlighter()
        {
            regularExpressionKeywords = new Regex(@"\b(?:html|head|title|meta|body|div|p|h[1-6]|a|img|ul|ol|li|table|tr|td)\b");
            regularExpressionComments = new Regex(@"\/\/.*$");
            regularExpressionStartComments = new Regex(@"\/\*");
            regularExpressionEndComments = new Regex(@"\*\/");

            typeColor.Add(Token.TokenKind.Identifier, new Color(121, 201, 125));
            typeColor.Add(Token.TokenKind.Keyword, new Color(240, 153, 153));
            typeColor.Add(Token.TokenKind.String, Default);
            typeColor.Add(Token.TokenKind.Number, Default);
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
