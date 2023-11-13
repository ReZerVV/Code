using EasyUI.Draw;
using System.Text.RegularExpressions;

namespace Word.Core.Syntax
{
    public class CssLangSyntaxHighlighter : IHighlighter
    {
        public string LangName => "CSS Lang";
        public string LangExtension => @"(css|less|sass|scss)$";
        public Color Default => ApplicationCode.Theme.Foreground;

        private Regex regularExpressionKeywords;
        private Regex regularExpressionComments;
        private Regex regularExpressionStartComments;
        private Regex regularExpressionEndComments;

        private Dictionary<Token.TokenKind, Color> typeColor = new();
        private Dictionary<string, Color> keywordColor = new();

        public CssLangSyntaxHighlighter()
        {
            regularExpressionKeywords = new Regex(@"\b(?:@keyframes|@media|@import|@font-face|@page|@viewport|align-content|align-items|align-self|all|animation|animation-delay|animation-direction|animation-duration|animation-fill-mode|animation-iteration-count|animation-name|animation-play-state|animation-timing-function|backface-visibility|background|background-attachment|background-blur|background-clip|background-color|background-image|background-origin|background-position|background-repeat|background-size|border|border-bottom|border-bottom-color|border-bottom-left-radius|border-bottom-right-radius|border-bottom-style|border-bottom-width|border-collapse|border-color|border-image|border-image-outset|border-image-repeat|border-image-slice|border-image-source|border-image-width|border-left|border-left-color|border-left-style|border-left-width|border-radius|border-right|border-right-color|border-right-style|border-right-width|border-spacing|border-style|border-top|border-top-color|border-top-left-radius|border-top-right-radius|border-top-style|border-top-width|border-width|bottom|box-decoration-break|box-shadow|box-sizing|caption-side|caret-color|clear|clip|color|column-count|column-fill|column-gap|column-rule|column-rule-color|column-rule-style|column-rule-width|column-span|column-width|columns|content|counter-increment|counter-reset|cursor|direction|display|empty-cells|filter|flex|flex-basis|flex-direction|flex-flow|flex-grow|flex-shrink|flex-wrap|float|font|font-family|font-feature-settings|font-kerning|font-language-override|font-size|font-size-adjust|font-stretch|font-style|font-synthesis|font-variant|font-variant-alternates|font-variant-caps|font-variant-east-asian|font-variant-ligatures|font-variant-numeric|font-variant-position|font-weight|grid|grid-area|grid-auto-columns|grid-auto-flow|grid-auto-rows|grid-column|grid-column-end|grid-column-gap|grid-column-start|grid-gap|grid-row|grid-row-end|grid-row-gap|grid-row-start|grid-template|grid-template-areas|grid-template-columns|grid-template-rows|hanging-punctuation|height|hyphens|image-orientation|image-rendering|image-resolution|ime-mode|initial-letter|justify-content|left|letter-spacing|line-break|line-height|list-style|list-style-image|list-style-position|list-style-type|margin|margin-bottom|margin-left|margin-right|margin-top|max-height|max-width|min-height|min-width|mix-blend-mode|object-fit|object-position|opacity|order|orphans|outline|outline-color|outline-offset|outline-style|outline-width|overflow|overflow-wrap|overflow-x|overflow-y|padding|padding-bottom|padding-left|padding-right|padding-top|page-break-after|page-break-before|page-break-inside|perspective|perspective-origin|pointer-events|position|quotes|resize|right|scroll-behavior|tab-size|table-layout|text-align|text-align-last|text-combine-upright|text-decoration|text-decoration-color|text-decoration-line|text-decoration-style|text-indent|text-justify|text-orientation|text-overflow|text-shadow|text-transform|text-underline-position|top|transform|transform-origin|transform-style|transition|transition-delay|transition-duration|transition-property|transition-timing-function|unicode-bidi|user-select|vertical-align|visibility|white-space|widows|width|will-change|word-break|word-spacing|word-wrap|writing-mode|z-index)\b");
            regularExpressionComments = new Regex(@"\/\/.*$");
            regularExpressionStartComments = new Regex(@"\/\*");
            regularExpressionEndComments = new Regex(@"\*\/");

            typeColor.Add(Token.TokenKind.Identifier, Default);
            typeColor.Add(Token.TokenKind.Keyword, new Color(197, 165, 197));
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
