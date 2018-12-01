using UITools;

namespace Sale
{
    [UIAutoSetup]
    public class UICtrlSetting : UICtrlGenericBase<UIViewSetting>
    {
        protected override void InitGroupId()
        {
            _groupId = (int) EUIGroupType.Pop1;
        }

        protected override void OnViewCreated()
        {
            base.OnViewCreated();
            _cachedView.CloseBtn.onClick.AddListener(CloseBtn);
            _cachedView.PayTypeSetting.onClick.AddListener(PayTypeSetting);
            _cachedView.RoomSetting.onClick.AddListener(RoomSetting);
            _cachedView.QuitBtn.onClick.AddListener(QuitBtn);
        }

        private void PayTypeSetting()
        {
            SocialGUIManager.Instance.OpenUI<UICtrlChangePayType>();
        }

        private void RoomSetting()
        {
            SocialGUIManager.Instance.OpenUI<UICtrlChangeRoom>();
        }

        private void QuitBtn()
        {
            AppMain.Instance.QuitGame();
        }

        private void CloseBtn()
        {
            SocialGUIManager.Instance.CloseUI<UICtrlSetting>();
        }
    }
}