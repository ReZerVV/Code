using EasyUI.Components;
using EasyUI.Core;
using EasyUI.Core.Components;
using EasyUI.Draw;
using Word.Commands;

namespace Word.CustomComponents
{
    public class CommandComponent : IComponent
    {
        public ICommand Command = null;
        private List<string> help = new();
        private int helpListOffset = 0;
        private int helpListHeight = 20;

        private int selectedIndex = -1;
        private InputComponent inputComponent = new()
        {
            TextStyle = new PixelStyle { Bold = true },
        };
        public string Title { get; set; } = "Search";
        public string Text
        {
            get
            {
                return inputComponent.Text;
            }
            set
            {
                inputComponent.Text = value;
            }
        }
        public bool IsChanged
        {
            get
            {
                return inputComponent.IsChanged;
            }
            set
            {
                Command = null;
                Text = string.Empty;
                inputComponent.Cursor = 0;
                inputComponent.IsChanged = value;
                selectedIndex = -1;
            }
        }

        public Color Foreground
        {
            get
            {
                return inputComponent.Foreground;
            }
            set
            {
                inputComponent.Foreground = value;
            }
        }
        public Color Background
        {
            get
            {
                return inputComponent.Background;
            }
            set
            {
                inputComponent.Background = value;
            }
        }
        public Color SelectCommandColor { get; set; } = Color.Green;

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
                inputComponent.Position = new Vector2(Position.x + 1, Position.y + 1);
            }
        }
        
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
                inputComponent.Size = new Vector2(Size.x - 3, Size.y);
            }
        }

        private void MoveSelectUp()
        {
            if (selectedIndex - 1 >= -1)
            {
                selectedIndex--;
            }
        }

        private void MoveSelectDown()
        {
            if (selectedIndex + 1 < help.Count)
            {
                selectedIndex++;
            }
        }

        private void Select()
        {
            if (Command == null)
            {
                if (selectedIndex >= 0 && selectedIndex < help.Count)
                {
                    Command = AppState.CommandService.GetByName(help[selectedIndex]);
                    if (Command == null && !Command.HasArgs)
                    {
                        Command.Execute();
                        IsChanged = false;
                    }
                }
            }
            else
            {
                if (selectedIndex == -1 && Text.Length != 0)
                {
                    Command.Args.Add(Text);
                    Text = string.Empty;
                    inputComponent.Cursor = 0;
                }
                else if (selectedIndex >= 0 && selectedIndex < help.Count)
                {
                    Command.Args.Add(help[selectedIndex]);
                    Text = string.Empty;
                    inputComponent.Cursor = 0;
                }
                if (Command.Args.Count >= Command.CountArgs)
                {
                    Command.Execute();
                    IsChanged = false;
                }
            }
            selectedIndex = -1;
        }

        public void Input(ConsoleKeyInfo keyInfo)
        {
            if (IsChanged && keyInfo.Key == ConsoleKey.Escape)
                IsChanged = false;
            else if (keyInfo.Key == ConsoleKey.UpArrow)
                MoveSelectUp();
            else if (keyInfo.Key == ConsoleKey.DownArrow)
                MoveSelectDown();
            else if (keyInfo.Key == ConsoleKey.Enter)
                Select();
            else
                inputComponent.Input(keyInfo);
        }

        public void Update()
        {
            inputComponent.Update();
            if (IsChanged)
            {
                if (Command != null)
                {
                    if (Text == string.Empty)
                    {
                        help = Command.Help();
                    }
                    else
                    {
                        help = Command.Help()
                            .Where(h => h.Contains(Text))
                            .ToList();
                    }
                }
                else
                {
                    help = AppState.CommandService.Search(Text);
                }
            }
            if (selectedIndex >= help.Count)
            {
                selectedIndex = -1;
                helpListOffset = 0;
            }
            if (selectedIndex != -1 && 
                selectedIndex < helpListOffset)
            {
                helpListOffset--;
            }
            if (selectedIndex >= helpListOffset + helpListHeight)
            {
                helpListOffset++;
            }
        }

        public void Layout()
        {
            inputComponent.Layout();
        }

        public void Render(Canvas canvas)
        {
            if (IsChanged)
            {
                if (help.Count >= helpListHeight)
                {
                    canvas.DrawRoundedBorder(
                        x: Position.x,
                        y: Position.y,
                        w: Size.x,
                        h: helpListHeight + 3,
                        color: Foreground,
                        background: Background);
                    if (help.Count != 0)
                    {
                        canvas.DrawFillRect(
                            x: Position.x + 1,
                            y: Position.y + 2,
                            w: Size.x - 2,
                            h: helpListHeight,
                            color: Background);
                    }
                }
                else
                {
                    canvas.DrawRoundedBorder(
                        x: Position.x,
                        y: Position.y,
                        w: Size.x,
                        h: help.Count + 3,
                        color: Foreground,
                        background: Background);
                    if (help.Count != 0)
                    {
                        canvas.DrawFillRect(
                            x: Position.x + 1,
                            y: Position.y + 2,
                            w: Size.x - 2,
                            h: help.Count,
                            color: Background);
                    }
                }
                inputComponent.Render(canvas);
                for (int commandIndex = helpListOffset; commandIndex < help.Count; commandIndex++)
                {
                    if (commandIndex >= helpListOffset + helpListHeight)
                    {
                        break;
                    }
                    if (commandIndex == selectedIndex) 
                    {
                        canvas.DrawFillRect(
                            x: Position.x + 1,
                            y: Position.y + commandIndex + 2 - helpListOffset,
                            w: Size.x - 2,
                            h: 1,
                            color: SelectCommandColor);
                        canvas.DrawText(
                            text: help[commandIndex].Length < Size.x - 4
                                ? "▶ " + help[commandIndex]
                                : "▶ " + help[commandIndex].Substring(0, Size.x - 7) + "...",
                            x: Position.x + 1,
                            y: Position.y + commandIndex + 2 - helpListOffset,
                            foreground: Foreground,
                            background: SelectCommandColor,
                            style: PixelStyle.StyleNone);
                        continue;
                    }
                    canvas.DrawText(
                        text: help[commandIndex].Length < Size.x - 2
                            ? help[commandIndex]
                            : help[commandIndex].Substring(0, Size.x - 5) + "...",
                        x: Position.x + 1,
                        y: Position.y + commandIndex + 2 - helpListOffset,
                        foreground: Foreground,
                        background: Background,
                        style: PixelStyle.StyleNone);
                }
            }
            if (!IsChanged)
            {
                canvas.DrawText(
                    text: Title,
                    x: Position.x + Size.x / 2 - Title.Length / 2,
                    y: Position.y,
                    foreground: Foreground,
                    background: Background,
                    style: PixelStyle.StyleNone);
            }
        }
    }
}
