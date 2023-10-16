using EasyUI.Draw;

namespace EasyUI.Core
{
    public class ModalWindow : WindowBase
    {
        public Color Background { get; set; } = Color.Black;

        public ModalWindow(int x, int y, int width, int height)
            : base(width, height)
        {
            Position.x = x;
            Position.y = y;
        }

        public override void Render(Canvas canvas)
        {
            canvas.DrawFillRect(
                Position.x,
                Position.y,
                Size.x,
                Size.y,
                Background);
            base.Render(canvas);
        }

        public override void Update()
        {
            if (Content != null) 
            {
                Content.Position = Position;
                Content.Size = Size;
            }
            base.Update();
        }
    }
}
