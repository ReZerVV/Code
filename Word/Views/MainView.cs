using EasyUI.Core;
using EasyUI.Core.Components;
using EasyUI.Draw;
using Word.CustomComponents;

namespace Word.Views
{
    public class MainView : IComponent
    {
        private StatusBarComponent statusBar = new();
        private CommandComponent commandLineBar = new();
        public IComponent? CurrentView { get; set; } = null;

        private Vector2 size = Vector2.Zero;
        public Vector2 Size
        {
            get
            {
                return size;
            }
            set
            {
                size = value;
                CurrentView.Size = new Vector2(Size.x, Size.y - 2);
                commandLineBar.Size = new Vector2(50, 1);
                commandLineBar.Position = new Vector2(
                    Position.x + Size.x / 2 - commandLineBar.Size.x / 2,
                    Position.y);
                statusBar.Size = new Vector2(Size.x, 1);
            }
        }

        private Vector2 position = Vector2.Zero;
        public Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
                if (CurrentView != null)
                {
                    CurrentView.Position = new Vector2(Position.x, Position.y + 1);
                }
                commandLineBar.Position = new Vector2(
                    Position.x + Size.x / 2 - commandLineBar.Size.x / 2,
                    Position.y);
                statusBar.Position = new Vector2(Position.x, Position.y + Size.y - 1);
            }
        }

        private Color foreground;
        public Color Foreground
        {
            get
            {
                return foreground;
            }
            set
            {
                foreground = value;
                commandLineBar.Foreground = foreground;
            }
        }

        private Color background;
        public Color Background
        {
            get
            {
                return background;
            }
            set
            {
                background = value;
            }
        }

        private Color backgroundDark;
        public Color BackgroundDark
        {
            get
            {
                return backgroundDark;
            }
            set
            {
                backgroundDark = value;
                commandLineBar.Background = backgroundDark;
            }
        }

        public MainView()
        {
            Foreground = AppState.Theme.Foreground;
            Background = AppState.Theme.Background;
            BackgroundDark = AppState.Theme.BackgroundDark;
            commandLineBar.SelectCommandColor = AppState.Theme.CommandLineBarSelectColor;
        }

        private void NewDocument()
        {
            if (CurrentView is not DocumentTabComponent)
            {
                AppState.NavigationService.Navigate(new DocumentTabComponent());
            }
            AppState.DocumentStore.OpenDoc(new Core.Document());
        }

        private void CloseDocument()
        {
            AppState.DocumentStore.CloseDoc();
            if (AppState.DocumentStore.CurrentDoc == null)
            {
                CurrentView = null;
            }
        }

        private void RenameDocument()
        {
            if (AppState.DocumentStore.CurrentDoc == null)
            {
                return;
            }
            var command = AppState.CommandService.GetByName("Rename File");
            command.Args.Add(AppState.DocumentStore.CurrentDoc.GetName());
            commandLineBar.IsChanged = true;
            commandLineBar.Command = command;
        }

        private void SaveDocument()
        {
            if (AppState.DocumentStore.CurrentDoc == null)
            {
                return;
            }
            var command = AppState.CommandService.GetByName("Save File");
            command.Args.Add(AppState.DocumentStore.CurrentDoc.GetName());
            commandLineBar.IsChanged = true;
            commandLineBar.Command = command;
        }

        private void OpenDocument()
        {
            var command = AppState.CommandService.GetByName("Open File");
            commandLineBar.IsChanged = true;
            commandLineBar.Command = command;
        }

        public void Input(ConsoleKeyInfo keyInfo)
        {
            if ((keyInfo.Modifiers & ConsoleModifiers.Control) != 0 &&
                (keyInfo.Modifiers & ConsoleModifiers.Alt) != 0 &&
                keyInfo.Key == ConsoleKey.P)
                commandLineBar.IsChanged = true;
            else if (commandLineBar.IsChanged)
                commandLineBar.Input(keyInfo);
            else if ((keyInfo.Modifiers & ConsoleModifiers.Control) != 0 &&
                    keyInfo.Key == ConsoleKey.N)
                NewDocument();
            else if ((keyInfo.Modifiers & ConsoleModifiers.Control) != 0 &&
                    keyInfo.Key == ConsoleKey.O)
                OpenDocument();
            else if ((keyInfo.Modifiers & ConsoleModifiers.Control) != 0 &&
                    keyInfo.Key == ConsoleKey.Q)
                CloseDocument();
            else if ((keyInfo.Modifiers & ConsoleModifiers.Control) != 0 &&
                    keyInfo.Key == ConsoleKey.R)
                RenameDocument();
            else if ((keyInfo.Modifiers & ConsoleModifiers.Control) != 0 &&
                    keyInfo.Key == ConsoleKey.S)
                SaveDocument();
            else
                CurrentView.Input(keyInfo);
        }

        public void Update()
        {
            if (AppState.ThemeChanged)
            {
                Foreground = AppState.Theme.Foreground;
                Background = AppState.Theme.Background;
                BackgroundDark = AppState.Theme.BackgroundDark;
                commandLineBar.SelectCommandColor = AppState.Theme.CommandLineBarSelectColor;
            }

            if (AppState.NavigationService.IsViewChanged)
            {
                CurrentView = AppState.NavigationService.CurrentView;
                Position = Vector2.Zero;
                AppState.NavigationService.IsViewChanged = false;
            }
            if (CurrentView == null)
            {
                AppState.NavigationService.Navigate(new WelcomeView());
                Update();
            }
            
            commandLineBar.Update();
            CurrentView.Update();
            statusBar.Update();
        }

        public void Layout()
        {
            commandLineBar.Layout();
            if (CurrentView != null)
            {
                CurrentView.Layout();
            }
            statusBar.Layout();
        }

        public void Render(Canvas canvas)
        {
            canvas.DrawFillRect(
                x: Position.x,
                y: Position.y,
                w: Size.x,
                h: Size.y,
                color: Background);
            canvas.DrawFillRect(
                x: Position.x,
                y: Position.y,
                w: Size.x,
                h: 1,
                color: BackgroundDark);
            if (CurrentView != null)
            {
                CurrentView.Render(canvas);
            }
            commandLineBar.Render(canvas);
            statusBar.Render(canvas);
        }
    }
}
