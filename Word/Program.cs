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
        public ApplicationCode()
            : base()
        {
            MainComponent = new MainView();
            MainComponent.Position = Vector2.Zero;
        }
    }
}
