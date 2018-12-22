using System;
using System.Collections.Generic;
using MyTools;

namespace Sale
{
    public class DataCollectHandler
    {
        private Dictionary<int, HashSet<RoomRecordData>> _monthData = new Dictionary<int, HashSet<RoomRecordData>>();
        private Dictionary<int, HashSet<PayRecord>> _dayData = new Dictionary<int, HashSet<PayRecord>>();
        private List<DayData> _collectDatas;
        private List<RoomMonthData> _roomData = new List<RoomMonthData>();
        private bool _isDirty;
        private int _minDate;
        private int _minMonth;

        public List<DayData> CollectDatas
        {
            get
            {
                if (_collectDatas == null || _isDirty)
                {
                    ProcessDayDatas();
                }

                return _collectDatas;
            }
        }

        public void Init(List<RoomRecordData> roomRecords)
        {
            Clear();
            foreach (var record in roomRecords)
            {
                AddRecord(record);
            }
        }

        public void AddPayRecord(PayRecord pay)
        {
            _isDirty = true;
            var days = pay.PayTime.GetDays();
            if (_minDate > days)
            {
                _minDate = days;
            }

            var list = GetDayData(days, true);
            if (!list.Contains(pay))
            {
                list.Add(pay);
            }
        }

        public void RemovePayRecord(PayRecord pay)
        {
            _isDirty = true;
            var days = pay.PayTime.GetDays();
            GetDayData(days).Remove(pay);
        }

        private void AddMonthData(RoomRecordData data, int month)
        {
            var monthData = GetMonthData(month, true);
            if (!monthData.Contains(data))
            {
                monthData.Add(data);
            }
        }

        public void AddRecord(RoomRecordData data)
        {
            _isDirty = true;
            var startMonth = data.CheckInDate.GetMonths();
            AddMonthData(data, startMonth);
            var endMonth = data.CheckOutDate.AddDays(-1).GetMonths();
            if (endMonth != startMonth)
            {
                AddMonthData(data, endMonth);
            }

            foreach (var payRecord in data.PayRecords)
            {
                AddPayRecord(payRecord);
            }
        }

        public void RemoveRecord(RoomRecordData data)
        {
            _isDirty = true;
            foreach (var payRecord in data.PayRecords)
            {
                RemovePayRecord(payRecord);
            }

            var startMonth = data.CheckInDate.GetMonths();
            GetMonthData(startMonth).Remove(data);
            var endMonth = data.CheckOutDate.AddDays(-1).GetMonths();
            if (endMonth != startMonth)
            {
                GetMonthData(endMonth).Remove(data);
            }
        }

        public void ChangeCheckInDate(RoomRecordData data, DateTime oldCheckInDate)
        {
            _isDirty = true;
            var oldMonth = oldCheckInDate.GetMonths();
            var newMonth = data.CheckInDate.GetMonths();
            if (newMonth != oldMonth)
            {
                GetMonthData(oldMonth).Remove(data);
                AddMonthData(data, newMonth);
            }
        }

        public void ChangeCheckOutDate(RoomRecordData data, DateTime oldCheckOutDate)
        {
            _isDirty = true;
            var oldMonth = oldCheckOutDate.AddDays(-1).GetMonths();
            var newMonth = data.CheckOutDate.AddDays(-1).GetMonths();
            if (newMonth != oldMonth)
            {
                var list = GetMonthData(oldMonth);
                if (list != null)
                {
                    list.Remove(data);
                }

                AddMonthData(data, newMonth);
            }
        }

        private HashSet<RoomRecordData> GetMonthData(int month, bool create = false)
        {
            HashSet<RoomRecordData> records;
            if (_monthData.TryGetValue(month, out records))
            {
                return records;
            }

            if (create)
            {
                records = new HashSet<RoomRecordData>();
                _monthData.Add(month, records);
            }

            return records;
        }

        private HashSet<PayRecord> GetDayData(int day, bool create = false)
        {
            HashSet<PayRecord> pays;
            if (_dayData.TryGetValue(day, out pays))
            {
                return pays;
            }

            if (create)
            {
                pays = new HashSet<PayRecord>();
                _dayData.Add(day, pays);
            }

            return pays;
        }

        private void ProcessDayDatas()
        {
            if (_collectDatas == null)
            {
                _collectDatas = new List<DayData>();
            }
            else
            {
                _collectDatas.Clear();
            }

            var now = DateTime.Now.GetDays();
            for (int i = now; i >= _minDate; i--)
            {
                HashSet<PayRecord> list;
                _dayData.TryGetValue(i, out list);
                _collectDatas.Add(new DayData(i, list));
            }
        }

        private void Clear()
        {
            _minDate = int.MaxValue;
            _monthData.Clear();
            _dayData.Clear();
        }

        public List<RoomMonthData> GetRoomMonthData(int curMonth)
        {
            if (SaleDataManager.Instance.Rooms.Count != _roomData.Count)
            {
                RefreshRoomData();
            }

            foreach (var monthData in _roomData)
            {
                monthData.Clear();
                monthData.SetCurMonth(curMonth);
            }

            var data = GetMonthData(curMonth);
            if (data != null)
            {
                foreach (var roomRecordData in data)
                {
                    var roomIndex = roomRecordData.RoomIndex;
                    if (roomIndex < _roomData.Count)
                    {
                        _roomData[roomIndex].AddData(roomRecordData);
                    }
                    else
                    {
                        LogHelper.Warning("roomIndex {0} is out of range", roomIndex);
                    }
                }
            }

            return _roomData;
        }

        private void RefreshRoomData()
        {
            var rooms = SaleDataManager.Instance.Rooms;
            for (int i = 0; i < rooms.Count; i++)
            {
                if (i == _roomData.Count)
                {
                    _roomData.Add(new RoomMonthData(i));
                }
            }

            for (int i = _roomData.Count - 1; i >= rooms.Count; i--)
            {
                _roomData.RemoveAt(i);
            }
        }
    }
}