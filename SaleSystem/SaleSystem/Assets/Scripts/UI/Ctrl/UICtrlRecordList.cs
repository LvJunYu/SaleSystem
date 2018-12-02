using System.Collections.Generic;
using MyTools;
using UITools;
using UnityEngine;
using UnityEngine.UI;

namespace Sale
{
    [UIAutoSetup(EUIAutoSetupType.Create)]
    public class UICtrlRecordList : UICtrlAnimationBase<UIViewRecordList>
    {
        private List<RoomRecordData> _records = new List<RoomRecordData>();
        private ESearchType _searchType;
        private string _searchContent;

        private List<string> _searchOptions = new List<string>
        {
            ESearchType.按订单号.ToString(),
            ESearchType.按房间号.ToString(),
            ESearchType.按姓名.ToString(),
        };

        protected override void OnViewCreated()
        {
            base.OnViewCreated();
            _cachedView.CloseBtn.onClick.AddListener(OnCloseBtn);
            _cachedView.AllBtn.onClick.AddListener(RefreshData);
            _cachedView.GridDataScroller.Set(OnRefreshItem, OnCreateItem);
            _cachedView.SearchBtn.onClick.AddListener(SearchBtn);
            _cachedView.Dropdown.ClearOptions();
            _cachedView.Dropdown.AddOptions(_searchOptions);
            _cachedView.Dropdown.onValueChanged.AddListener(DropDownValChanged);
            _cachedView.Dropdown.value = 0;
            _searchType = ESearchType.按订单号;
        }

        protected override void OnOpen(object parameter)
        {
            base.OnOpen(parameter);
            var room = parameter as Room;
            if (room == null)
            {
                RefreshData();
            }
            else
            {
                _cachedView.Dropdown.value = (int) ESearchType.按房间号;
                _searchContent = _cachedView.SearchInputField.text = room.IdStr;
                _records.Clear();
                _records.AddRange(room.Records);
                _records.Reverse();
                RefreshView();
            }
        }

        protected override void OnClose()
        {
            base.OnClose();
            _searchContent = _cachedView.SearchInputField.text = string.Empty;
        }

        protected override void InitEventListener()
        {
            base.InitEventListener();
            RegisterEvent(EMessengerType.OnRoomRecordChanged, OnRoomRecordChanged);
        }

        protected override void InitGroupId()
        {
            _groupId = (int) EUIGroupType.Pop2;
        }

        protected override void SetPartAnimations()
        {
            base.SetPartAnimations();
            SetPart(_cachedView.PannelRtf, EAnimationType.MoveFromDown);
            SetPart(_cachedView.BGRtf, EAnimationType.Fade);
        }

        private void OnRoomRecordChanged()
        {
            if (_isOpen)
            {
                Search();
            }
        }

        private void DropDownValChanged(int arg0)
        {
            _cachedView.SearchInputField.text = string.Empty;
            _searchType = (ESearchType) arg0;
            switch (_searchType)
            {
                case ESearchType.按订单号:
                    _cachedView.GuidTxt.text = "请输入订单号查询";
                    _cachedView.SearchInputField.contentType = InputField.ContentType.IntegerNumber;
                    break;
                case ESearchType.按房间号:
                    _cachedView.GuidTxt.text = "请输入房间号查询";
                    _cachedView.SearchInputField.contentType = InputField.ContentType.IntegerNumber;
                    break;
                case ESearchType.按姓名:
                    _cachedView.GuidTxt.text = "请输入姓名查询";
                    _cachedView.SearchInputField.contentType = InputField.ContentType.Standard;
                    break;
            }
        }

        private void Search()
        {
            if (string.IsNullOrEmpty(_searchContent))
            {
                RefreshData();
            }
            else
            {
                switch (_searchType)
                {
                    case ESearchType.按订单号:
                        SearchByRecordId(int.Parse(_searchContent));
                        break;
                    case ESearchType.按房间号:
                        SearchByRoomId(int.Parse(_searchContent));
                        break;
                    case ESearchType.按姓名:
                        SearchByName(_searchContent);
                        break;
                }
            }
        }

        private void SearchBtn()
        {
            _searchContent = _cachedView.SearchInputField.text;
            Search();
        }

        private void SearchByRecordId(int id)
        {
            var records = SaleDataManager.Instance.RoomRecords;
            for (int i = records.Count - 1; i >= 0; i--)
            {
                var record = records[i];
                if (record.Id == id)
                {
                    _records.Clear();
                    _records.Add(record);
                    RefreshView();
                    return;
                }
            }

            SocialGUIManager.ShowPopupDialogFormat("没有订单号为{0}的订单", id);
        }

        private void SearchByRoomId(int id)
        {
            _records.Clear();
            var records = SaleDataManager.Instance.RoomRecords;
            for (int i = records.Count - 1; i >= 0; i--)
            {
                var record = records[i];
                if (record.RoomIndex + 1 == id)
                {
                    _records.Add(record);
                }
            }

            if (_records.Count == 0)
            {
                SocialGUIManager.ShowPopupDialogFormat("没有房间号为{0}的订单", id);
            }
            else
            {
                RefreshView();
            }
        }

        private void SearchByName(string name)
        {
            _records.Clear();
            var records = SaleDataManager.Instance.RoomRecords;
            for (int i = records.Count - 1; i >= 0; i--)
            {
                var record = records[i];
                if (record.RoommerName == name)
                {
                    _records.Add(record);
                }
            }

            if (_records.Count == 0)
            {
                SocialGUIManager.ShowPopupDialogFormat("没有房客姓名为{0}的订单", name);
            }
            else
            {
                RefreshView();
            }
        }

        private void RefreshData()
        {
            _records.Clear();
            _records.AddRange(SaleDataManager.Instance.RoomRecords);
            _records.Reverse();
            RefreshView();
        }

        private void RefreshView()
        {
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

        private enum ESearchType
        {
            按订单号,
            按房间号,
            按姓名
        }
    }
}