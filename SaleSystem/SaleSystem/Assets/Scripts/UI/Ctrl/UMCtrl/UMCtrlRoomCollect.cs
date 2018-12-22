using UITools;

namespace Sale
{
    public class UMCtrlRoomCollect : UMCtrlGenericBase<UMViewRoom>
    {
        private USCtrlInfo[] _infos;
        private RoomMonthData _data;

        protected override void OnViewCreated()
        {
            base.OnViewCreated();
            _cachedView.Btn.interactable = false;
            var infoViews = _cachedView.GetComponentsInChildren<USViewInfo>(true);
            _infos = new USCtrlInfo[infoViews.Length];
            for (int i = 0; i < infoViews.Length; i++)
            {
                _infos[i] = new USCtrlInfo();
                _infos[i].Init(infoViews[i]);
            }
        }

        private void RefreshView()
        {
            if (_data == null) return;
            var room = SaleDataManager.Instance.Rooms[_data.Index];
            _infos[0].SetInfo("房 号：", room.IdStr);
            _infos[1].SetInfo("名 称：", room.Name);
            _infos[2].SetInfo("入住天数：", _data.UseDays.Count.ToString());
            _infos[3].SetInfo("总收入：", _data.TotalIncome.ToString());
        }

        public void SetData(RoomMonthData roomMonthData)
        {
            _data = roomMonthData;
            RefreshView();
        }
    }
}