using System;
using System.Collections.Generic;
using MyTools;
using UnityEngine;

namespace Sale
{
    public class DataLoadHandler
    {
        private readonly Version _version = new Version(1, 0);
        private string _saveFilePath = string.Format("{0}/SaveData", Application.persistentDataPath);
        private string _roomFilePath = string.Format("{0}/Rooms", Application.persistentDataPath);
        private string _payTypeFilePath = string.Format("{0}/PayTypes", Application.persistentDataPath);
        private RecordsData _recordsData;
        private RoomsData _roomsData;
        private PayTypeData _payTypeData;

        private List<Room> _rooms = new List<Room>();

        public List<Room> Rooms
        {
            get { return _rooms; }
        }

        public List<string> PayTypes
        {
            get { return _payTypeData.PayType; }
        }

        public List<RoomRecordData> RoomRecords
        {
            get { return _recordsData.RoomRecords; }
        }

        public int RecordIndex
        {
            get { return _recordsData.RecordIndex; }
            set { _recordsData.RecordIndex = value; }
        }

        public void LoadData()
        {
            LoadRecordsData();
            LoadRoomsData();
            LoadPayTypeData();
        }

        public void LoadRecordsData()
        {
            LogHelper.Debug("load sale data");
            _recordsData = DataManager.Instance.LoadData<RecordsData>(_saveFilePath);
            if (_recordsData == null)
            {
                _recordsData = new RecordsData();
                _recordsData.RecordIndex = 1;
            }

            _recordsData.RoomRecords.Sort((p, q) => p.Id - q.Id);
        }

        public void LoadRoomsData()
        {
            LogHelper.Debug("load rooms data");
            _roomsData = DataManager.Instance.LoadData<RoomsData>(_roomFilePath);
            if (_roomsData == null)
            {
                _roomsData = new RoomsData();
                _roomsData.Rooms.Add(new RoomData("客房1"));
                _roomsData.Rooms.Add(new RoomData("客房2"));
                _roomsData.Rooms.Add(new RoomData("客房3"));
                _roomsData.Rooms.Add(new RoomData("客房4"));
            }

            _rooms.Clear();
            for (int i = 0; i < _roomsData.Rooms.Count; i++)
            {
                var room = new Room(i);
                room.SetData(_roomsData.Rooms[i]);
                _rooms.Add(room);
            }
        }

        public void LoadPayTypeData()
        {
            LogHelper.Debug("load payType data");
            _payTypeData = DataManager.Instance.LoadData<PayTypeData>(_payTypeFilePath);
            if (_payTypeData == null)
            {
                _payTypeData = new PayTypeData();
                _payTypeData.PayType.Add("微信");
                _payTypeData.PayType.Add("支付宝");
                _payTypeData.PayType.Add("现金");
            }
        }

        public void SaveRecordsData()
        {
            if (!UserData.Instance.CheckIdentity()) return;
            LogHelper.Debug("save sale data");
            DataManager.Instance.SaveData(_recordsData, _version, _saveFilePath);
        }

        public void SaveRoomData()
        {
            if (!UserData.Instance.CheckIdentity()) return;
            LogHelper.Debug("save room data");
            DataManager.Instance.SaveData(_roomsData, _version, _roomFilePath);
        }

        public void SavePayTypeData()
        {
            if (!UserData.Instance.CheckIdentity()) return;
            LogHelper.Debug("save pay type data");
            DataManager.Instance.SaveData(_payTypeData, _version, _payTypeFilePath);
        }

        public void ChangePayTypes(List<string> payTypes)
        {
            _payTypeData.PayType = payTypes;
            SavePayTypeData();
        }

        public void ChangeRooms()
        {
            _roomsData.Rooms.Clear();
            for (int i = 0; i < _rooms.Count; i++)
            {
                _roomsData.Rooms.Add(_rooms[i].GetData());
            }

            SaveRoomData();
        }
    }

    public class RecordsData : DataBase
    {
        public int RecordIndex;
        public List<RoomRecordData> RoomRecords = new List<RoomRecordData>();
    }

    public class PayTypeData : DataBase
    {
        public List<string> PayType = new List<string>();
    }

    public class RoomsData : DataBase
    {
        public List<RoomData> Rooms = new List<RoomData>();
    }
}