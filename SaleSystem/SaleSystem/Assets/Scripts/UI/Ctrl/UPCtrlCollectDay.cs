using System;
using System.Collections.Generic;
using System.Linq;

namespace Sale
{
    public class UPCtrlCollectDay : UPCtrlCollectBase
    {
        private List<int> _dayNum = new List<int>(31);
        private List<string> _dayStr = new List<string>(31);
        private UMCtrlCollectData _collectCtrl;

        protected override void OnViewCreated()
        {
            base.OnViewCreated();
            _collectCtrl = new UMCtrlCollectData();
            _collectCtrl.Init(_cachedView.Pannels[Menu]);
            _collectCtrl.SetLineName("日期", "收入");
        }

        public override void RefreshView()
        {
            var curDate = _mainCtrl.CurDate;
            var monthPayData = SaleDataManager.Instance.CollectHandler.GetMonthPayData(curDate.GetMonths());
            var days = DateTime.DaysInMonth(curDate.Year, curDate.Month);
            SetDays(days);
            if (monthPayData != null)
            {
                for (int i = 0; i < days; i++)
                {
                    var payData = monthPayData.GetDayPayData(i + 1);
                    if (payData != null)
                    {
                        foreach (var payRecord in payData)
                        {
                            _dayNum[i] += payRecord.PayNum;
                        }
                    }
                }
            }

            var max = _dayNum.Max();
            if (max == 0)
            {
                max = 100;
            }

            _collectCtrl.SetData(_dayStr, _dayNum, max);
//            _collectCtrl.SetRaw(_dayStr.Count);
        }

        private void SetDays(int days)
        {
            for (int i = _dayStr.Count; i < days; i++)
            {
                _dayStr.Add((i + 1).ToString());
            }

            for (int i = 0; i < days; i++)
            {
                if (i < _dayNum.Count)
                {
                    _dayNum[i] = 0;
                }
                else
                {
                    _dayNum.Add(0);
                }
            }

            while (_dayStr.Count > days)
            {
                _dayStr.RemoveAt(_dayStr.Count - 1);
            }

            while (_dayNum.Count > days)
            {
                _dayNum.RemoveAt(_dayNum.Count - 1);
            }
        }
    }
}