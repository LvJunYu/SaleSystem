using System;
using System.Collections.Generic;
using MyTools;

namespace Sale
{
    public class DataLoadHandler
    {
        private readonly Version _version = new Version(1, 0);
        private List<Room> _rooms = new List<Room>();

        public List<Room> Rooms
        {
            get { return _rooms; }
        }

        public SaleData LoadData()
        {
            LogHelper.Info("LoadData");
//            DataManager.Instance.ClearData();
            var data = DataManager.Instance.LoadData<SaleData>();
            if (data == null)
            {
                data = InitDta();
            }

            ParseData(data);
            return data;
        }

        public void SaveData(SaleData data)
        {
            if (!UserData.Instance.CheckIdentity()) return;
            LogHelper.Info("SaveData");
            DataManager.Instance.SaveData(data, _version);
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
            data.RoomRecords.Sort((p, q) => p.Id - q.Id);
            _rooms.Clear();
            for (int i = 0; i < data.Rooms.Count; i++)
            {
                var room = new Room(i);
                room.SetData(data.Rooms[i]);
                _rooms.Add(room);
            }
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