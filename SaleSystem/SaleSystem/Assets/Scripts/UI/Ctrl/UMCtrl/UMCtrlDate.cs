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
            _cachedView.Hour.onEndEdit.AddListener(HourValChanged);
            _cachedView.Minute.onEndEdit.AddListener(MinuteValChanged);
        }

        private void MinuteValChanged(string arg0)
        {
            if (string.IsNullOrEmpty(arg0) || !SaleTools.CheckMinuteValid(int.Parse(arg0)))
            {
                _cachedView.Minute.text = _dateTime.Minute.ToString();
            }
        }

        private void HourValChanged(string arg0)
        {
            if (string.IsNullOrEmpty(arg0) || !SaleTools.CheckHourValid(int.Parse(arg0)))
            {
                _cachedView.Hour.text = _dateTime.Hour.ToString();
            }
        }

        private void DayValChanged(string arg0)
        {
            if (string.IsNullOrEmpty(arg0) || !SaleTools.CheckDayValid(int.Parse(arg0)))
            {
                _cachedView.Day.text = _dateTime.Day.ToString();
            }
        }

        private void MonthValChanged(string arg0)
        {
            if (string.IsNullOrEmpty(arg0) || !SaleTools.CheckMonthValid(int.Parse(arg0)))
            {
                _cachedView.Month.text = _dateTime.Month.ToString();
            }
        }

        private void YearValChanged(string arg0)
        {
            if (string.IsNullOrEmpty(arg0) || !SaleTools.CheckYearValid(int.Parse(arg0)))
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
            _cachedView.Hour.text = dateTime.Hour.ToString();
            _cachedView.Minute.text = dateTime.Minute.ToString();
        }

        public DateTime GetDateTime()
        {
            return new DateTime(SaleTools.SafeIntParse(_cachedView.Year.text), SaleTools.SafeIntParse(_cachedView.Month.text),
                SaleTools.SafeIntParse(_cachedView.Day.text), SaleTools.SafeIntParse(_cachedView.Hour.text),
                SaleTools.SafeIntParse(_cachedView.Minute.text), 0);
        }
    }
}