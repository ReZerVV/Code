using EasyUI.Components;
using EasyUI.Draw;

namespace EasyUI.Layouts
{
    public abstract class LayoutBase : ComponentBase
    {
        public int Count => Items.Count;

        public List<ComponentBase> Items { get; set; } = new List<ComponentBase>();

        public void Add(ComponentBase component)
        {
            Items.Add(component);
            OnPropertyChanged();
        }

        public bool Remove(ComponentBase component)
        {
            OnPropertyChanged();
            return Items.Remove(component);
        }

        public void RemoveAt(int index)
        {
            Items.RemoveAt(index);
            OnPropertyChanged();
        }

        public void Clear()
        {
            Items.Clear();
            OnPropertyChanged();
        }

        public override void Input(ConsoleKeyInfo keyInfo)
        {
            foreach (var item in Items)
            {
                item.Input(keyInfo);
            }
        }

        public override void Update()
        {
            base.Update();
            foreach (var item in Items)
            {
                item.Update();
            }
        }

        public override void LateUpdate()
        {
            base.LateUpdate();
            foreach (var item in Items)
            {
                item.LateUpdate();
            }
        }

        public override void Render(Canvas canvas)
        {
            foreach (var item in Items)
            {
                item.Render(canvas);
            }
        }
    }
}
