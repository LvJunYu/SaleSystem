using System;
using System.Collections.Generic;

namespace Sale
{
    public class RoomRecord
    {
        public int RoomIndex;
        public DateTime CreateDate;
        public DateTime CheckInDate;
        public DateTime CheckOutDate;
        public ERoomerState State;
        public int Price;
        public List<PayRecord> PayRecords = new List<PayRecord>();
    }

    public enum ERoomerState
    {
        Reservation,
        CheckIn,
        CheckOut
    }

    public class RoomData
    {
        public string Name;
        public int Price;

        public RoomData()
        {
        }

        public RoomData(string name, int price = SaleConstDefine.DefaultRoomPrice)
        {
            Name = name;
            Price = price;
        }
    }

    public class PayRecord
    {
        public EPayType PayType;
        public int PayNum;

        public PayRecord(int payNum, EPayType payType)
        {
            PayNum = payNum;
            PayType = payType;
        }
    }

    public enum EPayType
    {
        现金,
        微信,
        支付宝,
        Pos机,
        Max
    }
}