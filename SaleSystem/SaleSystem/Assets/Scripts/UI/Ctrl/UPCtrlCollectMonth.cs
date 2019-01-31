using System;
using System.Collections.Generic;
using UITools;
using UnityEngine;

namespace Sale
{
    public class UPCtrlCollectMonth : UPCtrlCollectBase
    {
        private DateTime _curDate;
        private List<RoomMonthData> _dataList;

        protected override void OnViewCreated()
        {
            base.OnViewCreated();
            _cachedView.MonthGridDataScroller.Set(OnRefreshItem, OnCreateItem);
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
            _dataList = SaleDataManager.Instance.CollectHandler.GetRoomMonthData(_curDate.GetMonths());
            _cachedView.MonthGridDataScroller.SetItemCount(_dataList.Count);
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

        private IDataItemRenderer OnCreateItem(RectTransform arg1)
        {
            var item = new UMCtrlRoomCollect();
            item.Init(arg1);
            return item;
        }

        private void OnRefreshItem(IDataItemRenderer item, int index)
        {
            if (_isOpen)
            {
                if (index < _dataList.Count)
                {
                    item.Set(_dataList[index]);
                }
            }
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