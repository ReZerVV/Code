using System;
using System.Drawing;
using System.Text;

namespace EasyUI.Draw
{
    public class Canvas
    {
        private Pixel[] viewBuffer;

        public int Width { get; private set; }
        public int Height { get; private set; }

        public Canvas(int width, int height)
        {
            Width = width;
            Height = height;
            viewBuffer = new Pixel[Width * Height];
            for (int indexBuffer = 0; indexBuffer < Width * Height; indexBuffer++)
            {
                viewBuffer[indexBuffer] = new Pixel();
            }
            Console.OutputEncoding = Encoding.UTF8;
            Console.SetBufferSize(width, height);
            Console.SetWindowSize(width, height);
            Console.CursorVisible = false;
        }

        public void RenderBuffer()
        {
            StringBuilder stringBuffer = new StringBuilder();
            for (int indexBuffer = 0; indexBuffer < Width * Height; indexBuffer++)
            {
                if (indexBuffer != 0 &&
                    viewBuffer[indexBuffer].Foreground.Equals(viewBuffer[indexBuffer - 1].Foreground) &&
                    viewBuffer[indexBuffer].Background.Equals(viewBuffer[indexBuffer - 1].Background))
                {
                    stringBuffer.Append(viewBuffer[indexBuffer].Symbol);
                }
                else
                {
                    stringBuffer.Append($"\u001b[48;2;{viewBuffer[indexBuffer].Background.R};{viewBuffer[indexBuffer].Background.G};{viewBuffer[indexBuffer].Background.B}m\x1b[38;2;{viewBuffer[indexBuffer].Foreground.R};{viewBuffer[indexBuffer].Foreground.G};{viewBuffer[indexBuffer].Foreground.B}m{viewBuffer[indexBuffer].Symbol}");
                }
            }
            Console.SetCursorPosition(0, 0);
            Console.Write(stringBuffer.ToString());
        }

        public void ClearBuffer()
        {
            for (int indexBuffer = 0; indexBuffer < Width * Height; indexBuffer++)
            {
                viewBuffer[indexBuffer].Symbol = " ";
                viewBuffer[indexBuffer].Background = new Color(0, 0, 0);
            }
        }

        #region Utils
        private bool IsValidCoords(int x, int y)
        {
            return x >= 0 && x < Width && y >= 0 && y < Height;
        }

        private int CoordsToIndex(int x, int y)
        {
            return y * Width + x;
        }

        private void Swap(ref int x, ref int y)
        {
            int temp = x;
            x = y;
            y = temp;
        }
        #endregion

        #region Draw
        public void DrawPixel(int x, int y, Color color)
        {
            if (IsValidCoords(x, y))
            {
                int indexBuffer = CoordsToIndex(x, y);

                viewBuffer[indexBuffer].Symbol = " ";
                viewBuffer[indexBuffer].Background = color;
            }
        }

        public void DrawLine(int x0, int y0, int x1, int y1, Color color)
        {
            int dx = x1 - x0;
            int dy = y1 - y0;

            int swaps = 0;
            if (dy > dx)
            {
                Swap(ref dx, ref dy);
                swaps = 1;
            }

            int a = Math.Abs(dy);
            int b = -Math.Abs(dx);

            double d = 2 * a + b;
            int x = x0;
            int y = y0;

            int s = 1;
            int q = 1;
            if (x0 > x1) q = -1;
            if (y0 > y1) s = -1;

            DrawPixel(x, y, color);
            for (int k = 0; k < dx; k++)
            {
                if (d >= 0)
                {
                    d = 2 * (a + b) + d;
                    y = y + s;
                    x = x + q;
                }
                else
                {
                    if (swaps == 1) y = y + s;
                    else x = x + q;
                    d = 2 * a + d;
                }

                DrawPixel(x, y, color);
            }
        }

        public void DrawRect(int x, int y, int w, int h, Color color)
        {
            DrawLine(x, y, x + w, y, color);
            DrawLine(x, y, x, y + h, color);
            DrawLine(x + w, y, x + w, y + h, color);
            DrawLine(x, y + h, x + w, y + h, color);
        }

        public void DrawFillRect(int x, int y, int w, int h, Color color)
        {
            for (int indexX = x; indexX < x + w; indexX++)
            {
                for (int indexY = y; indexY < y + h; indexY++)
                {
                    DrawPixel(indexX, indexY, color);
                }
            }
        }

        public void DrawSymbol(string symbol, int x, int y, Color foreground, Color background)
        {
            if (symbol.Length != 1)
            {
                throw new Exception("Invalid symbol");
            }
            if (IsValidCoords(x, y))
            {
                int indexBuffer = CoordsToIndex(x, y);

                viewBuffer[indexBuffer].Symbol = symbol;
                viewBuffer[indexBuffer].Foreground = foreground;
                viewBuffer[indexBuffer].Background = background;
            }
        }

        public void DrawText(string text, int x, int y, Color foreground, Color background)
        {
            for (int indexSymbol = 0; indexSymbol < text.Length; indexSymbol++, x++)
            {
                if (!IsValidCoords(x, y))
                {
                    break;
                }
                DrawSymbol(text[indexSymbol].ToString(), x, y, foreground, background);
            }
        }

        public void DrawRoundedBorder(int x, int y, int w, int h, Color color, Color background)
        {
            DrawSymbol(
                "╭",
                x,
                y,
                color,
                background);
            DrawSymbol(
                "╮",
                x + w - 1,
                y,
                color,
                background);

            for (int indexX = x + 1; indexX < x + w - 1; indexX++)
            {
                DrawSymbol(
                    "─",
                    indexX,
                    y,
                    color,
                    background);

                DrawSymbol(
                    "─",
                    indexX,
                    y + h - 1,
                    color,
                    background);
            }
            for (int indexY = y + 1; indexY < y + h - 1; indexY++)
            {
                DrawSymbol(
                    "│",
                    x,
                    indexY,
                    color,
                    background);
                DrawSymbol(
                    "│",
                    x + w - 1,
                    indexY,
                    color,
                    background);
            }

            DrawSymbol(
                "╰",
                x,
                y + h - 1,
                color,
                background);
            DrawSymbol(
                "╯",
                x + w - 1,
                y + h - 1,
                color,
                background);
        }
        
        public void DrawBorder(int x, int y, int w, int h, Color color, Color background)
        {
            DrawSymbol(
                "┌",
                x,
                y,
                color,
                background);
            DrawSymbol(
                "┐",
                x + w - 1,
                y,
                color,
                background);

            for (int indexX = x + 1; indexX < x + w - 1; indexX++)
            {
                DrawSymbol(
                    "─",
                    indexX,
                    y,
                    color,
                    background);

                DrawSymbol(
                    "─",
                    indexX,
                    y + h - 1,
                    color,
                    background);
            }
            for (int indexY = y + 1; indexY < y + h - 1; indexY++)
            {
                DrawSymbol(
                    "│",
                    x,
                    indexY,
                    color,
                    background);
                DrawSymbol(
                    "│",
                    x + w - 1,
                    indexY,
                    color,
                    background);
            }

            DrawSymbol(
                "└",
                x,
                y + h - 1,
                color,
                background);
            DrawSymbol(
                "┘",
                x + w - 1,
                y + h - 1,
                color,
                background);
        }

        public void DrawUnicodeVerticalLine(int x, int y, int h, Color color, Color background)
        {
            for (int indexY = y; indexY < y + h; indexY++) 
            {
                DrawSymbol(
                    "│",
                    x,
                    indexY,
                    color,
                    background);
            }
        }

        public void DrawUnicodeHorizontalLine(int x, int y, int w, Color color, Color background)
        {
            for (int indexX = x; indexX < x + w; indexX++)
            {
                DrawSymbol(
                    "─",
                    indexX,
                    y,
                    color,
                    background);
            }
        }
        #endregion
    }
}
