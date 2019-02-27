using System.Collections.Generic;
using System.Linq;

namespace Sale
{
    public class UPCtrlCollectRoom : UPCtrlCollectBase
    {
        private List<int> _roomNum = new List<int>();
        private List<string> _roomName = new List<string>();
        private UMCtrlCollectData _collectCtrl;

        protected override void OnViewCreated()
        {
            base.OnViewCreated();
            _collectCtrl = new UMCtrlCollectData();
            _collectCtrl.Init(_cachedView.Pannels[Menu]);
            _collectCtrl.SetLineName("房间", "收入");
        }

        public override void RefreshView()
        {
            var curDate = _mainCtrl.CurDate;
            var monthPayData = SaleDataManager.Instance.CollectHandler.GetMonthPayData(curDate.GetMonths());
            _roomNum.Clear();
            _roomName.Clear();
            var rooms = SaleDataManager.Instance.Rooms;
            foreach (var room in rooms)
            {
                _roomName.Add(room.Name);
                _roomNum.Add(0);
            }

            foreach (var payRecord in monthPayData.PayDatas)
            {
                var recordId = payRecord.RecordId;
                var record = SaleDataManager.Instance.GetRoomRecord(recordId);
                if (record != null && record.RoomIndex < rooms.Count)
                {
                    _roomNum[record.RoomIndex] += payRecord.PayNum;
                }
            }

            var max = _roomNum.Max();
            if (max == 0)
            {
                max = 100;
            }

            _collectCtrl.SetData(_roomName, _roomNum, max);
        }
    }

    public class RoomMonthData
    {
        public int Index;
        public HashSet<int> UseDays = new HashSet<int>();
        public int TotalIncome;
        public int CurMonth;

        public RoomMonthData(int index)
        {
            Index = index;
        }

        public void SetCurMonth(int curMonth)
        {
            CurMonth = curMonth;
        }

        public void Clear()
        {
            UseDays.Clear();
            TotalIncome = 0;
        }

        public void AddData(RoomRecord roomRecord)
        {
            var endDays = roomRecord.CheckOutDate.GetDays();
            var date = roomRecord.CheckInDate;
            while (date.GetDays() <= endDays)
            {
                var month = date.GetMonths();
                if (month == CurMonth)
                {
                    AddDay(date.Day);
                }

                date = date.AddDays(1);
            }

            var payRecords = roomRecord.PayRecords;
            foreach (var payRecord in payRecords)
            {
                if (payRecord.PayTime.GetMonths() == CurMonth)
                {
                    TotalIncome += payRecord.PayNum;
                }
            }
        }

        private void AddDay(int day)
        {
            if (!UseDays.Contains(day))
            {
                UseDays.Add(day);
            }
        }
    }
}