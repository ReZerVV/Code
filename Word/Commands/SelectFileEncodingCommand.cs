using System.Text;
using Word.Stores;

namespace Word.Commands
{
    public class SelectFileEncodingCommand : ICommand
    {
        public string Name => "Select Encodings";

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
            if (Args.Count == 1)
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                return Encoding.GetEncodings()
                    .Select(x => x.GetEncoding())
                    .Select(encoding => $"{encoding.EncodingName} ({encoding.CodePage})")
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
            var document = AppState.DocumentStore.Docs.First(document => document.GetName().Equals(Args[0]));
            if (document == null)
            {
                AppState.NotificationStore.Send(Notification.Error("Document not found"));
                return;
            }
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var encoding = Encoding.GetEncodings()
                    .Select(x => x.GetEncoding())
                    .FirstOrDefault(x => $"{x.EncodingName} ({x.CodePage})" == Args[1]);
            if (encoding == null)
            {
                AppState.NotificationStore.Send(Notification.Error("Encoding not found"));
                return;
            }
            document.Encoding = encoding;
            AppState.NotificationStore.Send(Notification.Error($"The selected encoding style for the {document.GetName()} document"));
        }
    }
}
