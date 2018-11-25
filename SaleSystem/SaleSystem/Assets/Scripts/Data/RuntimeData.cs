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
        private Queue<RoomRecord> _records = new Queue<RoomRecord>();
        private Queue<RoomRecord> _unFinishRecords = new Queue<RoomRecord>();

        public int Index
        {
            get { return _index; }
        }

        public string Name
        {
            get { return _name; }
        }

        public Queue<RoomRecord> Records
        {
            get { return _records; }
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

        public Room(int index)
        {
            _index = index;
            _indexStr = (index + 1).ToString();
            _name = "客房" + _indexStr;
            _price = SaleConstDefine.DefaultRoomPrice;
        }

        public void AddRecord(RoomRecord roomRecord)
        {
            _records.Enqueue(roomRecord);
            _unFinishRecords.Enqueue(roomRecord);
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
                if (record.CheckOutDate.GetDays() < nowDay || record.State == ERoomerState.CheckOut)
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
    }

    public enum ERoomState
    {
        空闲,
        订房,
        入住,
        待退房,
    }
}