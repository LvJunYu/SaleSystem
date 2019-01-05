using System;
using System.Collections.Generic;
using MyTools;

namespace Sale
{
    public class SaleDataManager : Singleton<SaleDataManager>
    {
        private DataLoadHandler _dataLoadHandler = new DataLoadHandler();
        private DataCollectHandler _dataCollectHandler = new DataCollectHandler();

        public List<Room> Rooms
        {
            get { return _dataLoadHandler.Rooms; }
        }

        public List<RoomRecordData> RoomRecords
        {
            get { return _dataLoadHandler.RoomRecords; }
        }

        public List<string> PayTypes
        {
            get { return _dataLoadHandler.PayTypes; }
        }

        public DataCollectHandler CollectHandler
        {
            get { return _dataCollectHandler; }
        }

        public int RecordIndex
        {
            get { return _dataLoadHandler.RecordIndex; }
        }

        public void Init()
        {
            _dataLoadHandler.LoadData();
            _dataCollectHandler.Init(RoomRecords);
            RefreshRoomRecords();
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
            _dataLoadHandler.ChangePayTypes(payTypes);
        }

        public void ChangeRooms()
        {
            _dataLoadHandler.ChangeRooms();
        }

        public void AddRoomRecord(RoomRecordData data)
        {
            Rooms[data.RoomIndex].AddRecord(data);
            RoomRecords.Add(data);
            _dataCollectHandler.AddRecord(data);
            _dataLoadHandler.RecordIndex++;
            _dataLoadHandler.SaveRecordsData();
        }

        public void RemoveRoomRecord(RoomRecordData data)
        {
            RoomRecords.Remove(data);
            _dataCollectHandler.RemoveRecord(data);
            _dataLoadHandler.SaveRecordsData();
        }

        public void ChangeRecord(RoomRecordData data, int oldRoomIndex, DateTime oldCheckInDate,
            DateTime oldCheckOutDate, List<PayRecord> oldPayRecords)
        {
            if (oldRoomIndex != data.RoomIndex)
            {
                Rooms[oldRoomIndex].RemoveRecord(data);
                Rooms[data.RoomIndex].AddRecord(data);
            }

            if (oldCheckInDate.GetDays() != data.CheckInDate.GetDays() || oldCheckOutDate.GetDays() != data.CheckOutDate.GetDays())
            {
                _dataCollectHandler.ChangeDate(data, oldCheckInDate, oldCheckOutDate);
            }

            if (oldPayRecords != data.PayRecords)
            {
                _dataCollectHandler.ChangePayRecord(data, oldPayRecords);
            }

            _dataLoadHandler.SaveRecordsData();
        }
    }
}