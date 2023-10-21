using EasyUI.Components;
using EasyUI.Draw;

namespace EasyUI.Core
{
    public abstract class WindowBase
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
            }
        }

        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }

        public WindowBase(int width, int height)
        {
            Position = Vector2.Zero;
            Size = new Vector2(width, height);
        }

        public virtual void Input(ConsoleKeyInfo keyInfo)
        {
            if (Content != null)
            {
                Content.Input(keyInfo);
            }
        }

        public virtual void Update()
        {
            if (Content != null)
            { 
                Content.Update();
            }
        }

        public virtual void LateUpdate()
        {
            if (Content != null)
            {
                content.Position = Position;
                content.Size = Size;
                Content.LateUpdate();
            }
        }

        public virtual void Render(Canvas canvas)
        {
            if (Content != null) 
            {
                Content.Render(canvas);
            }
        }
    }
}
