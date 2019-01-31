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

        public List<RoomRecord> RoomRecords
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

        public void AddRoomRecord(RoomRecord data)
        {
            Rooms[data.RoomIndex].AddRecord(data);
            _dataCollectHandler.AddRecord(data);
            _dataLoadHandler.AddRecord(data);
        }

        public void RemoveRoomRecord(RoomRecord data)
        {
            _dataCollectHandler.RemoveRecord(data);
            _dataLoadHandler.RemoveRecord(data);
        }

        public void ChangeRecord(RoomRecord data, int oldRoomIndex, DateTime oldCheckInDate,
            DateTime oldCheckOutDate, List<PayRecord> oldPayRecords)
        {
            if (oldRoomIndex != data.RoomIndex)
            {
                Rooms[oldRoomIndex].RemoveRecord(data);
                Rooms[data.RoomIndex].AddRecord(data);
            }

            if (oldCheckInDate.GetDays() != data.CheckInDate.GetDays() ||
                oldCheckOutDate.GetDays() != data.CheckOutDate.GetDays())
            {
                _dataCollectHandler.ChangeDate(data, oldCheckInDate, oldCheckOutDate);
            }

            if (oldPayRecords != data.PayRecords)
            {
                _dataCollectHandler.ChangePayRecord(data, oldPayRecords);
            }

            _dataLoadHandler.ChangeRecord(data);
        }

        public void ClearRecordsData()
        {
            _dataLoadHandler.ClearRecordsData();
            _dataCollectHandler.Clear();
            SocialGUIManager.ShowPopupDialog("房间订单数据已清空。");
        }
    }
}