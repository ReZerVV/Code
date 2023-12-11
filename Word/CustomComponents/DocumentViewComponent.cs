using EasyUI.Core;
using Word.Core;
using EasyUI.Core.Components;
using EasyUI.Draw;

namespace Word.CustomComponents
{
    public class DocumentViewComponent : IComponent
    {
        public Document Doc { get; set; } = null;
        public DocumentCursor Cursor { get; set; } = null;

        public Vector2 Position { get; set; } = Vector2.Zero;
        public Vector2 Size { get; set; } = Vector2.Zero;
        public int NumberingStripWidth = 5;

        public Color Foreground { get; set; } = AppState.Theme.Foreground;
        public Color Background { get; set; } = AppState.Theme.Background;
        public Color BackgroundDark { get; set; } = AppState.Theme.BackgroundDark;
        
        public Color NumberingStripForeground { get; set; } = AppState.Theme.NumberingStripForeground;
        public Color NumberingStripBackground { get; set; } = AppState.Theme.NumberingStripBackground;

        public void Input(ConsoleKeyInfo keyInfo)
        {
            if (keyInfo.Key == ConsoleKey.UpArrow)
                Cursor.TryMoveUp();
            else if (keyInfo.Key == ConsoleKey.DownArrow)
                Cursor.TryMoveDown();
            else if (keyInfo.Key == ConsoleKey.LeftArrow)
                Cursor.TryMoveLeft();
            else if (keyInfo.Key == ConsoleKey.RightArrow)
                Cursor.TryMoveRight();
            else if (keyInfo.Key == ConsoleKey.Home)
                Cursor.TryMoveToStartLine();
            else if (keyInfo.Key == ConsoleKey.End)
                Cursor.TryMoveToEndLine();
            else if (keyInfo.Key == ConsoleKey.Tab)
                Doc.InsertTab(Cursor);
            else if (keyInfo.Key == ConsoleKey.Enter)
                Doc.InsertNewLine(Cursor);
            else if (keyInfo.Key == ConsoleKey.Backspace)
                Doc.Remove(Cursor);
            else if (keyInfo.Key == ConsoleKey.Escape)
                return;
            else
                Doc.InsertText(keyInfo.KeyChar.ToString(), Cursor);
        }

        public void Update()
        {
            if (AppState.ThemeChanged)
            {
                Foreground = AppState.Theme.Foreground;
                Background = AppState.Theme.Background;
                BackgroundDark = AppState.Theme.BackgroundDark;
                NumberingStripForeground = AppState.Theme.NumberingStripForeground;
                NumberingStripBackground = AppState.Theme.NumberingStripBackground;
            }
        }

        public void Layout()
        {
            Cursor.ScreenWidth = Size.x;
            Cursor.ScreenHeight = Size.y;
        }

        public void Render(Canvas canvas)
        {
            RenderDocument(canvas);
        }

        public void RenderDocument(Canvas canvas)
        {
            if (Doc == null)
            {
                return;
            }
            canvas.DrawFillRect(
                x: Position.x,
                y: Position.y,
                w: NumberingStripWidth,
                h: Size.y,
                color: NumberingStripBackground);

            Canvas documentCanvas = new Canvas(Doc.Buffer.Max(line => line.Length) + 1, Doc.Buffer.Count, Background);
            
            var markupText = Doc.Markup();
            for (int markupTextIndex = 0, linePosition = 0;
                markupTextIndex < markupText.Count;
                markupTextIndex++, linePosition++)
            {
                if (markupTextIndex < Cursor.DocYOffset)
                {
                    continue;
                }
                if (markupTextIndex > Cursor.DocYOffset + Size.y - 1)
                {
                    break;
                }

                // Render numbering bar
                canvas.DrawText(
                    text: (markupTextIndex + Cursor.PartOffset + 1).ToString(),
                    x: Position.x + NumberingStripWidth / 2 - (markupTextIndex + Cursor.PartOffset + 1).ToString().Length / 2,
                    y: Position.y + linePosition - Cursor.DocYOffset,
                    foreground: NumberingStripForeground,
                    background: NumberingStripBackground,
                    style: PixelStyle.StyleDim);

                for (int markupTextIndexOffset = 0, offsetPosition = 0;
                    markupTextIndexOffset < markupText[markupTextIndex].Count;
                    markupTextIndexOffset++)
                {
                    documentCanvas.DrawText(
                        text: markupText[markupTextIndex][markupTextIndexOffset].Value,
                        x: offsetPosition,
                        y: linePosition,
                        foreground: ColorToPixelColor(markupText[markupTextIndex][markupTextIndexOffset].ColorOptions),
                        background: Background,
                        style: StyleToPixelStyle(markupText[markupTextIndex][markupTextIndexOffset].StyleOptions));
                    offsetPosition += markupText[markupTextIndex][markupTextIndexOffset].Value.Length;
                }
                if (markupTextIndex == Cursor.GetLineIndex())
                {
                    documentCanvas.DrawSymbol(
                        symbol: Cursor.Offset >= Doc.Buffer[Cursor.GetLineIndex()].Length
                            ? " "
                            : Doc.Buffer[markupTextIndex][Cursor.Offset].ToString(),
                        x: Cursor.Offset,
                        y: linePosition,
                        foreground: Background,
                        background: Foreground,
                        style: PixelStyle.StyleNone);
                }
            }

            canvas.CanvasToCanvas(
                canvas: documentCanvas,
                offsetCanvasX: Cursor.DocXOffset,
                offsetCanvasY: Cursor.DocYOffset,
                x: Position.x + NumberingStripWidth,
                y: Position.y,
                w: Size.x - NumberingStripWidth,
                h: Size.y);
        }

        public Color ColorToPixelColor(Core.Syntax.Styles.Color color)
            => new EasyUI.Draw.Color(color.R, color.G, color.B);

        public PixelStyle StyleToPixelStyle(Core.Syntax.Styles.Style style)
            => new EasyUI.Draw.PixelStyle
            {
                Bold = style.Bold,
                Dim = style.Dim,
                Italic = style.Italic,
                Underline = style.Underline,
                SlowBlink = style.SlowBlink,
                RapidBlink = style.RapidBlink,
                Overlined = style.Overlined
            };
    }
}
