using System.Data;
using System.Reflection;
using Word.Core;
using Word.Core.Syntax.Styles;
using Word.Stores;

namespace Word.Commands
{
    public class SelectMarkerCommand : ICommand
    {
        public string Name => "Select Marker";
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
                return MarkerService.Markers
                    .Select(m => m.LangName)
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
            var marker = MarkerService.Markers
                .FirstOrDefault(marker => marker.LangName == Args[1]);
            if (marker == null)
            {
                AppState.NotificationStore.Send(Notification.Error("Marker not found"));
                return;
            }
            document.Marker = marker;
            AppState.NotificationStore.Send(Notification.Info($"The selected highlight style for the {document.GetName()} document"));
        }
    }
}
