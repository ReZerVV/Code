namespace Word.Core.Test 
{
    internal class Program 
    {
        public static void Main(string[] args)
        {
            Document doc = new Document("test");
            doc.Body.CreateNode(
                new DocumentNode 
                {
                    Type = DocumentNodeType.Paragraph,
                });
            doc.Body.ChildNodes[0].CreateNode(
                new DocumentNode
                {
                    Type = DocumentNodeType.Span,
                    InnerText = "Hello",
                    Style = "Standart",
                });
            doc.Body.ChildNodes[0].CreateNode(
                new DocumentNode
                {
                    Type = DocumentNodeType.Span,
                    InnerText = " world",
                    Style = "Standart",
                });
            return;
        }
    }
}