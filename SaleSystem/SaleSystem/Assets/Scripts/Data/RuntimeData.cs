using System;
using System.Collections.Generic;

namespace Sale
{
    public class Room
    {
        private int _index;
        private string _indexStr;
        private string _name;
        private int _price;
        private ERoomState _state;
        private List<RoomRecordData> _records = new List<RoomRecordData>();
        private Queue<RoomRecordData> _unFinishRecords = new Queue<RoomRecordData>();
        //todo 现在这里还不是有序的

        public int Index
        {
            get { return _index; }
        }

        public string Name
        {
            get { return _name; }
        }

        public ERoomState State
        {
            get { return _state; }
        }

        public string IndexStr
        {
            get { return _indexStr; }
        }

        public int Price
        {
            get { return _price; }
        }

        public List<RoomRecordData> Records
        {
            get { return _records; }
        }

        public Room(int index)
        {
            _index = index;
            _indexStr = (index + 1).ToString();
            _name = "客房" + _indexStr;
            _price = SaleConstDefine.DefaultRoomPrice;
        }

        public void AddRecord(RoomRecordData roomRecordData)
        {
            _records.Add(roomRecordData);
            _unFinishRecords.Enqueue(roomRecordData);
        }

        public void ClearRecords()
        {
            _records.Clear();
            _unFinishRecords.Clear();
        }

        public void RefreshState()
        {
            var nowDay = DateTime.Now.GetDays();
            while (_unFinishRecords.Count > 0)
            {
                var record = _unFinishRecords.Peek();
                if (record.CheckOutDate.GetDays() < nowDay || record.State == ERoomerState.退房)
                {
                    _unFinishRecords.Dequeue();
                }
                else
                {
                    break;
                }
            }

            if (_unFinishRecords.Count == 0)
            {
                _state = ERoomState.空闲;
            }
            else
            {
                var curRecord = _unFinishRecords.Peek();
                if (curRecord.CheckInDate.GetDays() <= nowDay)
                {
                    if (curRecord.CheckOutDate.GetDays() == nowDay)
                    {
                        _state = ERoomState.待退房;
                    }
                    else
                    {
                        _state = ERoomState.入住;
                    }
                }
                else
                {
                    _state = ERoomState.订房;
                }
            }
        }

        public void SetData(RoomData data)
        {
            SetData(data.Name, data.Price);
        }

        public void SetData(string name, int price)
        {
            _name = name;
            _price = price;
        }

        public RoomData GetData()
        {
            return new RoomData(_name, _price);
        }

        public bool CheckDateConflict(DateTime checkInData, DateTime checkOutDate)
        {
            foreach (var record in _unFinishRecords)
            {
                if (record.IsConflict(checkInData, checkOutDate))
                {
                    return true;
                }
            }

            return false;
        }
    }

    public enum ERoomState
    {
        空闲,
        订房,
        入住,
        待退房,
    }

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
}