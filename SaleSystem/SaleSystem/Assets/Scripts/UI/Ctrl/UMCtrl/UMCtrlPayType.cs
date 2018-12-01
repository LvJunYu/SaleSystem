using MyTools;
using UITools;

namespace Sale
{
    public class UMCtrlPayType : UMCtrlGenericBase<UMViewPayType>
    {
        private string _payType;
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
            _cachedView.NameInputField.onEndEdit.AddListener(NameEditEnd);
        }

        private void NameEditEnd(string arg0)
        {
            if (string.IsNullOrEmpty(arg0))
            {
                _cachedView.NameInputField.text = _payType;
            }
            else
            {
                _isDirty = _payType != arg0;
            }
        }

        public void SetActive(bool b)
        {
            _cachedView.SetActiveEx(b);
        }

        public void SetData(string payType)
        {
            _payType = payType;
            _cachedView.NameInputField.text = payType;
        }

        public string GetData()
        {
            return _cachedView.NameInputField.text;
        }
    }
}