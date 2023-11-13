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
                return ApplicationCode.DocumentStore.Documents
                    .Select(document => document.Name)
                    .ToList();
            }
            if (Args.Count == 1)
            {
                var document = ApplicationCode.DocumentStore.Documents
                    .FirstOrDefault(document => document.Name == Args[0]);
                if (document != null &&
                    document.Path != string.Empty)
                {
                    CountArgs = 1;
                }
            }
            return new();
        }
        
        public void Execute()
        {
            var document = ApplicationCode.DocumentStore.Documents.First(document => document.Name.Equals(Args[0]));
            if (document == null)
            {
                ApplicationCode.NotificationStore.Send(Notification.Error("Document not found"));
                return;
            }
            if (document.Path == string.Empty)
            {
                document.Path = Args[1];
            }
            document.Save();
            ApplicationCode.NotificationStore.Send(Notification.Info($"Saved the {document.Name} document"));
        }
    }
}
