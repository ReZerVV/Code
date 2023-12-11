using System.Reflection;
using Word.Stores;
using Word.Themes;

namespace Word.Commands
{
    public class SelectColorThemeCommand : ICommand
    {
        public string Name => "Select Color Theme";

        public bool HasArgs => true;

        public int CountArgs => 1;

        public List<string> Args { get; set; } = new();

        public List<string> Help()
        {
            if (Args.Count == 0)
            {
                return Assembly.GetExecutingAssembly().GetTypes()
                    .Where(type => typeof(ITheme).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract)
                    .Select(type => ((ITheme)Activator.CreateInstance(type)).Name)
                    .ToList();
            }
            return new();
        }

        public void Execute()
        {
            if (Args.Count != 1)
            {
                AppState.NotificationStore.Send(Notification.Error("Invalid command format"));
                return;
            }
            var theme = (ITheme?)Assembly.GetExecutingAssembly().GetTypes()
                 .Where(type => typeof(ITheme).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract)
                 .Select(type => (ITheme)Activator.CreateInstance(type))
                 .FirstOrDefault(highlighter => highlighter.Name == Args[0]);
            if (theme == null)
            {
                AppState.NotificationStore.Send(Notification.Error("Theme not found"));
                return;
            }
            AppState.Theme = theme;
            AppState.ThemeChanged = true;
            AppState.NotificationStore.Send(Notification.Info($"Color theme has been changed to {theme.Name}"));
        }
    }
}
