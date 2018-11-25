using System;
using System.Collections.Generic;

namespace Sale
{
    public class RoomRecord
    {
        public int RoomIndex;
        public DateTime CheckInDate;
        public DateTime CheckOutDate;
        public float Price;
        public List<PayRecord> PayRecords = new List<PayRecord>();
    }

    public class PayRecord
    {
        public EPayType PayType;
        public float PayNum;
    }

    public enum EPayType
    {
        Cash,
        WeChat,
        Alipay,
        Pos
    }
}