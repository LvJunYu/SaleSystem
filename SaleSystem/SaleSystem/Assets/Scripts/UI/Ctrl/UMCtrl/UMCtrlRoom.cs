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
            if (_room == null) return;
            _infos[0].SetInfo("房号：", (_room.Index + 1).ToString());
            _infos[1].SetInfo("名称：", _room.Name);
            _infos[2].SetInfo("状态：", "正常");
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

        private enum ERoomState
        {
            Normal,
            Use
        }
    }
}