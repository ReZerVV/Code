using EasyUI.Core;
using EasyUI.Draw;
using Word.Core;

namespace Word.CustomElements
{
    public class DocumentEditor
    {
        public const int PAGE_WIDTH_SIZE = 80;
        public const int PAGE_HEIGHT_SIZE = 40;

        private Document Document { get; set; } = new Document("New document");
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
            { // Render Menubar "◐◓◑◒"
                canvas.DrawText(
                    $"│{Document.Name}│",
                    Position.x,
                    Position.y,
                    Foreground,
                    Background);
            }
            {
                //canvas.DrawSymbol(
                //    "▼",
                //    (canvas.Width - PAGE_WIDTH_SIZE) / 2,
                //    Position.y + 4,
                //    Foreground,
                //    Background);
                //canvas.DrawSymbol(
                //    "▼",
                //    (canvas.Width - PAGE_WIDTH_SIZE) / 2 + PAGE_WIDTH_SIZE - 1,
                //    Position.y + 4,
                //    Foreground,
                //    Background);
                //canvas.DrawUnicodeHorizontalLine(
                //    (canvas.Width - PAGE_WIDTH_SIZE) / 2,
                //    Position.y + 5,
                //    PAGE_WIDTH_SIZE,
                //    Foreground,
                //    Background);

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

            //documentCanvas.DrawSymbol(
            //    " ",
            //    cursorPosition.x,
            //    cursorPosition.y,
            //    Background,
            //    Foreground);

            documentCanvas.DrawFillRect(50,50,50,50,Color.Red);

            canvas.CanvasToCanvas(
                documentCanvas,
                0,
                cursorPosition.y,
                (canvas.Width - PAGE_WIDTH_SIZE) / 2,
                (canvas.Height - PAGE_HEIGHT_SIZE) / 2,
                PAGE_WIDTH_SIZE,
                PAGE_HEIGHT_SIZE);

            canvas.DrawBorder(
                (canvas.Width - PAGE_WIDTH_SIZE) / 2,
                (canvas.Height - PAGE_HEIGHT_SIZE) / 2,
                PAGE_WIDTH_SIZE,
                PAGE_HEIGHT_SIZE,
                Foreground,
                Background);
        }
    }
}
