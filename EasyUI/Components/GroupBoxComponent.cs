using EasyUI.Core.Options;
using EasyUI.Draw;

namespace EasyUI.Components
{
    public class GroupBoxComponent : ComponentBase
    {
        private string title;
        public string Title 
        { 
            get 
            { 
                return title; 
            } 
            set 
            { 
                title = value;
                OnPropertyChanged();
            }
        }

        private ComponentBase content;
        public ComponentBase Content
        {
            get
            {
                return content;
            }
            set
            {
                content = value;
                OnPropertyChanged();
            }
        }

        private Color borderBrush = Color.White;
        public Color BorderBrush
        {
            get
            {
                return borderBrush;
            }
            set
            {
                borderBrush = value;
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

        private BorderStyle borderStyle = BorderStyle.Solid;
        public BorderStyle BorderStyle
        {
            get
            {
                return borderStyle;
            }
            set
            {
                borderStyle = value;
                OnPropertyChanged();
            }
        }

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

        public override void Render(Canvas canvas)
        {
            base.Render(canvas);
            switch (BorderStyle)
            {
                case BorderStyle.None:
                    {
                    }
                    break;

                case BorderStyle.Solid:
                    {
                        canvas.DrawBorder(
                            Position.x,
                            Position.y,
                            Size.x,
                            Size.y,
                            BorderBrush,
                            Background);
                    }
                    break;

                case BorderStyle.Rounded:
                    {
                        canvas.DrawRoundedBorder(
                            Position.x,
                            Position.y,
                            Size.x,
                            Size.y,
                            BorderBrush,
                            Background);
                    }
                    break;
            }
            canvas.DrawText(
                $" {Title} ",
                (int)((Position.x + Size.x) / 2 - (Title.Length / 2)),
                Position.y,
                Foreground,
                Background);
            Content?.Render(canvas);
        }

        public override void Update()
        {
            base.Update();
            if (Content != null) 
            {
                Content.Position = Position + 1;
                Content.Size = Size - 1;
            }
        }

        public override void Input(ConsoleKeyInfo keyInfo)
        {
            base.Input(keyInfo);
            if (Content != null) 
            {
                Content.Input(keyInfo);
            }
        }

        public override void LateUpdate()
        {
            base.LateUpdate();
            if (Content != null) 
            {
                Content.LateUpdate();
            }
        }
    }
}
