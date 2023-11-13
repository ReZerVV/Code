using EasyUI.Draw;
using System.Text.RegularExpressions;

namespace Word.Core.Syntax
{
    public class TypeScriptLangSyntaxHighlighter : IHighlighter
    {
        public string LangName => "Type Script Lang";
        public string LangExtension => @"ts$";
        public Color Default => ApplicationCode.Theme.Foreground;

        private Regex regularExpressionKeywords;
        private Regex regularExpressionComments;
        private Regex regularExpressionStartComments;
        private Regex regularExpressionEndComments;

        private Dictionary<Token.TokenKind, Color> typeColor = new();
        private Dictionary<string, Color> keywordColor = new();

        public TypeScriptLangSyntaxHighlighter()
        {
            regularExpressionKeywords = new Regex(@"\b(?:abstract|as|await|break|case|catch|class|const|continue|debugger|declare|default|delete|do|else|enum|export|extends|false|finally|for|from|function|get|if|implements|import|in|instanceof|interface|is|keyof|let|module|namespace|new|null|number|object|package|private|protected|public|readonly|require|return|set|static|string|super|switch|this|throw|true|try|type|typeof|undefined|var|void|while|with|yield)\b");
            regularExpressionComments = new Regex(@"\/\/.*$");
            regularExpressionStartComments = new Regex(@"\/\*");
            regularExpressionEndComments = new Regex(@"\*\/");

            typeColor.Add(Token.TokenKind.Identifier, new Color(57, 187, 145));
            typeColor.Add(Token.TokenKind.Keyword, new Color(160, 102, 122));
            typeColor.Add(Token.TokenKind.String, new Color(172, 125, 85));
            typeColor.Add(Token.TokenKind.Number, new Color(138, 206, 255));
            typeColor.Add(Token.TokenKind.Operator, new Color(255, 88, 52));
            typeColor.Add(Token.TokenKind.Delimiter, Default);
            typeColor.Add(Token.TokenKind.Comment, new Color(139, 148, 158));
            typeColor.Add(Token.TokenKind.SpecialCharacter, Default);
            typeColor.Add(Token.TokenKind.Whitespace, Default);
            typeColor.Add(Token.TokenKind.Other, Default);

            keywordColor.Add("implements", new Color(38, 61, 148));
            keywordColor.Add("class", new Color(38, 61, 148));
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
