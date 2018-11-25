using UITools;
using UnityEngine;

namespace Sale
{
    public abstract class UPCtrlRecordBase : UPCtrlBase<UICtrlRecord, UIViewRecord>
    {
        public UICtrlRecord.EMenu Menu;

        protected override void OnViewCreated()
        {
            base.OnViewCreated();
            _cachedView.GridDataScrollers[(int)Menu].Set(ItemRefresh, ItemFactory);
        }

        protected abstract IDataItemRenderer ItemFactory(RectTransform parent);
        protected abstract void ItemRefresh(IDataItemRenderer item, int index);
    }
}