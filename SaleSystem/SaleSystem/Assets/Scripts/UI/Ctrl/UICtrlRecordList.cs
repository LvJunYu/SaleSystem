using System.Collections.Generic;
using UITools;
using UnityEngine;

namespace Sale
{
    [UIAutoSetup]
    public class UICtrlRecordList : UICtrlGenericBase<UIViewRecordList>
    {
        private List<RoomRecordData> _records = new List<RoomRecordData>();

        protected override void OnViewCreated()
        {
            base.OnViewCreated();
            _cachedView.CloseBtn.onClick.AddListener(OnCloseBtn);
            _cachedView.GridDataScroller.Set(OnRefreshItem, OnCreateItem);
            _cachedView.SearchBtn.onClick.AddListener(SearchBtn);
        }

        protected override void OnOpen(object parameter)
        {
            base.OnOpen(parameter);
            RefreshView();
        }

        protected override void OnClose()
        {
            base.OnClose();
            _cachedView.SearchInputField.text = string.Empty;
        }

        protected override void InitGroupId()
        {
            _groupId = (int) EUIGroupType.Pop2;
        }

        private void SearchBtn()
        {
            var content = _cachedView.SearchInputField.text;
            if (string.IsNullOrEmpty(content))
            {
                RefreshView();
            }
            else
            {
                var id = int.Parse(content);
                var records = SaleDataManager.Instance.RoomRecords;
                for (int i = records.Count - 1; i >= 0; i--)
                {
                    var record = _records[i];
                    if (record.Id == id)
                    {
                        _records.Clear();
                        _records.Add(record);
                        _cachedView.GridDataScroller.SetItemCount(_records.Count);
                        return;
                    }
                }

                RefreshView();
                SocialGUIManager.ShowPopupDialogFormat("没有订单号为{0}的订单", id);
            }
        }

        private void RefreshView()
        {
            _records.Clear();
            _records.AddRange(SaleDataManager.Instance.RoomRecords);
            _records.Reverse();
            _cachedView.GridDataScroller.SetItemCount(_records.Count);
        }

        private void OnCloseBtn()
        {
            SocialGUIManager.Instance.CloseUI<UICtrlRecordList>();
        }

        private void OnRefreshItem(IDataItemRenderer item, int index)
        {
            if (_isOpen)
            {
                if (index < _records.Count)
                {
                    item.Set(_records[index]);
                }
            }
        }

        private IDataItemRenderer OnCreateItem(RectTransform arg1)
        {
            var item = new UMCtrlRoomRecord();
            item.Init(arg1);
            return item;
        }
    }
}