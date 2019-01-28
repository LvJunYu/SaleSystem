using System;
using System.Collections.Generic;

namespace Sale
{
    public class RoomRecord
    {
        public int Id;
        public int RoomIndex;
        public string RoommerName;
        public int RoommerNum;
        public DateTime CreateDate;
        public DateTime CheckInDate;
        public DateTime CheckOutDate;
        public ERoomerState State;
        public int Price;
        public List<PayRecord> PayRecords = new List<PayRecord>();
        public List<PayRecord> ChangePayRecords;
        public ERecordPhase RecordPhase;

        public RoomRecord()
        {
        }

        public RoomRecord(RoomRecordData data)
        {
            Id = data.Id;
            RoomIndex = data.RoomIndex;
            RoommerName = data.RoommerName;
            RoommerNum = data.RoommerNum;
            CreateDate = data.CreateDate;
            CheckInDate = data.CheckInDate;
            CheckOutDate = data.CheckOutDate;
            State = data.State;
            Price = data.Price;
            PayRecords = data.PayRecords;
            RecordPhase = ERecordPhase.Valid;
        }

        public RoomRecordData GetData(RoomRecordData data = null)
        {
            if (data == null)
            {
                data = new RoomRecordData();
            }

            data.Id = Id;
            data.RoomIndex = RoomIndex;
            data.RoommerName = RoommerName;
            data.RoommerNum = RoommerNum;
            data.CreateDate = CreateDate;
            data.CheckInDate = CheckInDate;
            data.CheckOutDate = CheckOutDate;
            data.State = State;
            data.Price = Price;
            data.PayRecords = PayRecords;
            return data;
        }
    }

    public class Room
    {
        private int _index;
        private string _idStr;
        private string _name;
        private int _price;
        private ERoomState _state;
        private List<RoomRecord> _records = new List<RoomRecord>();
        private List<RoomRecord> _unFinishRecords = new List<RoomRecord>();

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

        public string IdStr
        {
            get { return _idStr; }
        }

        public int Price
        {
            get { return _price; }
        }

        public List<RoomRecord> Records
        {
            get { return _records; }
        }

        public Room(int index)
        {
            _index = index;
            _idStr = (index + 1).ToString();
            _name = "客房" + _idStr;
            _price = SaleConstDefine.DefaultRoomPrice;
        }

        public void AddRecord(RoomRecord roomRecord)
        {
            _records.Add(roomRecord);
            _unFinishRecords.Add(roomRecord);
        }

        public void RemoveRecord(RoomRecord data)
        {
            _records.Remove(data);
            var index = _unFinishRecords.IndexOf(data);
            if (index >= 0)
            {
                _unFinishRecords.RemoveAt(index);
            }
        }

        public void ClearRecords()
        {
            _records.Clear();
            _unFinishRecords.Clear();
        }

        public void RefreshState()
        {
            _unFinishRecords.Sort((p, q) => (q.CheckInDate - p.CheckInDate).Days); //入住时间倒叙排
            var nowDay = DateTime.Now.GetDays();
            var index = _unFinishRecords.Count - 1;
            while (index >= 0 && index < _unFinishRecords.Count)
            {
                var record = _unFinishRecords[index];
                if (record.CheckOutDate.GetDays() < nowDay || record.State == ERoomerState.退房)
                {
                    _unFinishRecords.RemoveAt(index);
                }

                index--;
            }

            if (_unFinishRecords.Count == 0)
            {
                _state = ERoomState.空闲;
            }
            else
            {
                var lastRecord = _unFinishRecords[_unFinishRecords.Count - 1];
                if (lastRecord.CheckInDate.GetDays() <= nowDay)
                {
                    if (lastRecord.CheckOutDate.GetDays() == nowDay)
                    {
                        _state = ERoomState.今天到期;
                    }
                    else
                    {
                        _state = ERoomState.已入住;
                    }
                }
                else
                {
                    _state = ERoomState.已预定;
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

        public bool CheckDateConflict(RoomRecord checkRecord, DateTime checkInData, DateTime checkOutDate)
        {
            foreach (var record in _unFinishRecords)
            {
                if (record == checkRecord) continue;
                if (DateTimeHelper.IsConflict(checkInData, checkOutDate, record.CheckInDate, record.CheckOutDate))
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
        已预定,
        已入住,
        今天到期,
    }

    public enum ERecordPhase
    {
        Creating,
        Valid
    }
}