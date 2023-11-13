using Word.Core;

namespace Word.Stores
{
    public class DocumentStore
    {
        public List<Document> Documents { get; set; } = new();
        
        public int CurrentIndex { get; set; } = 0;
        public Document Current => GetCurrentDocument();
        
        public Document? GetCurrentDocument()
        {
            if (CurrentIndex < 0 || CurrentIndex >= Documents.Count)
            {
                return null;
            }
            return Documents[CurrentIndex];
        }

        public void MoveToNextDocument()
        {
            if (CurrentIndex + 1 < Documents.Count)
            {
                CurrentIndex++;
            }
            else
            {
                CurrentIndex = 0;
            }
        }

        public void MoveToPrevDocument()
        {
            if (CurrentIndex - 1 >= 0)
            {
                CurrentIndex--;
            }
            else
            {
                CurrentIndex = Documents.Count - 1;
            }
        }

        public void CreateAndOpenDocument(string name)
        {
            OpenDocument(new Document(name));
        }

        public void OpenDocument(Document document)
        {
            Documents.Add(document);
            CurrentIndex = Documents.Count - 1;
        }
        
        public void CloseDocument()
        {
            Documents.Remove(Current);
            CurrentIndex = Documents.Count - 1;
        }

        public void LoadDocument(string path)
        {
            Document document = new Document(string.Empty);
            document.Load(path);
            OpenDocument(document);
        }
    }
}
