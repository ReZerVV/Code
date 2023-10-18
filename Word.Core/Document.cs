namespace Word.Core
{
    public class Document
    {
        public DocumentNode Body { get; set; }
        public string Name => Body.InnerText;

        public Document(string name)
        {
            Body = new DocumentNode 
            {
                Type = DocumentNodeType.Body,
                InnerText = name,
            };
        }

        public static Document NewDocument(string name)
        {
            Document document = new Document(name);
            document.Body.CreateNode(
                new DocumentNode 
                {
                    Type = DocumentNodeType.Paragraph,
                    InnerText = string.Empty,
                });
            return document;
        }

        public static Document SampleDocument(string name)
        {
            Document document = new Document("Lorem ipsum");
            document.Body.CreateNode(
                new DocumentNode
                {
                    Type = DocumentNodeType.Paragraph,
                });
            document.Body.ChildNodes[0].CreateNode(
                new DocumentNode
                {
                    Type = DocumentNodeType.Span,
                    InnerText = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Fusce ultricies lacus ultricies, dictum magna a, tincidunt nulla. Aenean volutpat elit at posuere rhoncus. Fusce nec erat neque. Etiam quis magna vel neque facilisis iaculis finibus sit amet purus. Nam non scelerisque orci. Fusce eu ullamcorper neque. Ut et rutrum nulla. Aliquam erat volutpat.",
                    Style = "Standart",
                });
            document.Body.CreateNode(
                new DocumentNode
                {
                    Type = DocumentNodeType.Paragraph,
                });
            document.Body.ChildNodes[1].CreateNode(
                new DocumentNode
                {
                    Type = DocumentNodeType.Span,
                    InnerText = "In et enim gravida, venenatis sapien ac, ullamcorper metus. Etiam aliquam luctus lorem vel euismod. In hac habitasse platea dictumst. Nullam posuere vitae libero et finibus. Nam ac vehicula sapien. Pellentesque rutrum orci accumsan arcu malesuada, sit amet lacinia justo iaculis. Vivamus blandit lobortis gravida.",
                    Style = "Standart",
                });
            return document;
        }
    }
}
