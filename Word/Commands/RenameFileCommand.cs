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
                return AppState.DocumentStore.Docs
                    .Select(document => document.GetName())
                    .ToList();
            }
            return new();
        }

        public void Execute()
        {
            if (Args.Count != 2)
            {
                AppState.NotificationStore.Send(Notification.Error("Invalid command format"));
                return;
            }
            if (!AppState.DocumentStore.Docs.Any(document => document.GetName().Equals(Args[0])))
            {
                AppState.NotificationStore.Send(Notification.Error("Document not found"));
                return;
            }
            AppState.DocumentStore.Docs.First(document => document.GetName().Equals(Args[0])).SetName(Args[1]);
            AppState.NotificationStore.Send(Notification.Info("Document renamed"));
            Args = new();
        }
    }
}
