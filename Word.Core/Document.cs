using System.Collections;

namespace Word.Core
{
    public class Document : IEnumerable<DocumentNode>
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

        public static Document SampleDocument()
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
                    InnerText = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque semper lacus at dignissim eleifend. Nunc lacinia, urna ut varius suscipit, nulla risus tempor nisi, sed feugiat eros orci et dolor. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Aliquam dictum eleifend mi interdum viverra. Sed nulla risus, ultricies eget ante vitae, porta sollicitudin nulla. Nunc euismod sapien vel dui placerat, nec tempor nisi aliquam. Quisque molestie nulla interdum tristique convallis. Nam vestibulum quam non ultrices dapibus. Integer et arcu nec lacus sodales dignissim eget ac mi. Ut ligula nunc, rhoncus non enim ut, fermentum lacinia mi. Duis condimentum nec justo non sollicitudin.",
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
                    InnerText = "Quisque vulputate mi ac sapien suscipit lobortis. Donec rhoncus vehicula lobortis. Sed sollicitudin pulvinar lacus at imperdiet. Sed pulvinar consequat mauris vel rutrum. Maecenas faucibus imperdiet ipsum in consequat. Quisque libero nisi, mattis sed rhoncus at, euismod fermentum augue. Sed at egestas purus. Mauris ac ante vestibulum mi luctus ultricies vel non massa. Phasellus pharetra ligula in felis tincidunt semper. Aenean sed aliquam tortor.",
                    Style = "Standart",
                });
            document.Body.CreateNode(
                new DocumentNode
                {
                    Type = DocumentNodeType.Paragraph,
                });
            document.Body.ChildNodes[2].CreateNode(
                new DocumentNode
                {
                    Type = DocumentNodeType.Span,
                    InnerText = "Suspendisse eget lacinia risus. Proin posuere molestie nisi, ut hendrerit justo aliquet viverra. Etiam eget eros tincidunt, placerat felis vitae, tempor enim. Curabitur tristique malesuada neque. In eget neque elit. Aenean non ornare nisi. Ut gravida maximus massa non maximus. Cras et purus eget mauris accumsan volutpat eu id mauris. Maecenas sed hendrerit est. Sed eu pellentesque risus. In tincidunt purus in eros euismod, ut ullamcorper dolor vehicula.",
                    Style = "Standart",
                });
            document.Body.CreateNode(
                new DocumentNode
                {
                    Type = DocumentNodeType.Paragraph,
                });
            document.Body.ChildNodes[3].CreateNode(
                new DocumentNode
                {
                    Type = DocumentNodeType.Span,
                    InnerText = "Ut pretium quis ante in auctor. Cras eget auctor lectus. Fusce at vestibulum turpis. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Aliquam euismod mi eget nisi porttitor, in porta ipsum faucibus. Suspendisse tempus lorem ut massa pharetra consectetur. Vivamus nibh lorem, tempus sit amet faucibus sodales, dapibus quis urna. Interdum et malesuada fames ac ante ipsum primis in faucibus. Curabitur at fringilla ligula. Nulla facilisi. Proin in urna massa. Vestibulum at malesuada felis, id consequat leo. Mauris lacinia quam ut fringilla porta. Aliquam malesuada varius maximus.",
                    Style = "Standart",
                });
            document.Body.CreateNode(
                new DocumentNode
                {
                    Type = DocumentNodeType.Paragraph,
                });
            document.Body.ChildNodes[4].CreateNode(
                new DocumentNode
                {
                    Type = DocumentNodeType.Span,
                    InnerText = "Duis mollis lobortis diam, at luctus nisi sagittis ac. Etiam tristique ultricies dapibus. Cras accumsan, lectus a finibus fringilla, ligula sapien vulputate justo, eget iaculis augue leo sed arcu. Fusce eu vestibulum sapien. Donec dictum lacus et est dictum consectetur. Donec gravida cursus dui eu rhoncus. Integer risus massa, malesuada in volutpat efficitur, sodales sit amet augue. Suspendisse lobortis at tortor vitae eleifend. Nullam ultricies eros a mi dictum, non ultricies nisl scelerisque. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec a ante leo. Quisque id purus sit amet felis tincidunt lobortis.",
                    Style = "Standart",
                });
            return document;
        }

        public IEnumerator<DocumentNode> GetEnumerator()
        {
            ICollection<DocumentNode> listOfDocumentNodes = new List<DocumentNode>();
            Traverse(Body, listOfDocumentNodes);
            foreach (DocumentNode documentNode in listOfDocumentNodes)
            {
                yield return documentNode;
            }
        }

        private void Traverse(DocumentNode documentNode, ICollection<DocumentNode> listOfDocumentNodes)
        {
            listOfDocumentNodes.Add(documentNode);
            foreach (DocumentNode documentChildNode in documentNode.ChildNodes)
            {
                Traverse(documentChildNode, listOfDocumentNodes);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
