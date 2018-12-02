using UITools;

namespace Sale
{
    public class UMCtrlRoom : UMCtrlGenericBase<UMViewRoom>, IDataItemRenderer
    {
        private USCtrlInfo[] _infos;
        private Room _room;
        private ERoomState _state;

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
        }

        private void Btn()
        {
            SocialGUIManager.Instance.OpenUI<UICtrlRecordList>(_room);
        }

        private void RefreshView()
        {
            if (_room == null) return;
            _room.RefreshState();
            _infos[0].SetInfo("房号：", _room.IdStr);
            _infos[1].SetInfo("名称：", _room.Name);
            _infos[2].SetInfo("状态：", _room.State.ToString());
            _infos[3].SetActive(false);
        }

        public int Index { get; set; }

        public object Data
        {
            get { return _room; }
        }

        public void Set(object data)
        {
            _room = data as Room;
            RefreshView();
        }

        public void Unload()
        {
        }
    }
}