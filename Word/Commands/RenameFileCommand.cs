using Word.Stores;

namespace Word.Commands
{
    public class RenameFileCommand : ICommand
    {
        public string Name => "Rename File";
        public bool HasArgs => true;
        public int CountArgs => 2;
        public List<string> Args { get; set; } = new();

        public List<string> Help()
        {
            if (Args.Count == 0)
            {
                return ApplicationCode.DocumentStore.Documents
                    .Select(document => document.Name)
                    .ToList();
            }
            return new();
        }

        public void Execute()
        {
            if (Args.Count != 2)
            {
                ApplicationCode.NotificationStore.Send(Notification.Error("Invalid command format"));
                return;
            }
            if (!ApplicationCode.DocumentStore.Documents.Any(document => document.Name.Equals(Args[0])))
            {
                ApplicationCode.NotificationStore.Send(Notification.Error("Document not found"));
                return;
            }
            ApplicationCode.DocumentStore.Documents.First(document => document.Name.Equals(Args[0])).Name = Args[1];
            ApplicationCode.NotificationStore.Send(Notification.Info("Document renamed"));
            Args = new();
        }
    }
}
