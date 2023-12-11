using EasyUI.Components;
using EasyUI.Core;
using EasyUI.Core.Components;
using EasyUI.Draw;
using Word.Stores;

namespace Word.CustomComponents
{
    public class StatusBarComponent : IComponent
    {
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
                notificationComponent.Position = new(Position.x + 2, Position.y);
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
            }
        }

        public Color Foreground { get; set; } = AppState.Theme.StatusBarForeground;
        public Color Background { get; set; } = AppState.Theme.StatusBarBackground;

        private Vector2? cursorPosition = null;
        private int? spaces = null;
        private string? syntaxTypeDocument = null;
        private string? encodingDocument = null;

        private Notification? currentNotification = null;
        public int NotificationDelay { get; set; } = 200;
        private int notificationDelayIndex = 0;
        private AnimationComponent notificationComponent = new()
        {
            Frames = new() { "⚬", "○", "◯", "○" },
            Delay = 10,
        };

        public void Input(ConsoleKeyInfo keyInfo)
        {
            notificationComponent.Input(keyInfo);
        }

        public void Update()
        {
            if (AppState.ThemeChanged)
            {
                Foreground = AppState.Theme.StatusBarForeground;
                Background = AppState.Theme.StatusBarBackground;
            }

            if (AppState.DocumentStore.CurrentDoc != null)
            {
                var documentTabComponent = AppState.NavigationService.CurrentView as DocumentTabComponent;
                if (documentTabComponent != null)
                {
                    cursorPosition = new Vector2(
                        AppState.DocumentStore.Cursor.Offset,
                        AppState.DocumentStore.Cursor.Line + 1);
                    spaces = 4;
                }
                syntaxTypeDocument = AppState.DocumentStore.CurrentDoc.Marker.LangName;
                encodingDocument = $"{AppState.DocumentStore.CurrentDoc.Encoding.EncodingName} ({AppState.DocumentStore.CurrentDoc.Encoding.CodePage})";
            }
            else 
            {
                cursorPosition = null;
                spaces = null;
                syntaxTypeDocument = null;
                encodingDocument = null;
            }
            if (AppState.NotificationStore != null)
            {
                notificationDelayIndex++;
                if (notificationDelayIndex % NotificationDelay == 0)
                {
                    AppState.NotificationStore.Notifications.TryDequeue(out Notification? notification);
                    currentNotification = notification;
                }
                notificationComponent.Update();
            }
            else
            {
                currentNotification = null;
            }
            if (currentNotification != null &&
                currentNotification.Type == NotificationType.Info)
            {
                notificationComponent.Foreground = Foreground;
            }
            else if (currentNotification != null &&
                     currentNotification.Type == NotificationType.Error)
            {
                notificationComponent.Foreground = new Color(255, 125, 125);
            }
        }

        public void Layout()
        {
            notificationComponent.Layout();
        }

        public void Render(Canvas canvas)
        {
            canvas.DrawFillRect(
                x: Position.x,
                y: Position.y,
                w: Size.x,
                h: Size.y,
                color: Background);
            string statusStr = string.Empty;
            if (cursorPosition != null)
            {
                statusStr += $"Ln {cursorPosition.y}, Col {cursorPosition.x}";
            }
            if (spaces != null)
            {
                statusStr += $"  Spaces: {spaces}";
            }
            if (syntaxTypeDocument != null)
            {
                statusStr += $"  {syntaxTypeDocument}";
            }
            if (encodingDocument != null)
            {
                statusStr += $"  {encodingDocument}";
            }
            canvas.DrawText(
                text: statusStr,
                x: Position.x + Size.x - (statusStr.Length + 2),
                y: Position.y,
                foreground: Foreground,
                background: Background,
                style: PixelStyle.StyleNone);

            if (currentNotification != null)
            {
                notificationComponent.Render(canvas);
                string notificationStr = $"{currentNotification.Message}";
                canvas.DrawText(
                    text: notificationStr,
                    x: Position.x + 4,
                    y: Position.y,
                    foreground: Foreground,
                    background: Background,
                    style: PixelStyle.StyleItalic);
            }
        }
    }
}
