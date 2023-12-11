using Word.Stores;

namespace Word.Commands
{
    public class SaveFileCommand : ICommand
    {
        public string Name => "Save File";

        public bool HasArgs => true;

        public int CountArgs { get; set; } = 2;

        public List<string> Args { get; set; } = new();

        public List<string> Help()
        {
            if (Args.Count == 0)
            {
                return AppState.DocumentStore.Docs
                    .Select(document => document.GetName())
                    .ToList();
            }
            if (Args.Count == 1)
            {
                var document = AppState.DocumentStore.Docs
                    .FirstOrDefault(document => document.GetName() == Args[0]);
                if (document != null &&
                    document.GetPath() != string.Empty)
                {
                    return new() { document.GetPath() };
                }
            }
            return new();
        }
        
        public void Execute()
        {
            var document = AppState.DocumentStore.Docs.First(document => document.GetName().Equals(Args[0]));
            if (document == null)
            {
                AppState.NotificationStore.Send(Notification.Error("Document not found"));
                return;
            }
            document.SetPath(Args[1]);
            document.TrySave();
            AppState.NotificationStore.Send(Notification.Info($"Saved the {document.GetName()} document"));
        }
    }
}
