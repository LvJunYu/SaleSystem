using System;
using System.Collections.Generic;

namespace Sale
{
    public class RoomRecordData
    {
        public int Id;
        public int RoomIndex;
        public string RoommerName;
        public string RoommerNum;
        public DateTime CreateDate;
        public DateTime CheckInDate;
        public DateTime CheckOutDate;
        public ERoomerState State;
        public int Price;
        public List<PayRecord> PayRecords;
    }

    public enum ERoomerState
    {
        预定,
        入住,
        退房
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
        public int RecordId;
        public string PayType;
        public string PayDesc;
        public int PayNum;
        public DateTime PayTime;

        public PayRecord()
        {
        }

        public PayRecord(int payNum, string payType, DateTime payTime, int recordId)
        {
            PayNum = payNum;
            PayType = payType;
            PayTime = payTime;
            RecordId = recordId;
        }

        public static PayRecord CreateNew(int recordId)
        {
            return new PayRecord(0, SaleDataManager.Instance.PayTypes[0], DateTime.Now, recordId);
        }
    }
}