using MyTools;

namespace Sale
{
    public class UserData : Singleton<UserData>
    {
        private Account _account;
        private string _url = "http://192.168.1.102:7701/test?num=10&num1=555";

        public Account Account
        {
            get { return _account; }
        }

        public void GuestLogin()
        {
            _account = new Account("Guest", EUserType.Guest);
            NetworkManager.AppHttpClient.Post(_url, null, www =>
            {
                LogHelper.Info(www.text);
            });
        }

        public bool TryAdminLogin(string name, string pwd)
        {
            if (name == "123456" && pwd == "qqqqqq")
            {
                _account = new Account(name, EUserType.Administrator);
                return true;
            }
            else
            {
                SocialGUIManager.ShowPopupDialog("用户名或密码错误。");
                return false;
            }
        }

        public bool CheckIdentity(bool remind = true)
        {
            if (_account.UserType == EUserType.Administrator)
            {
                return true;
            }

            if (remind)
            {
                SocialGUIManager.ShowPopupDialog("您没有管理员权限");
            }

            return false;
        }
    }
}