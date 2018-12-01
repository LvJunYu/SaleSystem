using System;
using System.Collections.Generic;
using MyTools;

namespace Sale
{
    public class SaleDataManager : Singleton<SaleDataManager>
    {
        private readonly Version _version = new Version(1, 0);
        private SaleData _data;
        private int _recordIndex;
        private List<Room> _rooms = new List<Room>();
        private List<string> _payTypes;
        private List<RoomRecordData> _roomRecords;
        private bool _isDirty;

        public List<Room> Rooms
        {
            get { return _rooms; }
        }

        public List<RoomRecordData> RoomRecords
        {
            get { return _roomRecords; }
        }

        public int RecordIndex
        {
            get { return _recordIndex; }
        }

        public List<string> PayTypes
        {
            get { return _payTypes; }
            set
            {
                _payTypes = value;
                _isDirty = true;
            }
        }

        public void LoadData()
        {
//            DataManager.Instance.ClearData();
            _data = DataManager.Instance.LoadData<SaleData>();
            if (_data == null)
            {
                _data = InitDta();
            }

            ParseData(_data);
        }

        public void SaveData()
        {
            if (_isDirty)
            {
                _data = GetData();
                DataManager.Instance.SaveData(_data, _version);
                _isDirty = false;
            }
        }

        private SaleData InitDta()
        {
            var data = new SaleData();
            data.PayType.Add("微信");
            data.PayType.Add("支付宝");
            data.PayType.Add("现金");
            data.RecordIndex = 1;
            data.Rooms.Add(new RoomData("客房1"));
            data.Rooms.Add(new RoomData("客房2"));
            data.Rooms.Add(new RoomData("客房3"));
            data.Rooms.Add(new RoomData("客房4"));
            return data;
        }

        private void ParseData(SaleData data)
        {
            _recordIndex = data.RecordIndex;
            _roomRecords = data.RoomRecords;
            _payTypes = data.PayType;
            _roomRecords.Sort((p, q) => p.Id - q.Id);
            _rooms.Clear();
            for (int i = 0; i < data.Rooms.Count; i++)
            {
                var room = new Room(i);
                room.SetData(data.Rooms[i]);
                _rooms.Add(room);
            }

            RefreshRoomRecords();
        }

        public void RefreshRoomRecords()
        {
            for (int i = 0; i < _rooms.Count; i++)
            {
                _rooms[i].ClearRecords();
            }

            for (int i = 0; i < _roomRecords.Count; i++)
            {
                var roomIndex = _roomRecords[i].RoomIndex;
                if (roomIndex < _rooms.Count)
                {
                    _rooms[roomIndex].AddRecord(_roomRecords[i]);
                }
            }
        }

        private SaleData GetData()
        {
            if (_data == null)
            {
                _data = new SaleData();
            }

            _data.RecordIndex = _recordIndex;
            _data.PayType = _payTypes;
            _data.Rooms.Clear();
            for (int i = 0; i < _rooms.Count; i++)
            {
                _data.Rooms.Add(_rooms[i].GetData());
            }

            return _data;
        }

        public void AddRoomRecord(RoomRecordData data)
        {
            _roomRecords.Add(data);
            _recordIndex++;
            _isDirty = true;
        }
    }

    public class SaleData : DataBase
    {
        public int RecordIndex;
        public List<RoomData> Rooms = new List<RoomData>();
        public List<string> PayType = new List<string>();
        public List<RoomRecordData> RoomRecords = new List<RoomRecordData>();
    }
}