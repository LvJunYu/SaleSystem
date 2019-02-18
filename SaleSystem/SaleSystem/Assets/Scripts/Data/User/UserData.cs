using System;
using MyTools;

namespace Sale
{
    public class UserData : Singleton<UserData>
    {
        private Account _account;
        private string _url = "http://192.168.1.102:7701/test?num=10&num1=555";

        public UserData()
        {
            GuestLogin();
        }

        public void GuestLogin()
        {
            _account = new Account("Guest", EUserType.Guest);
        }

        public bool TryAdminLogin(string name, string pwd)
        {
            for (int i = 0; i < (int) EUserType.Max; i++)
            {
                if (name == Account.AdminNames[i] && pwd == Account.Passwords[i])
                {
                    _account = new Account(name, (EUserType) i);
                    return true;
                }
            }

            SocialGUIManager.ShowPopupDialog("用户名或密码错误。");
            return false;
        }

        public bool CheckIdentity(EBehaviorType behaviorType, bool remind = true)
        {
            switch (behaviorType)
            {
                case EBehaviorType.SaveRecordData:
                case EBehaviorType.CreateRecord:
                    if (_account.UserType > EUserType.Guest)
                    {
                        return true;
                    }
                    break;
                case EBehaviorType.ChangeRoomData:
                case EBehaviorType.ChangePayType:
                case EBehaviorType.DeleteAllData:
                    if (_account.UserType == EUserType.AdminLeve2)
                    {
                        return true;
                    }
                    break;
                case EBehaviorType.UpdateRecord:
                case EBehaviorType.DeleteRecord:
                    if (_account.UserType > EUserType.AdminLeve0)
                    {
                        return true;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException("behaviorType", behaviorType, null);
            }

            if (remind)
            {
                switch (behaviorType)
                {
                    case EBehaviorType.SaveRecordData:
                        SocialGUIManager.ShowPopupDialog("您没有权限修改订单数据");
                        break;
                    case EBehaviorType.ChangeRoomData:
                        SocialGUIManager.ShowPopupDialog("您没有权限修改房间数据");
                        break;
                    case EBehaviorType.ChangePayType:
                        SocialGUIManager.ShowPopupDialog("您没有权限修改付款类型数据");
                        break;
                    case EBehaviorType.DeleteAllData:
                        SocialGUIManager.ShowPopupDialog("您没有权限修删除所有数据");
                        break;
                    case EBehaviorType.CreateRecord:
                        SocialGUIManager.ShowPopupDialog("您没有权限创建订单");
                        break;
                    case EBehaviorType.UpdateRecord:
                        SocialGUIManager.ShowPopupDialog("您没有权限修改订单");
                        break;
                    case EBehaviorType.DeleteRecord:
                        SocialGUIManager.ShowPopupDialog("您没有权限删除订单");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("behaviorType", behaviorType, null);
                }
            }

            return false;
        }
    }

    public enum EBehaviorType
    {
        SaveRecordData,
        ChangeRoomData,
        ChangePayType,
        DeleteAllData,
        CreateRecord,
        UpdateRecord,
        DeleteRecord,
    }
}