namespace Sale
{
    public class Account
    {
        public EUserType UserType;
        public string UserName;

        public Account(string name, EUserType type)
        {
            UserName = name;
            UserType = type;
        }
    }

    public enum EUserType
    {
        Guest,
        Administrator
    }
}