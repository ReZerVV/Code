namespace Word.Core.Syntax.Styles
{
    public class RichText
    {
        public Color ColorOptions { get; set; }
        public Style StyleOptions { get; set; }
        public string Value { get; set; }

        
        public RichText(
            Color colorOptions,
            Style styleOptions,
            string value)
        {
            ColorOptions = colorOptions;
            StyleOptions = styleOptions;
            Value = value;
        }
    }
}
