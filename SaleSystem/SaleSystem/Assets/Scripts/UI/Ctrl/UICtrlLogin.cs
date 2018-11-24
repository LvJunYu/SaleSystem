using UITools;
using UnityEngine;

namespace Sale
{
    [UIAutoSetup(EUIAutoSetupType.Show)]
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
            if (!string.IsNullOrEmpty(name) && PlayerPrefs.HasKey(name) &&
                PlayerPrefs.GetString(name) == _cachedView.Pwd.text)
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