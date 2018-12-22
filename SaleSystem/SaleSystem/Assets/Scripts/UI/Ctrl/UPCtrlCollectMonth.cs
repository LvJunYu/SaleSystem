using System;
using System.Collections.Generic;

namespace Sale
{
    public class UPCtrlCollectMonth : UPCtrlCollectBase
    {
        private List<UMCtrlRoomCollect> _items = new List<UMCtrlRoomCollect>();
        private DateTime _curDate;

        protected override void OnViewCreated()
        {
            base.OnViewCreated();
            _cachedView.LeftBtn.onClick.AddListener(OnPreMonthBtn);
            _cachedView.RightBtn.onClick.AddListener(OnNextMonthBtn);
        }

        public override void Open()
        {
            base.Open();
            _curDate = DateTime.Now;
            RefreshView();
        }

        private void RefreshView()
        {
            _cachedView.YearTxt.text = _curDate.Year.ToString();
            _cachedView.MonthTxt.text = _curDate.Month.ToString();
            var data = SaleDataManager.Instance.CollectHandler.GetRoomMonthData(_curDate.GetMonths());
            var rooms = SaleDataManager.Instance.Rooms;
            for (int i = 0; i < rooms.Count; i++)
            {
                if (i == _items.Count)
                {
                    _items.Add(CreateItem());
                }

                var item = _items[i];
                item.SetData(data[i]);
            }
        }

        private UMCtrlRoomCollect CreateItem()
        {
            var item = new UMCtrlRoomCollect();
            item.Init(_cachedView.RoomDock);
            return item;
        }

        private void OnPreMonthBtn()
        {
            _curDate = _curDate.AddMonths(-1);
            RefreshView();
        }

        private void OnNextMonthBtn()
        {
            _curDate = _curDate.AddMonths(1);
            RefreshView();
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

        public void AddData(RoomRecordData roomRecordData)
        {
            var endDays = roomRecordData.CheckOutDate.GetDays();
            var date = roomRecordData.CheckInDate;
            while (date.GetDays() < endDays)
            {
                var month = date.GetMonths();
                if (month == CurMonth)
                {
                    AddDay(date.Day);
                }

                date = date.AddDays(1);
            }

            var payRecords = roomRecordData.PayRecords;
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