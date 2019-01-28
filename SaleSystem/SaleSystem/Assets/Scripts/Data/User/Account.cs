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

        public static string[] AdminNames = {"admin01", "admin02", "admin03"};
        public static string[] Passwords = {"aaaaaa", "bbbccc", "ddd123"};
    }

    public enum EUserType
    {
        Guest = -1,
        AdminLeve0,
        AdminLeve1,
        AdminLeve2,
        Max
    }
}