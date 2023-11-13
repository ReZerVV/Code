using EasyUI.Core;
using EasyUI.Core.Components;
using EasyUI.Draw;

namespace Word.CustomComponents
{
    public class DocumentTabComponent : IComponent
    {
        public DocumentViewComponent documentVeiwer = new();

        public Vector2 TabBarSize { get; set; } = new(15, 1);

        private Vector2 position = Vector2.Zero;
        public Vector2 Position 
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
                documentVeiwer.Position = new Vector2(Position.x, Position.y + TabBarSize.y);
            }
        }

        public Vector2 size = Vector2.Zero;
        public Vector2 Size 
        {
            get
            {
                return size;
            }
            set 
            {
                size = value;
                documentVeiwer.Size = new Vector2(Size.x, Size.y - TabBarSize.y);
            }
        }

        public Color Foreground { get; set; } = ApplicationCode.Theme.Foreground;
        public Color Background { get; set; } = ApplicationCode.Theme.Background;
        public Color BackgroundDark { get; set; } = ApplicationCode.Theme.BackgroundDark;

        public void Input(ConsoleKeyInfo keyInfo)
        {
            if ((keyInfo.Modifiers & ConsoleModifiers.Control) != 0 &&
                keyInfo.Key == ConsoleKey.End)
                ApplicationCode.DocumentStore.MoveToNextDocument();
            if ((keyInfo.Modifiers & ConsoleModifiers.Control) != 0 &&
                keyInfo.Key == ConsoleKey.Home)
                ApplicationCode.DocumentStore.MoveToPrevDocument();
            else
                documentVeiwer.Input(keyInfo);
        }

        public void Update()
        {
            if (ApplicationCode.DocumentStore.GetCurrentDocument() != documentVeiwer.Document)
            {
                documentVeiwer.Document = ApplicationCode.DocumentStore.GetCurrentDocument();
            }
            documentVeiwer.Update();

            if (ApplicationCode.ThemeChanged)
            {
                Foreground = ApplicationCode.Theme.Foreground;
                Background = ApplicationCode.Theme.Background;
                BackgroundDark = ApplicationCode.Theme.BackgroundDark;
            }
        }

        public void Layout()
        {
            documentVeiwer.Layout();
        }

        public void Render(Canvas canvas)
        {
            RenderTabBar(canvas);
            documentVeiwer.Render(canvas);
        }

        private void RenderTabBar(Canvas canvas)
        {
            if (ApplicationCode.DocumentStore.GetCurrentDocument() == null)
            {
                return;
            }

            canvas.DrawFillRect(
                x: Position.x,
                y: Position.y,
                w: Size.x,
                h: TabBarSize.y,
                color: BackgroundDark);
            for (int documentIndex = 0, xOffset = 0; documentIndex < ApplicationCode.DocumentStore.Documents.Count; documentIndex++, xOffset += TabBarSize.x)
            {
                if (documentIndex == ApplicationCode.DocumentStore.CurrentIndex)
                {
                    canvas.DrawFillRect(
                        x: Position.x + xOffset,
                        y: Position.y,
                        w: TabBarSize.x,
                        h: TabBarSize.y,
                        color: Background);
                    canvas.DrawText(
                        text: ApplicationCode.DocumentStore.Documents[documentIndex].Name.Length < TabBarSize.x - 2 
                            ? ApplicationCode.DocumentStore.Documents[documentIndex].Name
                            : ApplicationCode.DocumentStore.Documents[documentIndex].Name.Substring(0, TabBarSize.x - 5) + "...",
                        x: Position.x + xOffset + 1,
                        y: Position.y,
                        foreground: Foreground,
                        background: Background,
                        style: ApplicationCode.DocumentStore.Documents[documentIndex].IsSaved
                            ? PixelStyle.StyleNone
                            : PixelStyle.StyleItalic);
                    continue;
                }
                canvas.DrawFillRect(
                    x: Position.x + xOffset,
                    y: Position.y,
                    w: TabBarSize.x,
                    h: TabBarSize.y,
                    color: BackgroundDark);
                canvas.DrawText(
                    text: ApplicationCode.DocumentStore.Documents[documentIndex].Name.Length < TabBarSize.x - 2
                        ? ApplicationCode.DocumentStore.Documents[documentIndex].Name
                        : ApplicationCode.DocumentStore.Documents[documentIndex].Name.Substring(0, TabBarSize.x - 5) + "...",
                    x: Position.x + xOffset + 1,
                    y: Position.y,
                    foreground: Foreground,
                    background: BackgroundDark,
                    style: ApplicationCode.DocumentStore.Documents[documentIndex].IsSaved
                            ? PixelStyle.StyleDim
                            : new PixelStyle { Dim = true, Italic = true });
            }
        }
    }
}
