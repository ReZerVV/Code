namespace Word.Stores
{
    public enum NotificationType
    {
        Info = 0,
        Error = 1,
    }

    public class Notification
    {
        public NotificationType Type { get; set; }
        public string Message { get; set; }

        public Notification(NotificationType type, string message)
        {
            Type = type;
            Message = message;
        }

        public static Notification Info(string message)
        {
            return new Notification(NotificationType.Info, message);
        }

        public static Notification Error(string message)
        {
            return new Notification(NotificationType.Error, message);
        }
    }

    public class NotificationStore
    {
        public Queue<Notification> Notifications { get; set; } = new();

        public void Send(Notification notification)
        {
            Notifications.Enqueue(notification);
        }
    }
}
