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
                ApplicationCode.NotificationStore.Send(Notification.Error("Invalid command format"));
                return;
            }
            if (ApplicationCode.NavigationService.CurrentView is not DocumentTabComponent)
            {
                ApplicationCode.NavigationService.Navigate(new DocumentTabComponent());
            }
            ApplicationCode.DocumentStore.CreateAndOpenDocument(Args[0]);
            ApplicationCode.NotificationStore.Send(Notification.Info("Document created"));
            Args = new();
        }
    }
}
