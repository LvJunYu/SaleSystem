using UITools;

namespace Sale
{
    [UIAutoSetup]
    public class UICtrlChangeRoom : UICtrlGenericBase<UIViewChangeRoom>
    {
        protected override void InitGroupId()
        {
            _groupId = (int) EUIGroupType.PopUpDialog;
        }
    }
}