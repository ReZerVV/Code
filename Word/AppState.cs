using Word.Services;
using Word.Stores;
using Word.Themes;

namespace Word
{
    public static class AppState
    {
        public static DocumentStore DocumentStore { get; private set; } = new DocumentStore();
        public static ITheme Theme { get; set; } = new DefaultDarkTheme();
        public static bool ThemeChanged { get; set; } = false;
        public static NavigationService NavigationService { get; set; } = new NavigationService();
        public static CommandService CommandService { get; set; } = new CommandService();
        public static NotificationStore NotificationStore { get; set; } = new NotificationStore();

        public static void Initialize()
        {

        }
    }
}
