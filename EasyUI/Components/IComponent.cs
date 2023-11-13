using EasyUI.Draw;

namespace EasyUI.Core.Components
{
    public interface IComponent
    {        
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }

        public void Update();

        public void Layout();

        public void Input(ConsoleKeyInfo keyInfo);

        public void Render(Canvas canvas);
    }
}
