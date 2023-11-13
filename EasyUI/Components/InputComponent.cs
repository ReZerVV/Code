using EasyUI.Core;
using EasyUI.Core.Components;
using EasyUI.Draw;

namespace EasyUI.Components
{
    public class InputComponent : IComponent
    {
        public int Cursor { get; set; } = 0;
        private int offset = 0;
        public string Text { get; set; } = string.Empty;
        public Color Foreground { get; set; } = Color.White;
        public Color Background { get; set; } = Color.Black;
        public Vector2 Position { get; set; } = Vector2.Zero;
        public Vector2 Size { get; set; } = Vector2.Zero;
        public bool IsChanged { get; set; } = false;
        public PixelStyle TextStyle { get; set; } = PixelStyle.StyleNone;

        public void CursorMoveLeft()
        {
            if (Cursor - 1 >= 0)
            {
                Cursor--;
            }
        }

        public void CursorMoveRight()
        {
            if (Cursor + 1 <= Text.Length)
            {
                Cursor++;
            }
        }

        public void RemoveText()
        {
            if (Cursor == 0) 
            {
                return;
            }
            Text = Text.Substring(0, Cursor - 1)
                + (Cursor == Text.Length ? "" : Text.Substring(Cursor));
            Cursor--;
        }

        public void InsertText(string text)
        {
            Text = Text.Substring(0, Cursor)
                + text
                + (Cursor == Text.Length ? "" : Text.Substring(Cursor));
            Cursor++;
        }

        public void Input(ConsoleKeyInfo keyInfo)
        {
            if (!IsChanged)
                return;
            else if (keyInfo.Key == ConsoleKey.LeftArrow)
                CursorMoveLeft();
            else if (keyInfo.Key == ConsoleKey.RightArrow)
                CursorMoveRight();
            else if (keyInfo.Key == ConsoleKey.Backspace)
                RemoveText();
            else if (keyInfo.Key == ConsoleKey.Enter)
                return;
            else
                InsertText(keyInfo.KeyChar.ToString());
        }

        public void Update()
        {
            if (Cursor < offset)
            {
                offset--;
            }
            if (Cursor > offset + Size.x)
            {
                offset++;
            }
            if (Text.Length >= Size.x && offset + Size.x >= Text.Length) 
            {
                offset = Text.Length - Size.x;
            }
        }

        public void Render(Canvas canvas)
        {
            canvas.DrawLine(
                x0: Position.x,
                y0: Position.y,
                x1: Position.x + Size.x,
                y1: Position.y,
                color: Background);

            canvas.DrawText(
                text: Text.Length > Size.x ? Text.Substring(offset, Size.x) : Text,
                x: Position.x,
                y: Position.y,
                foreground: Foreground,
                background: Background,
                style: TextStyle);
            if (IsChanged)
            {
                canvas.DrawSymbol(
                    symbol: Cursor == Text.Length ? " " : Text[Cursor].ToString(),
                    x: Position.x + (Cursor - offset),
                    y: Position.y,
                    foreground: Background,
                    background: Foreground,
                    style: PixelStyle.StyleNone);
            }
        }

        public void Layout()
        {
        }
    }
}
