using System.Collections.Generic;
using UITools;
using UnityEngine;

namespace Sale
{
    public class UPCtrlRecordRoom : UPCtrlRecordBase
    {
        private List<Room> _rooms;

        public override void Open()
        {
            base.Open();
            _rooms = SaleDataManager.Instance.Rooms;
            RefreshView();
        }

        private void RefreshView()
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
    }
}