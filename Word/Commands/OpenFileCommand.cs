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
                AppState.NotificationStore.Send(Notification.Error("Invalid command format"));
                return;
            }
            if (AppState.NavigationService.CurrentView is not DocumentTabComponent)
            {
                AppState.NavigationService.Navigate(new DocumentTabComponent());
            }
            AppState.DocumentStore.LoadDoc(Args[0]);
            AppState.NotificationStore.Send(Notification.Info("Document loaded"));
        }
    }
}
