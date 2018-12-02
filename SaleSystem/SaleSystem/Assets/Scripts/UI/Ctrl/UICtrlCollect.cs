using System;
using System.Collections.Generic;
using MyTools;
using UITools;
using UnityEngine;
using UnityEngine.UI;

namespace Sale
{
    [UIAutoSetup]
    public class UICtrlCollect : UICtrlGenericBase<UIViewCollect>
    {
        private Dictionary<int, List<PayRecord>> _dateDic = new Dictionary<int, List<PayRecord>>();
        private int _minDate;
        private List<CollectData> _collectDatas = new List<CollectData>();

        protected override void OnViewCreated()
        {
            base.OnViewCreated();
            _cachedView.CloseBtn.onClick.AddListener(CloseBtn);
            _cachedView.GridDataScroller.Set(OnRefreshItem, OnCreateItem);
            InitPayTypes();
        }

        protected override void OnOpen(object parameter)
        {
            base.OnOpen(parameter);
            RefreshView();
        }

        protected override void InitEventListener()
        {
            base.InitEventListener();
            RegisterEvent(EMessengerType.OnPayTypeChanged, InitPayTypes);
        }

        protected override void InitGroupId()
        {
            _groupId = (int) EUIGroupType.Pop1;
        }

        private void RefreshView()
        {
            _dateDic.Clear();
            _minDate = int.MaxValue;
            var records = SaleDataManager.Instance.RoomRecords;
            foreach (var record in records)
            {
                var payList = record.PayRecords;
                for (var i = 0; i < payList.Count; i++)
                {
                    AddData(payList[i]);
                }
            }

            _collectDatas.Clear();
            var now = DateTime.Now.GetDays();
            for (int i = now; i >= _minDate; i--)
            {
                List<PayRecord> list;
                _dateDic.TryGetValue(i, out list);
                _collectDatas.Add(new CollectData(i, list));
            }

            _cachedView.GridDataScroller.SetItemCount(_collectDatas.Count);
        }

        private void AddData(PayRecord pay)
        {
            var days = pay.PayTime.GetDays();
            if (_minDate > days)
            {
                _minDate = days;
            }

            List<PayRecord> list;
            if (!_dateDic.TryGetValue(days, out list))
            {
                list = new List<PayRecord>();
                _dateDic.Add(days, list);
            }

            list.Add(pay);
        }

        private void CloseBtn()
        {
            SocialGUIManager.Instance.CloseUI<UICtrlCollect>();
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
 
        private void InitPayTypes()
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
    }

    public class CollectData
    {
        public int Days;
        public List<PayRecord> PayRecords;

        public CollectData(int days, List<PayRecord> payRecords)
        {
            Days = days;
            PayRecords = payRecords;
        }
    }
}