using System.Xml;

namespace Word.Core
{
    public enum DocumentNodeType 
    {
        Body,
        Paragraph, // Перевод на новую строку
        Span, // Текст
        List,
        ListItem,
    }

    public class DocumentNode
    {
        public DocumentNodeType Type;
        
        private string? style = null;
        public string? Style 
        { 
            get => style;
            set => SetStyle(value);
        }
        public string? InnerText = null;
        public DocumentNode? ParentNode = null;
        public List<DocumentNode> ChildNodes = new List<DocumentNode>();

        // Возвращает предыдущий одноуровневый узел.
        public DocumentNode? PreviousSibling => GetPreviousSibling();
        private DocumentNode? GetPreviousSibling()
        {
            if (ParentNode != null &&
                ParentNode.LastChild != null &&
                ParentNode.FirstChild != this)
            {
                return ParentNode.ChildNodes[ParentNode.ChildNodes.IndexOf(this) - 1];
            }
            return null;
        }

        // Возвращает следующий одноуровневый узел.
        public DocumentNode? NextSibling => GetNextSibling();
        private DocumentNode? GetNextSibling()
        {
            if (ParentNode != null &&
                ParentNode.LastChild != null &&
                ParentNode.LastChild != this)
            {
                return ParentNode.ChildNodes[ParentNode.ChildNodes.IndexOf(this) + 1];
            }
            return null;
        }

        public DocumentNode()
        {
            XmlDocument document;
        }

        // Имеет ли узел детей
        public bool HasChildNodes()
            => ChildNodes.Count > 0;

        public DocumentNode? FirstChild
            => HasChildNodes() ? ChildNodes.First() : null;

        public DocumentNode? LastChild
            => HasChildNodes() ? ChildNodes.Last() : null;

        private void SetStyle(string style)
        {
            if (ParentNode != null &&
                Type == DocumentNodeType.Span)
            {
                if (PreviousSibling?.Style == style)
                {
                    DocumentNode newDocumentNode = new DocumentNode
                    {
                        Type = DocumentNodeType.Span,
                        InnerText = PreviousSibling.InnerText + InnerText,
                    };
                    ParentNode.InsertAfter(newDocumentNode, PreviousSibling);
                    ParentNode.RemoveNode(PreviousSibling);
                    ParentNode.RemoveNode(this);
                    newDocumentNode.SetStyle(style);
                    return;
                }
                if (NextSibling?.Style == style)
                {
                    DocumentNode newDocumentNode = new DocumentNode
                    {
                        Type = DocumentNodeType.Span,
                        InnerText = InnerText + NextSibling.InnerText,
                    };
                    ParentNode.InsertAfter(newDocumentNode, this);
                    ParentNode.RemoveNode(NextSibling);
                    ParentNode.RemoveNode(this);
                    newDocumentNode.SetStyle(style);
                    return;
                }
                this.style = style;
            }
            else
            {
                this.style = style;
            }
        }

        public void CreateNode(DocumentNode documentNode)
        {
            documentNode.ParentNode = this;
            ChildNodes.Add(documentNode);
            documentNode.SetStyle(documentNode.Style);
        }

        public void RemoveNode(DocumentNode documentNode)
        {
            ChildNodes.Remove(documentNode);
        }

        public void RemoveNode(int index)
        {
            ChildNodes.RemoveAt(index);
        }

        public void InsertAfter(DocumentNode newDocumentNode, DocumentNode refDocumentNode)
        {
            int indexRefDocumentNode = ChildNodes.IndexOf(refDocumentNode);
            if (indexRefDocumentNode == -1)
            {
                return;
            }
            newDocumentNode.ParentNode = this;
            ChildNodes.Insert(indexRefDocumentNode, newDocumentNode);
        }

        public void InsertBefore(DocumentNode newDocumentNode, DocumentNode refDocumentNode)
        {
            int indexRefDocumentNode = ChildNodes.IndexOf(refDocumentNode);
            if (indexRefDocumentNode == -1)
            {
                return;
            }
            newDocumentNode.ParentNode = this;
            if (indexRefDocumentNode == ChildNodes.Count - 1)
            {
                ChildNodes.Add(newDocumentNode);
            }
            else
            { 
                ChildNodes.Insert(indexRefDocumentNode + 1, newDocumentNode);
            }
        }
    }
}