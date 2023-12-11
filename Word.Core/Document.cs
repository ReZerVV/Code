using Code.Core;
using System.Reflection;
using System.Text;
using Word.Core.Syntax.Markers;
using Word.Core.Syntax.Styles;

namespace Word.Core
{
    public class Document
    {
        private string path;

        public int PartOffset
        {
            get
            {
                if (PartBuffer.Count == 0)
                    return 0;
                return PartBuffer.Min(part => part.BeginPartPosition);
            }
        }
        public List<DocumentPart> PartBuffer { get; private set; } = new();
        public Encoding Encoding { get; set; }
        public IMarker Marker { get; set; }
        public bool IsSaved { get; set; }
        public bool IsLoadFromFile { get; set; } = false;
        public string? PathFrom { get; set; } = null;

        public string this[int lineIndex]
        {
            get
            {
                lineIndex -= PartOffset;
                for (int partIndex = 0; partIndex < PartBuffer.Count; partIndex++)
                {
                    if (PartBuffer[partIndex].Buffer.Count < lineIndex)
                    {
                        lineIndex -= PartBuffer[partIndex].Buffer.Count;
                    }
                    else
                    {
                        return PartBuffer[partIndex].Buffer[lineIndex];
                    }
                }
                throw new IndexOutOfRangeException(nameof(lineIndex));
            }
            set
            {
                lineIndex -= PartOffset;
                for (int partIndex = 0; partIndex < PartBuffer.Count; partIndex++)
                {
                    if (PartBuffer[partIndex].Buffer.Count < lineIndex)
                    {
                        lineIndex -= PartBuffer[partIndex].Buffer.Count;
                    }
                    else
                    {
                        PartBuffer[partIndex].Buffer[lineIndex] = value;
                        return;
                    }
                }
                throw new IndexOutOfRangeException(nameof(lineIndex));
            }
        }

        public Document(string name = "Undefined")
        {
            path = string.Empty;
            SetName(name);
            IsSaved = false;

            Marker = GetMarkerByExtension(GetExtension());
            Encoding = Encoding.UTF8;

            PartBuffer.Add(new DocumentPart());
            PartBuffer[0].Buffer.Add(string.Empty);
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

        public int GetPartIndexByLineIndex(int lineIndex)
        {
            lineIndex -= PartOffset;
            for (int partIndex = 0; partIndex < PartBuffer.Count; partIndex++)
            {
                if (PartBuffer[partIndex].Buffer.Count - 1 < lineIndex)
                {
                    lineIndex -= PartBuffer[partIndex].Buffer.Count - 1;
                }
                else
                {
                    return partIndex;
                }
            }
            throw new IndexOutOfRangeException(nameof(lineIndex));
        }
        public int GetDocumentSize()
        {
            if (PathFrom != null && File.Exists(PathFrom))
            {
                return DocumentLoader.GetCountLinesFromFile(this);
            }
            else
            {
                int lineCount = PartBuffer.Min(part => part.BeginPartPosition);
                foreach (DocumentPart docPart in PartBuffer)
                {
                    lineCount += docPart.Buffer.Count;
                }
                return lineCount;
            }
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
        public bool TrySave()
            => DocumentLoader.TrySave(this);

        public void Clear()
        {
            PartBuffer.Clear();
        }

        public List<List<RichText>> Markup()
        {
            return Marker.Markup(PartBuffer.SelectMany(part => part.Buffer).ToList());
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
