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
        private List<RoomRecord> _records = new List<RoomRecord>();

        public List<Room> Rooms
        {
            get { return _rooms; }
        }

        public List<string> PayTypes
        {
            get { return _payTypeData.PayType; }
        }

        public List<RoomRecord> RoomRecords
        {
            get { return _records; }
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

            _records.Clear();
            _recordsData.RoomRecords.Sort((p, q) => p.Id - q.Id);
            for (int i = 0; i < _recordsData.RoomRecords.Count; i++)
            {
                _records.Add(new RoomRecord(_recordsData.RoomRecords[i]));
            }
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
            if (!UserData.Instance.CheckIdentity(EBehaviorType.SaveRecordData)) return;
            LogHelper.Debug("save sale data");
            DataManager.Instance.SaveData(_recordsData, _version, _saveFilePath);
        }

        public void SaveRoomData()
        {
            if (!UserData.Instance.CheckIdentity(EBehaviorType.ChangeRoomData)) return;
            LogHelper.Debug("save room data");
            DataManager.Instance.SaveData(_roomsData, _version, _roomFilePath);
        }

        public void SavePayTypeData()
        {
            if (!UserData.Instance.CheckIdentity(EBehaviorType.ChangePayType)) return;
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

        public void AddRecord(RoomRecord data)
        {
            data.RecordPhase = ERecordPhase.Valid;
            _records.Add(data);
            _recordsData.RoomRecords.Add(data.GetData());
            _recordsData.RecordIndex++;
            SaveRecordsData();
        }

        public void RemoveRecord(RoomRecord data)
        {
            _records.Remove(data);
            _recordsData.RoomRecords.Remove(_recordsData.RoomRecords.Find(record => record.Id == data.Id));
            SaveRecordsData();
        }

        public void ChangeRecord(RoomRecord data)
        {
            for (int i = _recordsData.RoomRecords.Count - 1; i >= 0; i--)
            {
                var recordData = _recordsData.RoomRecords[i];
                if (recordData.Id == data.Id)
                {
                    data.GetData(_recordsData.RoomRecords[i]);
                    SaveRecordsData();
                    return;
                }
            }
        }

        public void ClearRecordsData()
        {
            DataManager.Instance.ClearData(_saveFilePath);
            LoadRecordsData();
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