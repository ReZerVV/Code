namespace Code.Core
{
    public class DocumentPart
    {
        public int BeginPartPosition { get; set; }
        public int SizePart { get; set; }
        public List<string> Buffer { get; set; }

        public DocumentPart(int beginPartPosition = 0, int sizePart = 0)
        {
            BeginPartPosition = beginPartPosition;
            SizePart = sizePart;
            Buffer = new List<string>();
        }
    }
}
