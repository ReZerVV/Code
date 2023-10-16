namespace EasyUI.Core
{
    public abstract class Designer
    {
        public Vector2 Position => Content.Position;
        public Vector2 Size => Content.Size;
        public WindowBase Content { get; set; }
    }
}
