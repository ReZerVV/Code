using System.Drawing;

namespace Word.Core
{
    public class DocumentCursor
    {
        private Document doc;
        public Document Doc
        {
            get
            {
                return doc;
            }
            set
            {
                doc = value;
                Offset = 0;
                Line = 0;
            }
        }

        public int DocXOffset { get; set; }
        public int DocYOffset { get; set; }

        public int ScreenWidth { get; set; }
        public int ScreenHeight { get; set; }

        public int PartOffset { get; set; }
        public readonly int PartSize;

        public int Offset { get; set; }
        public int Line { get; set; }

        public DocumentCursor(Document doc, int partSize = 10)
        {
            Doc = doc;
            
            Offset = 0;
            Line = 0;
            
            DocXOffset = 0;
            DocYOffset = 0;

            ScreenWidth = 0;
            ScreenHeight = 0;
            
            PartOffset = 0;
            PartSize = partSize;
        }

        public bool TryMoveLeft(int step = 1)
        {
            if (Doc == null)
            {
                return false;
            }
            if (Offset - step >= 0)
            {
                Offset -= step;
                if (Offset != 0 &&
                    Offset > DocXOffset + ScreenWidth - 5 - 1)
                {
                    DocXOffset += Offset - (DocXOffset + ScreenWidth - 5 - 1);
                }
                return true;
            }
            else if (TryMoveUp())
            {
                Offset = Doc[Line].Length;
                return true;
            }
            return false;
        }

        public bool TryMoveRight(int step = 1)
        {
            if (Doc == null)
            {
                return false;
            }
            if (Offset + step <= Doc[Line].Length)
            {
                Offset += step;
                if (Offset < DocXOffset)
                {
                    DocXOffset -= DocXOffset - Offset;
                }
                return true;
            }
            else if (TryMoveDown())
            {
                Offset = 0;
                return true;
            }
            return false;
        }

        public bool TryMoveUp(int step = 1)
        {
            if (Doc == null)
            {
                return false;
            }
            if (Line - step >= 0)
            {
                Line -= step;
                if (Offset > Doc[Line].Length)
                {
                    Offset = Doc[Line].Length;
                }
                return true;
            }
            return false;
        }

        public bool TryMoveDown(int step = 1)
        {
            if (Doc == null)
            {
                return false;
            }
            if (Line + step < Doc.GetDocumentSize())
            {
                Line += step;
                if (Offset > Doc[Line].Length)
                {
                    Offset = Doc[Line].Length;
                }
                return true;
            }
            return false;
        }

        public bool TryMoveToStartLine()
        {
            if (Doc == null)
            {
                return false;
            }
            Offset = 0;
            return true;
        }

        public bool TryMoveToEndLine()
        {
            if (Doc == null)
            {
                return false;
            }
            Offset = doc[Line].Length;
            return true;
        }
    }
}
