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

        public Color Foreground { get; set; } = AppState.Theme.Foreground;
        public Color Background { get; set; } = AppState.Theme.Background;
        public Color BackgroundDark { get; set; } = AppState.Theme.BackgroundDark;

        public void Input(ConsoleKeyInfo keyInfo)
        {
            if ((keyInfo.Modifiers & ConsoleModifiers.Control) != 0 &&
                keyInfo.Key == ConsoleKey.End)
                AppState.DocumentStore.MoveToNextDoc();
            if ((keyInfo.Modifiers & ConsoleModifiers.Control) != 0 &&
                keyInfo.Key == ConsoleKey.Home)
                AppState.DocumentStore.MoveToPrevDoc();
            else
                documentVeiwer.Input(keyInfo);
        }

        public void Update()
        {
            if (AppState.ThemeChanged)
            {
                Foreground = AppState.Theme.Foreground;
                Background = AppState.Theme.Background;
                BackgroundDark = AppState.Theme.BackgroundDark;
            }

            if (AppState.DocumentStore.IsDocChanged ||
                documentVeiwer.Doc == null)
            {
                documentVeiwer.Doc = AppState.DocumentStore.CurrentDoc;
                documentVeiwer.Cursor = AppState.DocumentStore.Cursor;
                AppState.DocumentStore.IsDocChanged = false;
            }
            documentVeiwer.Update();
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
            if (documentVeiwer.Doc == null)
            {
                return;
            }
            
            canvas.DrawFillRect(
                x: Position.x,
                y: Position.y,
                w: Size.x,
                h: TabBarSize.y,
                color: BackgroundDark);

            for (int documentIndex = 0, xOffset = 0; documentIndex < AppState.DocumentStore.Docs.Count; documentIndex++, xOffset += TabBarSize.x)
            {
                if (documentIndex == AppState.DocumentStore.CurrentIndex)
                {
                    canvas.DrawFillRect(
                        x: Position.x + xOffset,
                        y: Position.y,
                        w: TabBarSize.x,
                        h: TabBarSize.y,
                        color: Background);
                    canvas.DrawText(
                        text: AppState.DocumentStore.Docs[documentIndex].GetName().Length < TabBarSize.x - 2 
                            ? AppState.DocumentStore.Docs[documentIndex].GetName()
                            : AppState.DocumentStore.Docs[documentIndex].GetName().Substring(0, TabBarSize.x - 5) + "...",
                        x: Position.x + xOffset + 1,
                        y: Position.y,
                        foreground: Foreground,
                        background: Background,
                        style: AppState.DocumentStore.Docs[documentIndex].IsSaved
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
                    text: AppState.DocumentStore.Docs[documentIndex].GetName().Length < TabBarSize.x - 2
                        ? AppState.DocumentStore.Docs[documentIndex].GetName()
                        : AppState.DocumentStore.Docs[documentIndex].GetName().Substring(0, TabBarSize.x - 5) + "...",
                    x: Position.x + xOffset + 1,
                    y: Position.y,
                    foreground: Foreground,
                    background: BackgroundDark,
                    style: AppState.DocumentStore.Docs[documentIndex].IsSaved
                            ? PixelStyle.StyleDim
                            : new PixelStyle { Dim = true, Italic = true });
            }
        }
    }
}
