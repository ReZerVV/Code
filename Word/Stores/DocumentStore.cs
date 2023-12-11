using Word.Core;

namespace Word.Stores
{
    public class DocumentStore
    {
        public List<Document> Docs { get; set; } = new List<Document>();
        public bool IsDocChanged { get; set; } = false;
        public Document? CurrentDoc { get; set; } = null;
        public int CurrentIndex { get; set; } = 0;
        public Document? GetCurrentDoc()
        {
            if (CurrentIndex < 0 || CurrentIndex >= Docs.Count)
            {
                return null;
            }
            return Docs[CurrentIndex];
        }
        public DocumentCursor Cursor { get; set; }

        public void MoveToNextDoc()
        {
            if (CurrentIndex + 1 < Docs.Count)
            {
                CurrentIndex++;
            }
            else
            {
                CurrentIndex = 0;
            }
            CurrentDoc = GetCurrentDoc();
            Cursor = new DocumentCursor(CurrentDoc);
            IsDocChanged = true;
        }
        public void MoveToPrevDoc()
        {
            if (CurrentIndex - 1 >= 0)
            {
                CurrentIndex--;
            }
            else
            {
                CurrentIndex = Docs.Count - 1;
            }
            CurrentDoc = GetCurrentDoc();
            Cursor = new DocumentCursor(CurrentDoc);
            IsDocChanged = true;
        }

        public void OpenDoc(Document doc)
        {
            Docs.Add(doc);
            CurrentDoc = doc;
            CurrentIndex = Docs.Count - 1;
            Cursor = new DocumentCursor(doc);
            IsDocChanged = true;
        }
        public void CloseDoc()
        {
            Docs.Remove(GetCurrentDoc());
            CurrentIndex = Docs.Count - 1;
            CurrentDoc = GetCurrentDoc();
            IsDocChanged = true;
        }
        public void LoadDoc(string path)
        {
            Document doc = new Document(string.Empty);
            doc.SetPath(path);
            doc.TryLoad();
            OpenDoc(doc);
        }
    }
}
