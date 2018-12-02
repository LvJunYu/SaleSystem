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
            var name = _cachedView.Name.text;
            if (!string.IsNullOrEmpty(name) && name == "admin" &&
                "123456" == _cachedView.Pwd.text)
            {
                UserData.Instance.AdminLogin(name);
                SocialGUIManager.Instance.CloseUI<UICtrlLogin>();
                SocialGUIManager.Instance.OpenUI<UICtrlMainApp>();
            }
            else
            {
                SocialGUIManager.ShowPopupDialog("用户名或密码错误。");
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