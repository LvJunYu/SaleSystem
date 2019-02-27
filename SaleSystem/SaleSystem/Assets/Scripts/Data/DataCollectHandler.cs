using System;
using System.Collections.Generic;
using MyTools;

namespace Sale
{
    public class DataCollectHandler
    {
        private Dictionary<int, HashSet<RoomRecord>> _monthRoomRecord = new Dictionary<int, HashSet<RoomRecord>>();
        private Dictionary<int, MonthPayData> _monthPayDatas = new Dictionary<int, MonthPayData>();
        private List<RoomMonthData> _roomData = new List<RoomMonthData>();
        private bool _isDirty;
        private int _minDate;
        private int _minMonth;

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

            GetOrCreateMonthPayData(pay.PayTime).AddPayRecord(pay);
        }

        private MonthPayData GetOrCreateMonthPayData(DateTime date)
        {
            MonthPayData monthPayData;
            if (!_monthPayDatas.TryGetValue(date.GetMonths(), out monthPayData))
            {
                monthPayData = new MonthPayData(date);
                _monthPayDatas.Add(date.GetMonths(), monthPayData);
            }

            return monthPayData;
        }

        private void RemovePayRecord(PayRecord pay)
        {
            _isDirty = true;
            GetOrCreateMonthPayData(pay.PayTime).RemovePayRecord(pay);
        }

        private void AddMonthData(RoomRecord data, int month)
        {
            var monthData = _monthRoomRecord.GetOrCreateValue(month);
            if (!monthData.Contains(data))
            {
                monthData.Add(data);
            }
        }

        public void AddRecord(RoomRecord data)
        {
            _isDirty = true;
            var startMonth = data.CheckInDate.GetMonths();
            var endMonth = data.CheckOutDate.GetMonths();
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
            var endMonth = data.CheckOutDate.GetMonths();
            for (int i = startMonth; i <= endMonth; i++)
            {
                _monthRoomRecord[i].Remove(data);
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
            var oldEndMonth = oldCheckOutDate.GetMonths();
            var newStartMonth = data.CheckInDate.GetMonths();
            var newEndMonth = data.CheckOutDate.GetMonths();
            if (oldStartMonth != newStartMonth || oldEndMonth != newEndMonth)
            {
                for (int i = oldStartMonth; i <= oldEndMonth; i++)
                {
                    _monthRoomRecord[i].Remove(data);
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

        public void Clear()
        {
            _minDate = DateTime.Now.GetDays();
            _monthRoomRecord.Clear();
            _monthPayDatas.Clear();
        }

        public List<RoomMonthData> GetRoomMonthData(int month)
        {
            if (SaleDataManager.Instance.Rooms.Count != _roomData.Count)
            {
                RefreshRoomData();
            }

            foreach (var monthData in _roomData)
            {
                monthData.Clear();
                monthData.SetCurMonth(month);
            }

            var data = _monthRoomRecord.GetOrCreateValue(month);
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

            return _roomData;
        }

        public MonthPayData GetMonthPayData(int month)
        {
            MonthPayData monthPayData;
            _monthPayDatas.TryGetValue(month, out monthPayData);
            return monthPayData;
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

    public class MonthPayData
    {
        public DateTime Date;
        public Dictionary<int, HashSet<PayRecord>> _dayPayData = new Dictionary<int, HashSet<PayRecord>>();
        public HashSet<PayRecord> _payDatas = new HashSet<PayRecord>();

        public MonthPayData(DateTime date)
        {
            Date = date;
        }

        public HashSet<PayRecord> PayDatas
        {
            get { return _payDatas; }
        }

        public void AddPayRecord(PayRecord payRecord)
        {
            _dayPayData.GetOrCreateValue(payRecord.PayTime.Day).Add(payRecord);
            _payDatas.Add(payRecord);
        }

        public void RemovePayRecord(PayRecord payRecord)
        {
            _dayPayData[payRecord.PayTime.Day].Remove(payRecord);
            _payDatas.Remove(payRecord);
        }

        public HashSet<PayRecord> GetDayPayData(int day)
        {
            HashSet<PayRecord> payRecords;
            _dayPayData.TryGetValue(day, out payRecords);
            return payRecords;
        }

        public int GetTotalPay()
        {
            int sum = 0;
            foreach (var pay in _payDatas)
            {
                sum += pay.PayNum;
            }

            return sum;
        }
    }
}