using MyTools;

namespace Sale
{
    public class UserData : Singleton<UserData>
    {
        private Account _account;

        public Account Account
        {
            get { return _account; }
        }

        public void GuestLogin()
        {
            _account = new Account("Guest", EUserType.Guest);
        }

        public void AdminLogin(string name)
        {
            _account = new Account(name, EUserType.Administrator);
        }
    }
}