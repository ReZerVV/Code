using EasyUI.Core.Components;

namespace Word.Services
{
    public class NavigationService
    {
        public Stack<IComponent> History { get; private set; } = new Stack<IComponent>();
        public IComponent? CurrentView { get; private set; } = null;
        public bool IsViewChanged { get; set; } = false;

        public void Navigate(IComponent view)
        {
            if (CurrentView != null)
            {
                History.Push(CurrentView);
            }
            CurrentView = view;
            IsViewChanged = true;
        }

        public void Back()
        {
            if (History.TryPop(out IComponent? view))
            {
                CurrentView = view;
                IsViewChanged = true;
            }
        }
    }
}
