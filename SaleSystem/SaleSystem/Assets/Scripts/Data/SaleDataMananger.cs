using System;
using System.Collections.Generic;
using MyTools;

namespace Sale
{
    public class SaleDataManager : Singleton<SaleDataManager>
    {
        private readonly Version _version = new Version(1, 0);
        private SaleData _data;
        private List<Room> _rooms;
        private List<RoomRecord> _roomRecords;

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
            }

            ParseData(_data);
        }

        public void SaveData()
        {
            DataManager.Instance.SaveData(_data, _version);
        }

        private SaleData InitDta()
        {
            var data = new SaleData();
            data.RoomNames.Add("客房1");
            data.RoomNames.Add("客房2");
            data.RoomNames.Add("客房3");
            data.RoomNames.Add("客房4");
            return data;
        }

        private void ParseData(SaleData data)
        {
            _roomRecords = data.RoomRecords;
            _roomRecords.Sort((p, q) => (p.CheckInDate - q.CheckInDate).Days);

            _rooms = new List<Room>(data.RoomNames.Count);
            for (int i = 0; i < data.RoomNames.Count; i++)
            {
                var room = new Room(i);
                room.Name = data.RoomNames[i];
                _rooms.Add(room);
            }

            for (int i = 0; i < _roomRecords.Count; i++)
            {
                if (_roomRecords[i].RoomIndex < _rooms.Count)
                {
                    _rooms[i].AddRecord(_roomRecords[i]);
                }
            }
        }
    }

    public class SaleData : DataBase
    {
        public List<string> RoomNames = new List<string>();
        public List<RoomRecord> RoomRecords = new List<RoomRecord>();
    }
}