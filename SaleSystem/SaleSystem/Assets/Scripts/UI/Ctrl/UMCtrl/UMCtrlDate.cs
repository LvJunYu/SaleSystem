using System;
using UITools;

namespace Sale
{
    public class UMCtrlDate : UMCtrlGenericBase<UMViewDate>
    {
        private DateTime _dateTime;

        protected override void OnViewCreated()
        {
            base.OnViewCreated();
            _cachedView.Year.onEndEdit.AddListener(YearValChanged);
            _cachedView.Month.onEndEdit.AddListener(MonthValChanged);
            _cachedView.Day.onEndEdit.AddListener(DayValChanged);
        }

        private void DayValChanged(string arg0)
        {
            if (string.IsNullOrEmpty(arg0) || !DateTimeHelper.CheckDayValid(int.Parse(arg0)))
            {
                _cachedView.Day.text = _dateTime.Day.ToString();
            }
        }

        private void MonthValChanged(string arg0)
        {
            if (string.IsNullOrEmpty(arg0) || !DateTimeHelper.CheckMonthValid(int.Parse(arg0)))
            {
                _cachedView.Month.text = _dateTime.Month.ToString();
            }
        }

        private void YearValChanged(string arg0)
        {
            if (string.IsNullOrEmpty(arg0) || !DateTimeHelper.CheckYearValid(int.Parse(arg0)))
            {
                _cachedView.Year.text = _dateTime.Year.ToString();
            }
        }

        public void SetTitle(string title)
        {
            _cachedView.Title.text = title;
        }

        public void SetDate(DateTime dateTime)
        {
            _dateTime = dateTime;
            _cachedView.Year.text = dateTime.Year.ToString();
            _cachedView.Month.text = dateTime.Month.ToString();
            _cachedView.Day.text = dateTime.Day.ToString();
        }

        public DateTime GetDateTime()
        {
            return new DateTime(SaleTools.SafeParse(_cachedView.Year.text), SaleTools.SafeParse(_cachedView.Month.text),
                SaleTools.SafeParse(_cachedView.Day.text), SaleConstDefine.DefaultRecordHour, 0, 0);
        }
    }
}