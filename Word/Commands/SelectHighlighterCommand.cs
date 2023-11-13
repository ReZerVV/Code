using System.Data;
using System.Reflection;
using Word.Core.Syntax;
using Word.Stores;

namespace Word.Commands
{
    public class SelectHighlighterCommand : ICommand
    {
        public string Name => "Select Highlighter";
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
            if (Args.Count == 1)
            {
                return Assembly.GetExecutingAssembly().GetTypes()
                    .Where(type => typeof(IHighlighter).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract)
                    .Select(type => ((IHighlighter)Activator.CreateInstance(type)).LangName)
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
            var document = ApplicationCode.DocumentStore.Documents.First(document => document.Name.Equals(Args[0]));
            if (document == null)
            {
                ApplicationCode.NotificationStore.Send(Notification.Error("Document not found"));
                return;
            }
            var highlighter = (IHighlighter?)Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => typeof(IHighlighter).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract)
                .Select(type => (IHighlighter)Activator.CreateInstance(type))
                .Where(highlighter => highlighter.LangName == Args[1])
                .SingleOrDefault();
            if (highlighter == null)
            {
                ApplicationCode.NotificationStore.Send(Notification.Error("Document not found"));
                return;
            }
            document.Highlighter = highlighter;
            ApplicationCode.NotificationStore.Send(Notification.Info($"The selected highlight style for the {document.Name} document"));
        }
    }
}
