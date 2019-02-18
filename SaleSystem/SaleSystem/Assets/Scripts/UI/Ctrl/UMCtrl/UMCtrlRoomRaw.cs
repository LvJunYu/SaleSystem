using UITools;

namespace Sale
{
    public class UMCtrlRoomRaw : UMCtrlGenericBase<UMViewRoomRaw>
    {
        private Room _room;
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
            _cachedView.PriceInputField.onEndEdit.AddListener(PriceEditEnd);
        }

        private void PriceEditEnd(string arg0)
        {
            var price = _room.Price.ToString();
            if (string.IsNullOrEmpty(arg0))
            {
                _cachedView.PriceInputField.text = price;
            }
            else
            {
                _isDirty = price != arg0;
            }
        }

        private void NameEditEnd(string arg0)
        {
            var name = _room.Name;
            if (string.IsNullOrEmpty(arg0))
            {
                _cachedView.NameInputField.text = name;
            }
            else
            {
                _isDirty = name != arg0;
            }
        }

        public void SetActive(bool b)
        {
            _cachedView.SetActiveEx(b);
        }

        public void SetData(Room room)
        {
            _room = room;
            RefreshView();
        }

        private void RefreshView()
        {
            _cachedView.Title.text = _room.IdStr;
            _cachedView.NameInputField.text = _room.Name;
            _cachedView.PriceInputField.text = _room.Price.ToString();
            _isDirty = false;
        }

        public Room GetRoomData()
        {
            if (_isDirty)
            {
                _room.SetData(_cachedView.NameInputField.text, SaleTools.SafeIntParse(_cachedView.PriceInputField.text));
            }

            return _room;
        }
    }
}