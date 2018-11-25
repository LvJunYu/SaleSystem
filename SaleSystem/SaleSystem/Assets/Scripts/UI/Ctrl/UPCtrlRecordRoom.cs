using System.Collections.Generic;
using UITools;
using UnityEngine;

namespace Sale
{
    public class UPCtrlRecordRoom : UPCtrlRecordBase
    {
        private List<Room> _rooms;

        protected override void OnViewCreated()
        {
            base.OnViewCreated();
            _cachedView.CreateRoomRecordBtn.onClick.AddListener(CreateRoomRecordBtn);
            _cachedView.UpdateRoomRecordBtn.onClick.AddListener(UpdateRoomRecordBtn);
            _cachedView.ChangeRoomBtn.onClick.AddListener(ChangeRoomBtn);
        }

        private void ChangeRoomBtn()
        {
            SocialGUIManager.Instance.OpenUI<UICtrlChangeRoom>();
        }

        private void UpdateRoomRecordBtn()
        {
            SocialGUIManager.ShowPopupDialog("开发中");
        }

        private void CreateRoomRecordBtn()
        {
            SocialGUIManager.Instance.OpenUI<UICtrlCreateNew>();
        }

        public override void Open()
        {
            base.Open();
            _rooms = SaleDataManager.Instance.Rooms;
            RefreshView();
        }

        public void RefreshView()
        {
            _cachedView.GridDataScrollers[(int) Menu].SetItemCount(_rooms.Count);
        }

        protected override IDataItemRenderer ItemFactory(RectTransform parent)
        {
            var item = new UMCtrlRoom();
            item.Init(parent);
            return item;
        }

        protected override void ItemRefresh(IDataItemRenderer item, int index)
        {
            if (_isOpen)
            {
                if (index < _rooms.Count)
                {
                    item.Set(_rooms[index]);
                }
                else
                {
                    item.Unload();
                }
            }
        }

        public void OnRoomRecordChanged()
        {
            _cachedView.GridDataScrollers[(int) Menu].RefreshCurrent();
        }
    }
}