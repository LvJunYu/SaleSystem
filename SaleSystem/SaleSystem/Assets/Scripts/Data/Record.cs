using System;

namespace Sale
{
    public class Record
    {
        public DateTime CheckInDate;
        public DateTime CheckOutDate;
        public string RoomerName;
        public string RoomNum;
        public float RoomPrice;
        public EPayType PayType;
    }

    public enum EPayType
    {
        Cash,
        WeChat,
        Alipay,
        Pos
    }
}