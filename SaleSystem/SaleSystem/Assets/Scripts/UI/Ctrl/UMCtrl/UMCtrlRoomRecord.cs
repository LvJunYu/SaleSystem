using UITools;

namespace Sale
{
    public class UMCtrlRoomRecord : UMCtrlGenericBase<UMViewRoomRecord>, IDataItemRenderer
    {
        private RoomRecordData _record;
        private USCtrlInfo[] _infos;

        protected override void OnViewCreated()
        {
            base.OnViewCreated();
            _cachedView.Btn.onClick.AddListener(Btn);
            var infoViews = _cachedView.GetComponentsInChildren<USViewInfo>(true);
            _infos = new USCtrlInfo[infoViews.Length];
            for (int i = 0; i < infoViews.Length; i++)
            {
                _infos[i] = new USCtrlInfo();
                _infos[i].Init(infoViews[i]);
            }

            _infos[0].SetTitle("订单号：");
            _infos[1].SetTitle("房 间：");
            _infos[2].SetTitle("入住时间：");
            _infos[3].SetTitle("房客姓名：");
            _infos[4].SetTitle("退房时间：");
            _infos[5].SetTitle("状 态：");
        }

        private void RefreshView()
        {
            if (_record == null) return;
            _infos[0].SetContent(_record.Id.ToString());
            _infos[1].SetContent(SaleDataManager.Instance.Rooms[_record.RoomIndex].Name);
            _infos[2].SetContent(_record.CheckInDate.ToShortDateString());
            _infos[3].SetContent(_record.RoommerName);
            _infos[4].SetContent(_record.CheckOutDate.ToShortDateString());
            _infos[5].SetContent(_record.State.ToString());
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
            _record = data as RoomRecordData;
            RefreshView();
        }

        public void Unload()
        {
        }
    }
}