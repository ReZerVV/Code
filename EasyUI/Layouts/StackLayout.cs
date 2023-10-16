using EasyUI.Core.Options;

namespace EasyUI.Layouts
{
    public class StackLayout : LayoutBase
    {
        public Orientation Orientation { get; set; } = Orientation.Vertical;

        public int Gap { get; set; } = 1;

        public override void LateUpdate()
        {
            base.LateUpdate();
            for (int indexItem = 0; indexItem < Items.Count; indexItem++)
            {
                if (indexItem == 0)
                {
                    Items[indexItem].Position.x = Position.x;
                    Items[indexItem].Position.y = Position.y;
                    if (Orientation == Orientation.Vertical)
                    {
                        Size.x = Items[indexItem].Position.x + Items[indexItem].Size.x;
                    }
                    if (Orientation == Orientation.Horizontal)
                    {
                        Size.y = Items[indexItem].Position.y + Items[indexItem].Size.y;
                    }
                    Items[indexItem].LateUpdate();
                    continue;
                }
                if (Orientation == Orientation.Vertical)
                {
                    Items[indexItem].Position.x = Position.x;
                    Items[indexItem].Position.y = Items[indexItem - 1].Position.y + Items[indexItem - 1].Size.y + Gap;
                    Size.y = Items[indexItem].Position.y + Items[indexItem].Size.y;
                }
                if (Orientation == Orientation.Horizontal)
                {
                    Items[indexItem].Position.x = Items[indexItem - 1].Position.x + Items[indexItem - 1].Size.x + Gap;
                    Items[indexItem].Position.y = Position.y;
                    Size.x = Items[indexItem].Position.x + Items[indexItem].Size.x;
                }
                Items[indexItem].LateUpdate();
            }
            Changed = false;
        }
    }
}
