using EasyUI.Core;
using EasyUI.Draw;
using Word.Core;

namespace Word.CustomElements
{
    public class DocumentEditor
    {
        public const int PAGE_WIDTH_SIZE = 80;
        public const int PAGE_HEIGHT_SIZE = 40;

        private Document Document { get; set; } = Document.SampleDocument();
        private string Text { get; set; }
        private Vector2 cursorPosition { get; set; } = Vector2.Zero;
        private Canvas documentCanvas { get; set; } = new Canvas(PAGE_WIDTH_SIZE, 200, Color.Black);

        public Vector2 Position { get; set; } = Vector2.Zero;
        public Vector2 Size { get; set; } = Vector2.Zero;
        public Color Background { get; set; } = Color.Black;
        public Color Foreground { get; set; } = Color.White;
        public Color PageColor => documentCanvas.ClearColor;
        public bool IsFocus { get; set; }

        private void RemoveSymbol()
        {
            if (cursorPosition.x > 0)
            {
                Text = Text.Substring(0, cursorPosition.x - 1)
                    + (cursorPosition.x == Text.Length ? "" : Text.Substring(cursorPosition.x));
                CursorMoveLeft();
            }
        }

        private void InsertSymbol(char symbol)
        {
            Text = Text.Substring(0, cursorPosition.x)
                + symbol
                + (cursorPosition.x == Text.Length ? "" : Text.Substring(cursorPosition.x));
            CursorMoveRight();
        }

        private void CursorMoveLeft()
        {
            //if (cursorPosition.x - 1 >= 0)
            //    cursorPosition.x--;
            cursorPosition.x--;
        }

        private void CursorMoveRight()
        {
            //if (cursorPosition.x + 1 <= Text.Length)
            //    cursorPosition.x++;
            cursorPosition.x++;
        }

        private void CursorMoveUp()
        {
            if (cursorPosition.y > 0)
                cursorPosition.y--;
        }

        private void CursorMoveDown()
        {
            cursorPosition.y++;
        }

        public void Update(ConsoleKeyInfo keyInfo)
        {
            if (IsFocus)
            {
                //if (keyInfo.Key == ConsoleKey.Backspace)
                //    RemoveSymbol();
                //else if (keyInfo.Key == ConsoleKey.LeftArrow)
                //    CursorLeft();
                //else if (keyInfo.Key == ConsoleKey.RightArrow)
                //    CursorRight();
                //else
                //    InsertSymbol(keyInfo.KeyChar);
                if (keyInfo.Key == ConsoleKey.LeftArrow)
                    CursorMoveLeft();
                else if (keyInfo.Key == ConsoleKey.RightArrow)
                    CursorMoveRight();
                else if (keyInfo.Key == ConsoleKey.UpArrow)
                    CursorMoveUp();
                else if (keyInfo.Key == ConsoleKey.DownArrow)
                    CursorMoveDown();
            }
        }

        public void Layout()
        {

        }

        public void Render(Canvas canvas)
        {
            {
                

                //canvas.DrawUnicodeHorizontalLine(
                //    (canvas.Width - PAGE_WIDTH_SIZE) / 2,
                //    Position.y + 10,
                //    PAGE_WIDTH_SIZE,
                //    Foreground,
                //    Background);
                //canvas.DrawSymbol(
                //    "▲",
                //    (canvas.Width - PAGE_WIDTH_SIZE) / 2,
                //    Position.y + 11,
                //    Foreground,
                //    Background);
                //canvas.DrawSymbol(
                //    "▲",
                //    (canvas.Width - PAGE_WIDTH_SIZE) / 2 + PAGE_WIDTH_SIZE - 1,
                //    Position.y + 11,
                //    Foreground,
                //    Background);
            }
            documentCanvas.ClearBuffer();

            Queue<DocumentNode> documentNodeQueue = new Queue<DocumentNode>();
            documentNodeQueue.Enqueue(Document.Body);
            int rowPosition = 0;
            while (documentNodeQueue.TryDequeue(out DocumentNode documentNode))
            {
                if (documentNode.Type == DocumentNodeType.Span)
                {
                    int colPosition = 4;
                    for (int index = 0; index < documentNode.InnerText.Length; index += documentCanvas.Width - 9, rowPosition++)
                    {
                        if (rowPosition % PAGE_HEIGHT_SIZE == 0)
                        {
                            documentCanvas.DrawBorder(
                                3, 
                                rowPosition,
                                PAGE_WIDTH_SIZE - 7,
                                PAGE_HEIGHT_SIZE,
                                Foreground,
                                Background);
                            rowPosition++;
                        }

                        documentCanvas.DrawText(
                           text: rowPosition.ToString(),
                           x: 0,
                           y: rowPosition,
                           foreground: Foreground,
                           background: Background);
                        documentCanvas.DrawText(
                            text: documentNode.InnerText.Substring(index, Math.Min(documentCanvas.Width - 9, documentNode.InnerText.Length - index)),
                            x: colPosition,
                            y: rowPosition,
                            foreground: Foreground,
                            background: Background);
                    }
                }
                foreach (DocumentNode documentChildNode in documentNode.ChildNodes) 
                {
                    documentNodeQueue.Enqueue(documentChildNode);
                }
                if (documentNode.NextSibling != null)
                {
                    documentNodeQueue.Enqueue(documentNode.NextSibling);
                }
            }

            documentCanvas.DrawSymbol(
                " ",
                cursorPosition.x,
                cursorPosition.y,
                Background,
                Foreground);

            canvas.CanvasToCanvas(
                documentCanvas,
                0,
                cursorPosition.y - (cursorPosition.y % PAGE_HEIGHT_SIZE),
                (canvas.Width - PAGE_WIDTH_SIZE) / 2,
                6,
                PAGE_WIDTH_SIZE,
                PAGE_HEIGHT_SIZE);

            //canvas.DrawBorder(
            //    (canvas.Width - PAGE_WIDTH_SIZE) / 2,
            //    (canvas.Height - PAGE_HEIGHT_SIZE) / 2,
            //    PAGE_WIDTH_SIZE,
            //    PAGE_HEIGHT_SIZE,
            //    Foreground,
            //    Background);

            canvas.DrawSymbol(
                    "▼",
                    (canvas.Width - PAGE_WIDTH_SIZE) / 2 + 4,
                    Position.y + 4,
                    Foreground,
                    Background);
            canvas.DrawSymbol(
                "▼",
                (canvas.Width - PAGE_WIDTH_SIZE) / 2 + PAGE_WIDTH_SIZE - 6,
                Position.y + 4,
                Foreground,
                Background);
            canvas.DrawUnicodeHorizontalLine(
                (canvas.Width - PAGE_WIDTH_SIZE - 10) / 2,
                Position.y + 5,
                PAGE_WIDTH_SIZE + 10,
                Foreground,
                Background);
        }
    }
}
