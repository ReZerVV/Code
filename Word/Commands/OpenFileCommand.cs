using Word.CustomComponents;
using Word.Stores;

namespace Word.Commands
{
    public class OpenFileCommand : ICommand
    {
        public string Name => "Open File";

        public bool HasArgs => true;

        public int CountArgs => 1;

        public List<string> Args { get; set; } = new();

        public List<string> Help()
        {
            return new();
        }

        public void Execute()
        {
            if (Args.Count != 1)
            {
                ApplicationCode.NotificationStore.Send(Notification.Error("Invalid command format"));
                return;
            }
            if (ApplicationCode.NavigationService.CurrentView is not DocumentTabComponent)
            {
                ApplicationCode.NavigationService.Navigate(new DocumentTabComponent());
            }
            ApplicationCode.DocumentStore.LoadDocument(Args[0]);
            ApplicationCode.NotificationStore.Send(Notification.Info("Document loaded"));
        }
    }
}
