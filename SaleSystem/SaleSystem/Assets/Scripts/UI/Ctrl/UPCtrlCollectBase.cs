using UITools;

namespace Sale
{
    public class UPCtrlCollectBase : UPCtrlTapBase<UICtrlCollect, UIViewCollect>
    {
        public override void Open()
        {
            base.Open();
            RefreshView();
        }

        public virtual void RefreshView()
        {
        }
    }
}