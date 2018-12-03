using System;
using System.Collections.Generic;
using MyTools;

namespace Sale
{
    public class SaleDataManager : Singleton<SaleDataManager>
    {
        private readonly Version _version = new Version(1, 0);
        private SaleData _data;
        private List<Room> _rooms = new List<Room>();

        public List<Room> Rooms
        {
            get { return _rooms; }
        }

        public List<RoomRecordData> RoomRecords
        {
            get { return _data.RoomRecords; }
        }

        public int RecordIndex
        {
            get { return _data.RecordIndex; }
            set { _data.RecordIndex = value; }
        }

        public List<string> PayTypes
        {
            get { return _data.PayType; }
            set
            {
                _data.PayType = value;
                SaveData();
            }
        }

        public void LoadData()
        {
            LogHelper.Info("LoadData");
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
            if (!UserData.Instance.CheckIdentity()) return;
            LogHelper.Info("SaveData");
            DataManager.Instance.SaveData(_data, _version);
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
            RoomRecords.Sort((p, q) => p.Id - q.Id);
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

            for (int i = 0; i < RoomRecords.Count; i++)
            {
                var roomIndex = RoomRecords[i].RoomIndex;
                if (roomIndex < _rooms.Count)
                {
                    _rooms[roomIndex].AddRecord(RoomRecords[i]);
                }
            }
        }

        public void ChangeRooms()
        {
            _data.Rooms.Clear();
            for (int i = 0; i < _rooms.Count; i++)
            {
                _data.Rooms.Add(_rooms[i].GetData());
            }

            SaveData();
        }

        public void AddRoomRecord(RoomRecordData data)
        {
            RoomRecords.Add(data);
            RecordIndex++;
            SaveData();
        }

        public void RemoveRoomRecord(RoomRecordData data)
        {
            RoomRecords.Remove(data);
            SaveData();
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