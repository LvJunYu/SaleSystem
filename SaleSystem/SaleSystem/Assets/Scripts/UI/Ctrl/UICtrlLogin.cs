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
            _cachedView.Guest.onClick.AddListener(GuestBtn);
            _cachedView.Login.onClick.AddListener(LoginBtn);
        }

        private void LoginBtn()
        {
            if (UserData.Instance.TryAdminLogin(_cachedView.Name.text, _cachedView.Pwd.text))
            {
                SocialGUIManager.Instance.CloseUI<UICtrlLogin>();
                SocialGUIManager.Instance.OpenUI<UICtrlMainApp>();
            }
        }

        private void GuestBtn()
        {
            UserData.Instance.GuestLogin();
            SocialGUIManager.Instance.CloseUI<UICtrlLogin>();
            SocialGUIManager.Instance.OpenUI<UICtrlMainApp>();
        }
    }
}