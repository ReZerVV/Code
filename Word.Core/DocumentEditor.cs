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
            if (cursor.Offset < doc[cursor.Line].Length)
            {
                int partIndex = doc.GetPartIndexByLineIndex(cursor.Line);
                int lineIndex = cursor.Line;
                doc.PartBuffer[partIndex].Buffer.Insert(
                    lineIndex + 1,
                    doc[cursor.Line].Substring(cursor.Offset)
                );
                doc[cursor.Line] = doc[cursor.Line].Substring(0, cursor.Offset);
            }
            else
            {
                int partIndex = doc.GetPartIndexByLineIndex(cursor.Line);
                int lineIndex = cursor.Line;
                doc.PartBuffer[partIndex].Buffer.Insert(lineIndex + 1, string.Empty);
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
            doc[cursor.Line] = doc[cursor.Line].Substring(0, cursor.Offset)
                + text
                + (cursor.Offset >= doc[cursor.Line].Length ? ""
                : doc[cursor.Line].Substring(cursor.Offset));
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
                    cursor.Offset = doc[cursor.Line].Length;
                    doc[cursor.Line] += doc[cursor.Line + 1];
                    int partIndex = doc.GetPartIndexByLineIndex(cursor.Line);
                    int lineIndex = cursor.Line;
                    doc.PartBuffer[partIndex].Buffer.RemoveAt(lineIndex + 1);
                }
                return;
            }
            doc[cursor.Line] = doc[cursor.Line].Substring(0, cursor.Offset - 1)
                + (cursor.Offset >= doc[cursor.Line].Length ? ""
                : doc[cursor.Line].Substring(cursor.Offset));
            cursor.TryMoveLeft();
        }
    }
}
