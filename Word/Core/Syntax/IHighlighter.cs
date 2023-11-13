namespace Word.Core.Syntax
{
    public interface IHighlighter
    {
        string LangName { get; }
        string LangExtension { get; }
        List<TextFragmentInfo> Highlight(string text);
    }
}
