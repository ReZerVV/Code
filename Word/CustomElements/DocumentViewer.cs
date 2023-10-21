using EasyUI.Core;
using EasyUI.Draw;
using Word.Core;

namespace Word.CustomElements
{
    public class DocumentViewer
    {
        private List<string> documentLines { get; set; } = new List<string>();
        private Canvas documentCanvas { get; set; }
        private Vector2 documentCanvasOffset = Vector2.Zero;
        private Vector2 pageSize = new Vector2(50, 40);
        private float pageScale = 1.0f;

        public Vector2 Position { get; set; } = Vector2.Zero;
        public Vector2 Size { get; set; } = Vector2.Zero;
        public Document? Document { get; set; } = null;
        public Vector2 PageSize => pageSize * pageScale;
        public Color Foreground { get; set; } = Color.White;
        public Color Background { get; set; } = Color.Black;
        
        #region Cursor
        public Vector2 CursorPosition { get; set; } = Vector2.Zero;
        
        private void CursorMoveLeft()
        {
            if (CursorPosition.x - 1 >= 0)
            {
                CursorPosition.x--;
            }
            else if (CursorMoveUp())
            {
                CursorPosition.x = documentLines[CursorPosition.y].Length;
            }
        }

        private void CursorMoveRight()
        {
            if (CursorPosition.x + 1 <= documentLines[CursorPosition.y].Length)
            {
                CursorPosition.x++;
            }
            else if (CursorMoveDown())
            {
                CursorPosition.x = 0;
            }
        }

        private bool CursorMoveUp()
        {
            if (CursorPosition.y > 0)
            {
                CursorPosition.y--;
                if (CursorPosition.x > documentLines[CursorPosition.y].Length)
                {
                    CursorPosition.x = documentLines[CursorPosition.y].Length;
                }
                return true;
            }
            return false;
        }

        private bool CursorMoveDown()
        {
            if (CursorPosition.y + 1 < documentLines.Count)
            {
                CursorPosition.y++;
                if (CursorPosition.x > documentLines[CursorPosition.y].Length)
                {
                    CursorPosition.x = documentLines[CursorPosition.y].Length;
                }
                return true;
            }
            return false;
        }
        #endregion

        #region Document
        private List<string> DocumentToStringLines(Document document)
        {
            List<string> documentLines = new List<string>();
            int lineIndex = -1;
            foreach (DocumentNode documentNode in document)
            {
                switch (documentNode.Type)
                {
                    case DocumentNodeType.Paragraph:
                        {
                            documentLines.Add(string.Empty);
                            lineIndex += 1;
                        }
                        break;

                    case DocumentNodeType.Span:
                        {
                            foreach (string word in documentNode.InnerText.Split(' '))
                            {
                                if (documentLines[lineIndex].Length + word.Length >= PageSize.x)
                                {
                                    lineIndex++;
                                    documentLines.Add(string.Empty);
                                }
                                documentLines[lineIndex] += word + " ";
                            }
                        }
                        break;
                }
            }
            return documentLines;
        }
        #endregion

        public void Update()
        {
            if (Document != null)
            {
                documentLines = DocumentToStringLines(Document);
            }
        }

        public void Input(ConsoleKeyInfo keyInfo)
        {
            //if (keyInfo.Key == ConsoleKey.OemPlus)
            //{
            //    pageScale += 0.2f;
            //}
            //if (keyInfo.Key == ConsoleKey.OemMinus)
            //{
            //    pageScale -= 0.2f;
            //}
            if (keyInfo.Key == ConsoleKey.LeftArrow)
                CursorMoveLeft();
            else if (keyInfo.Key == ConsoleKey.RightArrow)
                CursorMoveRight();
            else if (keyInfo.Key == ConsoleKey.UpArrow)
                CursorMoveUp();
            else if (keyInfo.Key == ConsoleKey.DownArrow)
                CursorMoveDown();
        }

        public void Layout()
        {
            
        }

        public void Render(Canvas canvas)
        {
            // Draw message
            if (Document == null) 
            {
                string message = "Document not found to view. ";
                canvas.DrawText(
                    text: message,
                    x: Position.x + (Size.x - message.Length) / 2,
                    y: Position.y + (Size.y - 1) / 2,
                    foreground: Foreground,
                    background: Background);
                return;
            }

            int linePosition = 0;
            documentCanvas = new Canvas(Size.x, Size.y, Color.Black);
            for (int lineIndex = 0; lineIndex < documentLines.Count; lineIndex++)
            {
                if (linePosition % PageSize.y == PageSize.y - 4)
                {
                    linePosition += 4;
                }
                if (linePosition % PageSize.y == 0)
                {
                    documentCanvas.DrawBorder(
                        x: (documentCanvas.Width - PageSize.x) / 2 - 4,
                        y: linePosition - documentCanvasOffset.y,
                        w: PageSize.x + 8,
                        h: PageSize.y,
                        color: Foreground,
                        background: Background);
                    linePosition += 4;
                }
                documentCanvas.DrawText(
                    text: documentLines[lineIndex],
                    x: (documentCanvas.Width - PageSize.x) / 2 - documentCanvasOffset.x,
                    y: linePosition - documentCanvasOffset.y,
                    foreground: Foreground,
                    background: Background);
                if (lineIndex == CursorPosition.y)
                {
                    if (linePosition > documentCanvasOffset.y + Size.y - 4)
                    {
                        documentCanvasOffset.y++;
                    }
                    if (linePosition < documentCanvasOffset.y + 4)
                    {
                        documentCanvasOffset.y--;
                    }
                    documentCanvas.DrawSymbol(
                        symbol: CursorPosition.x == documentLines[lineIndex].Length ||
                            documentLines[lineIndex] == string.Empty ? " " : documentLines[lineIndex][CursorPosition.x].ToString(),
                        x: (documentCanvas.Width - PageSize.x) / 2 + CursorPosition.x - documentCanvasOffset.x,
                        y: linePosition - documentCanvasOffset.y,
                        foreground: Background,
                        background: Foreground);
                }
                linePosition++;
            }
            canvas.CanvasToCanvas(
                documentCanvas,
                0,
                0,
                Position.x,
                Position.y,
                Size.x,
                Size.y);
        }
    }
}
