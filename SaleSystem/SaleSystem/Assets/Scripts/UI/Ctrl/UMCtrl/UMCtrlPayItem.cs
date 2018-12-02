using System;
using MyTools;
using UITools;

namespace Sale
{
    public class UMCtrlPayItem : UMCtrlGenericBase<UMViewPayItem>
    {
        private PayRecord _payRecord;
        private bool _isDirty;

        public bool IsDirty
        {
            get { return _isDirty; }
            set { _isDirty = value; }
        }

        public bool IsActive
        {
            get { return _cachedView.isActiveAndEnabled; }
        }

        protected override void OnViewCreated()
        {
            base.OnViewCreated();
            _cachedView.ContentInputField.onEndEdit.AddListener(ContentInputField);
            _cachedView.Dropdown.onValueChanged.AddListener(OnPayTypeChanged);
        }

        private void OnPayTypeChanged(int arg0)
        {
            _isDirty = true;
        }

        private void ContentInputField(string arg0)
        {
            if (string.IsNullOrEmpty(arg0))
            {
                _cachedView.ContentInputField.text = "0";
            }
            else
            {
                _isDirty = _payRecord.PayNum.ToString() != arg0;
            }
        }

        public void SetActive(bool b)
        {
            _cachedView.SetActiveEx(b);
        }

        public void SetData(PayRecord payRecord)
        {
            _payRecord = payRecord;
            RefreshView();
        }

        private void RefreshView()
        {
            var payTypes = SaleDataManager.Instance.PayTypes;
            _cachedView.Dropdown.ClearOptions();
            _cachedView.Dropdown.AddOptions(payTypes);
            var index = payTypes.IndexOf(_payRecord.PayType);
            if (index < 0)
            {
                index = 0;
            }

            _cachedView.Dropdown.value = index;
            _cachedView.ContentInputField.text = _payRecord.PayNum.ToString();
            _isDirty = false;
        }

        public PayRecord GetData()
        {
            if (_isDirty)
            {
                _payRecord.PayTime = DateTime.Now;
                _payRecord.PayNum = int.Parse(_cachedView.ContentInputField.text);
                _payRecord.PayType = SaleDataManager.Instance.PayTypes[_cachedView.Dropdown.value];
                _isDirty = false;
            }

            return _payRecord;
        }
    }
}