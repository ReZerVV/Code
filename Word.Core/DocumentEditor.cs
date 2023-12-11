namespace Word.Core
{
    public class DocumentEditor
    {
        public static void InsertTab(Document doc, DocumentCursor cursor)
        {
            InsertText(new string(' ', 4 - cursor.Offset % 4), doc, cursor);
        }

        public static void InsertNewLine(Document doc, DocumentCursor cursor)
        {
            if (doc == null)
            {
                return;
            }
            doc.IsSaved = false;
            if (cursor.Offset < doc.Buffer[cursor.GetLineIndex()].Length)
            {
                doc.Buffer.Insert(
                cursor.Line + 1,
                   doc.Buffer[cursor.GetLineIndex()].Substring(cursor.Offset));
                doc.Buffer[cursor.GetLineIndex()] = doc.Buffer[cursor.GetLineIndex()].Substring(0, cursor.Offset);
            }
            else
            {
                doc.Buffer.Insert(cursor.Line + 1, string.Empty);
            }
            cursor.TryMoveDown();
            cursor.Offset = 0;
        }

        public static void InsertText(string text, Document doc, DocumentCursor cursor)
        {
            if (doc == null)
            {
                return;
            }
            doc.IsSaved = false;
            doc.Buffer[cursor.GetLineIndex()] = doc.Buffer[cursor.GetLineIndex()].Substring(0, cursor.Offset)
                + text
                + (cursor.Offset >= doc.Buffer[cursor.GetLineIndex()].Length ? ""
                : doc.Buffer[cursor.GetLineIndex()].Substring(cursor.Offset));
            cursor.TryMoveRight(text.Length);
        }

        public static void Remove(Document doc, DocumentCursor cursor)
        {
            if (doc == null)
            {
                return;
            }
            doc.IsSaved = false;
            if (cursor.Offset == 0)
            {
                if (cursor.TryMoveUp())
                {
                    cursor.Offset = doc.Buffer[cursor.GetLineIndex()].Length;
                    doc.Buffer[cursor.Line] += doc.Buffer[cursor.GetLineIndex() + 1];
                    doc.Buffer.RemoveAt(cursor.GetLineIndex() + 1);
                }
                return;
            }
            doc.Buffer[cursor.GetLineIndex()] = doc.Buffer[cursor.GetLineIndex()].Substring(0, cursor.Offset - 1)
                + (cursor.Offset >= doc.Buffer[cursor.GetLineIndex()].Length ? ""
                : doc.Buffer[cursor.GetLineIndex()].Substring(cursor.Offset));
            cursor.TryMoveLeft();
        }
    }
}
