namespace Word.Core
{
    public class Cursor
    {
        public int Offset;
        public int Line;

        public Cursor(int offset = 0, int line = 0)
        {
            Offset = offset;
            Line = line;
        }
    }

    public class Editor
    {
        public int TabSize { get; set; } = 4;

        private Cursor cursor;
        public Cursor Cursor => cursor;
        private Document document;
        public Document Document
        {
            get
            {
                return document;
            }
            set
            {
                document = value;
                cursor = new Cursor();
            }
        }

        public Editor(Document document = null)
        {
            Document = document;
        }

        public void CursorMoveLeft()
        {
            if (Document == null)
            {
                return;
            }
            if (cursor.Offset - 1 >= 0)
            {
                cursor.Offset--;
            }
            else if (CursorMoveUp())
            {
                cursor.Offset = Document.Lines[cursor.Line].Length;
            }
        }

        public void CursorMoveRight()
        {
            if (Document == null)
            {
                return;
            }
            if (cursor.Offset + 1 <= Document.Lines[cursor.Line].Length)
            {
                cursor.Offset++;
            }
            else if (CursorMoveDown())
            {
                cursor.Offset = 0;
            }
        }

        public bool CursorMoveUp()
        {
            if (cursor.Line > 0)
            {
                cursor.Line--;
                if (cursor.Offset > Document.Lines[cursor.Line].Length)
                {
                    cursor.Offset = Document.Lines[cursor.Line].Length;
                }
                return true;
            }
            return false;
        }

        public bool CursorMoveDown()
        {
            if (cursor.Line + 1 < Document.Lines.Count)
            {
                cursor.Line++;
                if (cursor.Offset > Document.Lines[cursor.Line].Length)
                {
                    cursor.Offset = Document.Lines[cursor.Line].Length;
                }
                return true;
            }
            return false;
        }

        public void CursorMoveToStartLine()
        {
            if (Document == null)
            {
                return;
            }
            Cursor.Offset = 0;
        }

        public void CursorMoveToEndLine()
        {
            if (Document == null)
            {
                return;
            }
            Cursor.Offset = Document.Lines[Cursor.Line].Length;
        }

        public void InsertText(string text)
        {
            if (Document == null)
            {
                return;
            }
            document.IsSaved = false;
            document.Lines[cursor.Line] = document.Lines[cursor.Line].Substring(0, cursor.Offset)
                + text
                + (cursor.Offset >= document.Lines[cursor.Line].Length ? ""
                : document.Lines[cursor.Line].Substring(cursor.Offset));
            Cursor.Offset++;
        }

        public void NewLine()
        {
            if (Document == null)
            {
                return;
            }
            document.IsSaved = false;
            if (Cursor.Offset < Document.Lines[Cursor.Line].Length)
            {
                Document.Lines.Insert(
                    Cursor.Line + 1,
                    document.Lines[cursor.Line].Substring(cursor.Offset));
                Document.Lines[Cursor.Line] = document.Lines[cursor.Line].Substring(0, cursor.Offset);
            }
            else
            {
                Document.Lines.Insert(Cursor.Line + 1, string.Empty);
            }

            Cursor.Line++;
            Cursor.Offset = 0;
        }

        public void Tab()
        {
            if (Document == null)
            {
                return;
            }
            document.IsSaved = false;
            document.Lines[cursor.Line] = document.Lines[cursor.Line].Substring(0, cursor.Offset)
                + new string(' ', TabSize - cursor.Offset % TabSize)
                + (cursor.Offset >= document.Lines[cursor.Line].Length ? ""
                : document.Lines[cursor.Line].Substring(cursor.Offset));
            Cursor.Offset += TabSize - cursor.Offset % TabSize;
        }

        public void RemoveText()
        {
            if (Document == null)
            {
                return;
            }
            document.IsSaved = false;
            if (Cursor.Offset == 0)
            {
                if (CursorMoveUp())
                {
                    Cursor.Offset = document.Lines[Cursor.Line].Length;
                    document.Lines[Cursor.Line] += document.Lines[Cursor.Line + 1];
                    document.Lines.RemoveAt(Cursor.Line + 1);
                }
                return;
            }
            document.Lines[Cursor.Line] = document.Lines[Cursor.Line].Substring(0, Cursor.Offset - 1)
                + (Cursor.Offset >= document.Lines[Cursor.Line].Length ? ""
                : document.Lines[Cursor.Line].Substring(Cursor.Offset));
            Cursor.Offset--;
        }

        public void Rename(string name)
        {
            document.IsSaved = false;
            Document.Name = name;
        }
    }
}
