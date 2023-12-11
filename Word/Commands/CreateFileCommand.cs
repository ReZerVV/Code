using Word.CustomComponents;
using Word.Stores;

namespace Word.Commands
{
    public class CreateFileCommand : ICommand
    {
        public string Name => "Create File";
        public bool HasArgs => true;
        public int CountArgs => 1;
        public List<string> Args { get; set; } = new();

        public List<string> Help()
        {
            return new();
        }

        public void Execute()
        {
            if (Args.Count != CountArgs)
            {
                AppState.NotificationStore.Send(Notification.Error("Invalid command format"));
                return;
            }
            if (AppState.NavigationService.CurrentView is not DocumentTabComponent)
            {
                AppState.NavigationService.Navigate(new DocumentTabComponent());
            }
            AppState.DocumentStore.OpenDoc(new Core.Document(Args[0]));
            AppState.NotificationStore.Send(Notification.Info("Document created"));
            Args = new();
        }
    }
}
