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

        public Color Foreground { get; set; } = ApplicationCode.Theme.StatusBarForeground;
        public Color Background { get; set; } = ApplicationCode.Theme.StatusBarBackground;

        private Vector2? cursorPosition = null;
        private int? spaces = null;
        private string? syntaxTypeDocument = null;

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
            if (ApplicationCode.DocumentStore.Current != null)
            {
                var documentTabComponent = ApplicationCode.NavigationService.CurrentView as DocumentTabComponent;
                if (documentTabComponent != null)
                {
                    cursorPosition = new Vector2(
                        documentTabComponent.documentVeiwer.Editor.Cursor.Offset,
                        documentTabComponent.documentVeiwer.Editor.Cursor.Line);
                    spaces = documentTabComponent.documentVeiwer.Editor.TabSize;
                }
                syntaxTypeDocument = ApplicationCode.DocumentStore.Current.Highlighter.LangName;
            }
            else 
            {
                cursorPosition = null;
                spaces = null;
                syntaxTypeDocument = null;
            }
            if (ApplicationCode.NotificationStore != null)
            {
                notificationDelayIndex++;
                if (notificationDelayIndex % NotificationDelay == 0)
                {
                    ApplicationCode.NotificationStore.Notifications.TryDequeue(out Notification? notification);
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

            if (ApplicationCode.ThemeChanged)
            {
                Foreground = ApplicationCode.Theme.StatusBarForeground;
                Background = ApplicationCode.Theme.StatusBarBackground;
                notificationComponent.Foreground = ApplicationCode.Theme.StatusBarForeground;
                notificationComponent.Background = ApplicationCode.Theme.StatusBarBackground;
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
