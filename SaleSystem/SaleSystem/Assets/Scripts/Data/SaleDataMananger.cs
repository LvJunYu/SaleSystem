using System;
using System.Collections.Generic;
using MyTools;

namespace Sale
{
    public class SaleDataManager : Singleton<SaleDataManager>
    {
        private SaleData _data;
        private DataLoadHandler _dataLoadHandler = new DataLoadHandler();
        private DataCollectHandler _dataCollectHandler = new DataCollectHandler();

        public List<Room> Rooms
        {
            get { return _dataLoadHandler.Rooms; }
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
        }

        public DataCollectHandler CollectHandler
        {
            get { return _dataCollectHandler; }
        }

        public void LoadData()
        {
            _data = _dataLoadHandler.LoadData();
            _dataCollectHandler.Init(RoomRecords);
            RefreshRoomRecords();
        }

        public void SaveData()
        {
            _dataLoadHandler.SaveData(_data);
        }

        public void RefreshRoomRecords()
        {
            for (int i = 0; i < Rooms.Count; i++)
            {
                Rooms[i].ClearRecords();
            }

            for (int i = 0; i < RoomRecords.Count; i++)
            {
                var roomIndex = RoomRecords[i].RoomIndex;
                if (roomIndex < Rooms.Count)
                {
                    Rooms[roomIndex].AddRecord(RoomRecords[i]);
                }
            }
        }

        public void ChangePayTypes(List<string> payTypes)
        {
            _data.PayType = payTypes;
            SaveData();
        }

        public void ChangeRooms()
        {
            _data.Rooms.Clear();
            for (int i = 0; i < Rooms.Count; i++)
            {
                _data.Rooms.Add(Rooms[i].GetData());
            }

            SaveData();
        }

        public void AddRoomRecord(RoomRecordData data)
        {
            RoomRecords.Add(data);
            _dataCollectHandler.AddRecord(data);
            RecordIndex++;
            SaveData();
        }

        public void RemoveRoomRecord(RoomRecordData data)
        {
            RoomRecords.Remove(data);
            _dataCollectHandler.RemoveRecord(data);
            SaveData();
        }

        public void ChangeRecord(RoomRecordData data, int oldRoomIndex, DateTime oldCheckInDate,
            DateTime oldCheckOutDate)
        {
            if (oldRoomIndex != data.RoomIndex)
            {
                Rooms[oldRoomIndex].RemoveRecord(data);
                Rooms[data.RoomIndex].AddRecord(data);
            }

            if (oldCheckInDate.GetDays() != data.CheckInDate.GetDays())
            {
                _dataCollectHandler.ChangeCheckInDate(data, oldCheckInDate);
            }

            if (oldCheckOutDate.GetDays() != data.CheckOutDate.GetDays())
            {
                _dataCollectHandler.ChangeCheckOutDate(data, oldCheckOutDate);
            }

            SaveData();
        }
    }
}