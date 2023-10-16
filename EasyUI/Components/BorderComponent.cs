using EasyUI.Core.Options;
using EasyUI.Draw;

namespace EasyUI.Components
{
    public class BorderComponent : ComponentBase
    {
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
                content.Position = Position + 1;
                content.Size = Size - 1;
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

        public override void Input(ConsoleKeyInfo keyInfo)
        {
            base.Input(keyInfo);
            Content.Input(keyInfo);
        }

        public override void LateUpdate()
        {
            base.LateUpdate();
            Content.LateUpdate();
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
            Content.Render(canvas);
        }

        public override void Update()
        {
            base.Update();
            Content.Update();
        }
    }
}
