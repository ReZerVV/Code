using EasyUI.Core;
using EasyUI.Core.Options;
using EasyUI.Draw;

namespace EasyUI.Components
{
    public class TextComponent : ComponentBase
    {
        private Color foreground = Color.White;
        public Color Foreground 
        {
            get
            {
                return foreground;
            }
            set
            {
                foreground = value;
                OnPropertyChanged();
            }
        }

        private Color background = Color.Black;
        public Color Background
        {
            get
            {
                return background;
            }
            set
            {
                background = value;
                OnPropertyChanged();
            }
        }

        private string text = string.Empty;
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
                OnPropertyChanged();
            }
        }

        private TextWrapping textWrapping = TextWrapping.NoWrap;
        public TextWrapping TextWrapping
        {
            get
            {
                return textWrapping;
            }
            set
            {
                textWrapping = value;
                OnPropertyChanged();
            }
        }

        public override void Render(Canvas canvas)
        {
            switch (TextWrapping)
            {
                case TextWrapping.NoWrap:
                    {
                        canvas.DrawText(
                            text: Text.Substring(0, Math.Min(Size.x, Text.Length)),
                            x: Position.x,
                            y: Position.y,
                            foreground: Foreground,
                            background: Background);
                    }
                break;

                case TextWrapping.Wrap:
                    { 
                        Vector2 currentPosition = new Vector2(Position.x, Position.y);
                        for (int index = 0; index < Text.Length; index += Size.x, currentPosition.y++)
                        {
                            if (currentPosition.y > Position.y + Size.y)
                            {
                                return;
                            }

                            canvas.DrawText(
                                text: Text.Substring(index, Math.Min(Size.x, Text.Length - index)),
                                x: currentPosition.x,
                                y: currentPosition.y,
                                foreground: Foreground,
                                background: Background);
                        }
                    }
                break;

                case TextWrapping.WrapByWord:
                    { 
                        Vector2 currentPosition = new Vector2(Position.x, Position.y);
                        foreach (string word in Text.Split(' '))
                        {
                            currentPosition.x += word.Length;
                            if (currentPosition.x >= Position.x + Size.x)
                            {
                                currentPosition.y++;
                                currentPosition.x = Position.x;
                            }

                            if (currentPosition.y > Position.y + Size.y)
                            {
                                return;
                            }

                            canvas.DrawText(
                                text: word,
                                x: currentPosition.x,
                                y: currentPosition.y,
                                foreground: Foreground,
                                background: Background);
                        }
                    }
                break;
            }
        }
    }
}
