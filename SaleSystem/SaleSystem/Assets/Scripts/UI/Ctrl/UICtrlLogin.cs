using UITools;

namespace Sale
{
    [UIAutoSetup]
    public class UICtrlLogin : UICtrlGenericBase<UIViewLogin>
    {
        protected override void InitGroupId()
        {
            _groupId = (int) EUIGroupType.Pop2;
        }

        protected override void OnViewCreated()
        {
            base.OnViewCreated();
        }
    }
}