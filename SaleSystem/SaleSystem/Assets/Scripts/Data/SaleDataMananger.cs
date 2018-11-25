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
        private List<RoomRecord> _roomRecords = new List<RoomRecord>();
        private bool _isDirty;

        public List<Room> Rooms
        {
            get { return _rooms; }
        }

        public List<RoomRecord> RoomRecords
        {
            get { return _roomRecords; }
        }

        public void LoadData()
        {
            _data = DataManager.Instance.LoadData<SaleData>();
            if (_data == null)
            {
                _data = InitDta();
                _isDirty = true;
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
            data.Rooms.Add(new RoomData("客房1"));
            data.Rooms.Add(new RoomData("客房2"));
            data.Rooms.Add(new RoomData("客房3"));
            data.Rooms.Add(new RoomData("客房4"));
            return data;
        }

        private void ParseData(SaleData data)
        {
            _roomRecords = data.RoomRecords;
            _roomRecords.Sort((p, q) => (p.CheckInDate - q.CheckInDate).Days);

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
                if (_roomRecords[i].RoomIndex < _rooms.Count)
                {
                    _rooms[i].AddRecord(_roomRecords[i]);
                }
            }
        }
  
        private SaleData GetData()
        {
            if (_data == null)
            {
                _data = new SaleData();
            }
            
            _data.Rooms.Clear();
            for (int i = 0; i < _rooms.Count; i++)
            {
                _data.Rooms.Add(_rooms[i].GetData());
            }

            return _data;
        }

        public void AddRoomRecord(RoomRecord data)
        {
            _roomRecords.Add(data);
            _isDirty = true;
        }
    }

    public class SaleData : DataBase
    {
        public List<RoomData> Rooms = new List<RoomData>();
        public List<RoomRecord> RoomRecords = new List<RoomRecord>();
    }
}