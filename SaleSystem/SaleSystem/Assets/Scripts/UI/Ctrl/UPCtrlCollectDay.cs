using System.Collections.Generic;
using UITools;
using UnityEngine;
using UnityEngine.UI;

namespace Sale
{
    public class UPCtrlCollectDay : UPCtrlCollectBase
    {
        private List<DayData> _collectDatas;

        protected override void OnViewCreated()
        {
            base.OnViewCreated();
            _cachedView.DayGridDataScroller.Set(OnRefreshItem, OnCreateItem);
            InitPayTypes();
        }

        public override void Open()
        {
            base.Open();
            RefreshView();
        }

        public void InitPayTypes()
        {
            var payTypes = SaleDataManager.Instance.PayTypes;
            var texts = _cachedView.PayTypeDock.GetComponentsInChildren<Text>(true);
            for (int i = 0; i < texts.Length; i++)
            {
                if (i < payTypes.Count)
                {
                    texts[i].text = payTypes[i];
                    texts[i].SetActiveEx(true);
                }
                else
                {
                    texts[i].SetActiveEx(false);
                }
            }
        }

        private void RefreshView()
        {
            _collectDatas = SaleDataManager.Instance.CollectHandler.CollectDatas;
            _cachedView.DayGridDataScroller.SetItemCount(_collectDatas.Count);
        }

        private IDataItemRenderer OnCreateItem(RectTransform arg1)
        {
            var item = new UMCtrlCollectItem();
            item.Init(arg1);
            return item;
        }

        private void OnRefreshItem(IDataItemRenderer item, int index)
        {
            if (_isOpen)
            {
                if (index < _collectDatas.Count)
                {
                    item.Set(_collectDatas[index]);
                }
            }
        }
    }
    
    public class DayData
    {
        public int Days;
        public HashSet<PayRecord> PayRecords;

        public DayData(int days, HashSet<PayRecord> payRecords)
        {
            Days = days;
            PayRecords = payRecords;
        }
    }
}