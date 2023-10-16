using EasyUI.Core;
using EasyUI.Draw;

namespace EasyUI.Components
{
    public abstract class ComponentBase
    {
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
                OnPropertyChanged();
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
                OnPropertyChanged();
            }
        }

        public bool Resizable { get; set; } = true;

        public bool Visible { get; set; } = true;

        public bool Focusable { get; set; } = false;

        public bool Focused => Focusable;

        protected bool Changed = false;

        protected void OnPropertyChanged()
        {
            Changed = true;
        }

        public virtual void Input(ConsoleKeyInfo keyInfo)
        {

        }

        public virtual void Update()
        {
            if (!Changed)
            {
                return;
            }
        }

        public virtual void LateUpdate()
        {
            if (!Changed)
            {
                return;
            }
        }

        public virtual void Render(Canvas canvas)
        {

        }
    }
}
