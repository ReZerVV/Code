using System.Reflection;
using System.Text;
using Word.Core.Syntax.Markers;
using Word.Core.Syntax.Styles;

namespace Word.Core
{
    public class Document
    {
        private string path;

        public int PartOffset { get; set; }
        public List<string> Buffer { get; private set; }
        public Encoding Encoding { get; set; }
        public IMarker Marker { get; set; }
        public bool IsSaved { get; set; }
        
        public Document(string name = "Undefined")
        {
            path = string.Empty;
            SetName(name);
            IsSaved = false;

            Marker = GetMarkerByExtension(GetExtension());
            Encoding = Encoding.UTF8;

            PartOffset = 0;
            Buffer = new List<string>();
            Buffer.Add(string.Empty);
        }
        
        private static IMarker GetMarkerByExtension(string extension)
        {
            var marker = Assembly.GetExecutingAssembly().GetTypes()
                    .Where(type => typeof(IMarker).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract)
                    .Select(type => (IMarker)Activator.CreateInstance(type))
                    .FirstOrDefault(marker => marker.LangExtension == extension);
            if (marker == null)
            {
                return new PlainTextMarker();
            }
            return marker;
        }

        public string GetExtension()
        {
            return Path.GetExtension(path);
        }
        public void SetName(string name)
        {
            path = path.Substring(0, path.Length - GetName().Length)
                + name;
        }
        public string GetName()
        {
            return Path.GetFileName(path);
        }
        public void SetPath(string pathToFile)
        {
            path = pathToFile.Trim();
            Marker = GetMarkerByExtension(GetExtension());
        }
        public string GetPath()
        {
            return path;
        }
        public bool HasPath()
        {
            return path != string.Empty;
        }

        public static Document? TryLoadFile(string path, int partSize = 100)
        {
            if (DocumentLoader.TryLoadFilePart(path, 0, partSize, out Document doc))
            {
                doc.SetPath(path);
                return doc;
            }
            return null;
        }
        public bool TryLoadFilePart(int start, int count)
            => DocumentLoader.TryLoadFilePart(this, start, count);
        public bool TrySave(string path)
            => DocumentLoader.TrySave(this, path);
        public bool TrySave()
            => DocumentLoader.TrySave(this, this.path);

        public int GetTotalLineCount()
        {
            if (DocumentLoader.TryGetLineCountFromFile(this, out int size))
            {
                return size;
            }
            return Buffer.Count;
        }

        public void Clear()
        {
            Buffer.Clear();
        }

        public List<List<RichText>> Markup()
        {
            return Marker.Markup(Buffer);
        }

        public void InsertTab(DocumentCursor cursor)
            => DocumentEditor.InsertTab(this, cursor);
        public void InsertNewLine(DocumentCursor cursor)
            => DocumentEditor.InsertNewLine(this, cursor);
        public void InsertText(string text, DocumentCursor cursor)
            => DocumentEditor.InsertText(text, this, cursor);
        public void Remove(DocumentCursor cursor)
            => DocumentEditor.Remove(this, cursor);
    }
}
