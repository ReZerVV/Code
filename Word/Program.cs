using EasyUI.Core;
using Word.Services;
using Word.Stores;
using Word.Themes;
using Word.Views;

namespace Word
{
    class Program
    {
        public static void Main(string[] args)
        {
            new ApplicationCode().AppLoop();
        }
    }

    class ApplicationCode : AppBase
    {
        public static ITheme Theme { get; set; } = new DefaultDarkTheme();
        public static bool ThemeChanged { get; set; } = false;
        public static NavigationService NavigationService { get; set; }
        public static CommandService CommandService { get; set; }
        public static DocumentStore DocumentStore { get; set; }
        public static NotificationStore NotificationStore { get; set; }

        public ApplicationCode()
            : base()
        {
            NavigationService = new NavigationService();
            CommandService = new CommandService();
            DocumentStore = new DocumentStore();
            NotificationStore = new NotificationStore();

            MainComponent = new MainView();
            MainComponent.Position = Vector2.Zero;
        }
    }
}
