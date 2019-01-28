using System;
using System.Collections.Generic;
using MyTools;

namespace Sale
{
    public class DataCollectHandler
    {
        private Dictionary<int, HashSet<RoomRecord>> _monthData = new Dictionary<int, HashSet<RoomRecord>>();
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

        public void Init(List<RoomRecord> roomRecords)
        {
            Clear();
            foreach (var record in roomRecords)
            {
                AddRecord(record);
            }
        }

        private void AddPayRecord(PayRecord pay)
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

        private void RemovePayRecord(PayRecord pay)
        {
            _isDirty = true;
            var days = pay.PayTime.GetDays();
            GetDayData(days).Remove(pay);
        }

        private void AddMonthData(RoomRecord data, int month)
        {
            var monthData = GetMonthData(month, true);
            if (!monthData.Contains(data))
            {
                monthData.Add(data);
            }
        }

        public void AddRecord(RoomRecord data)
        {
            _isDirty = true;
            var startMonth = data.CheckInDate.GetMonths();
            var endMonth = data.CheckOutDate.AddDays(-1).GetMonths();
            for (int i = startMonth; i <= endMonth; i++)
            {
                AddMonthData(data, i);
            }

            foreach (var payRecord in data.PayRecords)
            {
                AddPayRecord(payRecord);
            }
        }

        public void RemoveRecord(RoomRecord data)
        {
            _isDirty = true;
            var startMonth = data.CheckInDate.GetMonths();
            var endMonth = data.CheckOutDate.AddDays(-1).GetMonths();
            for (int i = startMonth; i <= endMonth; i++)
            {
                GetMonthData(i).Remove(data);
            }

            foreach (var payRecord in data.PayRecords)
            {
                RemovePayRecord(payRecord);
            }
        }

        public void ChangeDate(RoomRecord data, DateTime oldCheckInDate, DateTime oldCheckOutDate)
        {
            _isDirty = true;
            var oldStartMonth = oldCheckInDate.GetMonths();
            var oldEndMonth = oldCheckOutDate.AddDays(-1).GetMonths();
            var newStartMonth = data.CheckInDate.GetMonths();
            var newEndMonth = data.CheckOutDate.AddDays(-1).GetMonths();
            if (oldStartMonth != newStartMonth || oldEndMonth != newEndMonth)
            {
                for (int i = oldStartMonth; i <= oldEndMonth; i++)
                {
                    GetMonthData(i).Remove(data);
                }
                
                for (int i = newStartMonth; i <= newEndMonth; i++)
                {
                    AddMonthData(data, i);
                }
            }
        }

        public void ChangePayRecord(RoomRecord data, List<PayRecord> oldPayRecords)
        {
            _isDirty = true;
            foreach (var payRecord in oldPayRecords)
            {
                RemovePayRecord(payRecord);
            }

            foreach (var payRecord in data.PayRecords)
            {
                AddPayRecord(payRecord);
            }
        }

        private HashSet<RoomRecord> GetMonthData(int month, bool create = false)
        {
            HashSet<RoomRecord> records;
            if (_monthData.TryGetValue(month, out records))
            {
                return records;
            }

            if (create)
            {
                records = new HashSet<RoomRecord>();
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