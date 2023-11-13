using EasyUI.Core;
using EasyUI.Core.Components;
using EasyUI.Draw;
using Word.Core;

namespace Word.CustomComponents
{
    public class DocumentViewComponent : IComponent
    {
        private Editor editor = new Editor();
        public Editor Editor => editor;
        private Vector2 documentOffset = Vector2.Zero;

        public Vector2 Position { get; set; } = Vector2.Zero;
        public Vector2 Size { get; set; } = Vector2.Zero;
        public int NumberingStripWidth = 5;

        public Color Foreground { get; set; } = ApplicationCode.Theme.Foreground;
        public Color Background { get; set; } = ApplicationCode.Theme.Background;
        public Color BackgroundDark { get; set; } = ApplicationCode.Theme.BackgroundDark;
        
        public Color NumberingStripForeground { get; set; } = ApplicationCode.Theme.NumberingStripForeground;
        public Color NumberingStripBackground { get; set; } = ApplicationCode.Theme.NumberingStripBackground;

        public Document? Document
        {
            get
            {
                return editor.Document;
            }
            set
            {
                editor.Document = value;
            }
        }

        public void Input(ConsoleKeyInfo keyInfo)
        {
            if (keyInfo.Key == ConsoleKey.UpArrow)
                editor.CursorMoveUp();
            else if (keyInfo.Key == ConsoleKey.DownArrow)
                editor.CursorMoveDown();
            else if (keyInfo.Key == ConsoleKey.LeftArrow)
                editor.CursorMoveLeft();
            else if (keyInfo.Key == ConsoleKey.RightArrow)
                editor.CursorMoveRight();
            else if (keyInfo.Key == ConsoleKey.Home)
                editor.CursorMoveToStartLine();
            else if (keyInfo.Key == ConsoleKey.End)
                editor.CursorMoveToEndLine();
            else if (keyInfo.Key == ConsoleKey.Tab)
                editor.Tab();
            else if (keyInfo.Key == ConsoleKey.Enter)
                editor.NewLine();
            else if (keyInfo.Key == ConsoleKey.Backspace)
                editor.RemoveText();
            else if (keyInfo.Key == ConsoleKey.Escape)
                return;
            else
                editor.InsertText(keyInfo.KeyChar.ToString());
        }

        public void Update()
        {
            if (editor.Cursor.Line < documentOffset.y)
            {
                documentOffset.y--;
            }
            if (editor.Cursor.Line > documentOffset.y + Size.y - 1)
            {
                documentOffset.y++;
            }
            if (editor.Cursor.Offset < documentOffset.x)
            {
                documentOffset.x -= documentOffset.x - editor.Cursor.Offset;
            }
            if (editor.Cursor.Offset != 0 && 
                editor.Cursor.Offset > documentOffset.x + Size.x - NumberingStripWidth - 1)
            {
                documentOffset.x += editor.Cursor.Offset - (documentOffset.x + Size.x - NumberingStripWidth - 1);
            }

            if (ApplicationCode.ThemeChanged)
            {
                Foreground = ApplicationCode.Theme.Foreground;
                Background = ApplicationCode.Theme.Background;
                BackgroundDark = ApplicationCode.Theme.BackgroundDark;
                NumberingStripForeground = ApplicationCode.Theme.NumberingStripForeground;
                NumberingStripBackground = ApplicationCode.Theme.NumberingStripBackground;
            }
        }

        public void Layout()
        {
            
        }

        public void Render(Canvas canvas)
        {
            RenderDocument(canvas);
        }

        public void RenderDocument(Canvas canvas)
        {
            if (Document == null)
            {
                return;
            }
            canvas.DrawFillRect(
                x: Position.x,
                y: Position.y,
                w: NumberingStripWidth,
                h: Size.y,
                color: NumberingStripBackground);
            Canvas documentCanvas = new Canvas(Document.Lines.Max(line => line.Length) + 1, Document.Lines.Count, Background);
            var markupText = Document.GetMarkupText();
            for (int markupTextIndex = 0, linePosition = 0; markupTextIndex < markupText.Count; markupTextIndex++, linePosition++)
            {
                if (markupTextIndex < documentOffset.y) 
                {
                    continue;
                }
                if (markupTextIndex > documentOffset.y + Size.y - 1)
                {
                    break;
                }
                canvas.DrawText(
                    text: (markupTextIndex + 1).ToString(),
                    x: Position.x + NumberingStripWidth / 2 - (markupTextIndex + 1).ToString().Length / 2,
                    y: Position.y + linePosition - documentOffset.y,
                    foreground: NumberingStripForeground,
                    background: NumberingStripBackground,
                    style: PixelStyle.StyleDim);
                for (int markupTextIndexOffset = 0, offsetPosition = 0; 
                    markupTextIndexOffset < markupText[markupTextIndex].Count;
                    offsetPosition += markupText[markupTextIndex][markupTextIndexOffset].Text.Length, markupTextIndexOffset++)
                {
                    documentCanvas.DrawText(
                        text: markupText[markupTextIndex][markupTextIndexOffset].Text,
                        x: offsetPosition,
                        y: linePosition,
                        foreground: markupText[markupTextIndex][markupTextIndexOffset].Color,
                        background: Background,
                        style: markupText[markupTextIndex][markupTextIndexOffset].Style);
                }
                if (markupTextIndex == editor.Cursor.Line)
                {
                    documentCanvas.DrawSymbol(
                        symbol: editor.Cursor.Offset >= Document.Lines[markupTextIndex].Length
                            ? " "
                            : Document.Lines[markupTextIndex][editor.Cursor.Offset].ToString(),
                        x: editor.Cursor.Offset,
                        y: markupTextIndex,
                        foreground: Background,
                        background: Foreground,
                        style: PixelStyle.StyleNone);
                }
            }
            canvas.CanvasToCanvas(
                canvas: documentCanvas,
                offsetCanvasX: documentOffset.x,
                offsetCanvasY: documentOffset.y,
                x: Position.x + NumberingStripWidth,
                y: Position.y,
                w: Size.x - NumberingStripWidth,
                h: Size.y);
        }
    }
}
