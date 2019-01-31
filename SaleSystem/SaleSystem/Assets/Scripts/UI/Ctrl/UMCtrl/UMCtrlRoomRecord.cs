using UITools;
using UnityEngine.UI;

namespace Sale
{
    public class UMCtrlRoomRecord : UMCtrlGenericBase<UMViewRoomRecord>, IDataItemRenderer
    {
        private RoomRecord _record;
        private Text[] _infos;

        protected override void OnViewCreated()
        {
            base.OnViewCreated();
            _cachedView.Btn.onClick.AddListener(Btn);
            _infos = _cachedView.GetComponentsInChildren<Text>(true);
        }

        private void RefreshView()
        {
            if (_record == null) return;
            _infos[0].text = (_record.Id.ToString());
            _infos[1].text = (SaleDataManager.Instance.Rooms[_record.RoomIndex].Name);
            _infos[2].text = (_record.RoommerName);
            _infos[3].text = (_record.CheckInDate.GetDateStr());
            _infos[4].text = (_record.CheckOutDate.GetDateStr());
            _infos[5].text = _record.GetPayCount().ToString();
            _infos[6].text = (_record.State.ToString());
        }

        private void Btn()
        {
            SocialGUIManager.Instance.OpenUI<UICtrlUpdateRecord>(_record);
        }

        public int Index { get; set; }

        public object Data
        {
            get { return _record; }
        }

        public void Set(object data)
        {
            _record = data as RoomRecord;
            RefreshView();
        }

        public void Unload()
        {
        }
    }
}