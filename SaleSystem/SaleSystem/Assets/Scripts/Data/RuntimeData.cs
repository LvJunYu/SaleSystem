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

        public int GetPayCount()
        {
            int sum = 0;
            for (int i = 0; i < PayRecords.Count; i++)
            {
                sum += PayRecords[i].PayNum;
            }

            return sum;
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
            //删除所有结束订单
            for (int i = _unFinishRecords.Count - 1; i >= 0; i--)
            {
                if (_unFinishRecords[i].State == ERoomerState.退房 || _unFinishRecords[i].CheckOutDate < DateTime.Now)
                {
                    _unFinishRecords.RemoveAt(i);
                }
            }

            if (_unFinishRecords.Count == 0)
            {
                _state = ERoomState.空闲;
            }
            else
            {
                _unFinishRecords.Sort((p, q) => p.CheckInDate.CompareTo(q.CheckInDate));
                var firstRecord = _unFinishRecords[0];
                if (firstRecord.CheckOutDate.GetDays() == DateTime.Now.GetDays())
                {
                    _state = ERoomState.今天到期;
                }
                else
                {
                    if (firstRecord.CheckInDate < DateTime.Now)
                    {
                        _state = ERoomState.已入住;
                    }
                    else
                    {
                        _state = ERoomState.已预定;
                    }
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