using System.Collections.Generic;
using MyTools;
using UITools;
using UnityEngine;

namespace Sale
{
    [UIAutoSetup]
    public class UICtrlPay : UICtrlGenericBase<UIViewPay>
    {
        private List<UMCtrlPayItem> _items = new List<UMCtrlPayItem>();
        private int _curPayTypeCount;
        private float _parentHeight;
        private List<PayRecord> _payRecords;

        protected override void InitGroupId()
        {
            _groupId = (int) EUIGroupType.Pop3;
        }

        protected override void OnViewCreated()
        {
            base.OnViewCreated();
            _cachedView.AddBtn.onClick.AddListener(AddBtn);
            _cachedView.DeleteBtn.onClick.AddListener(DeleteBtn);
            _cachedView.OKBtn.onClick.AddListener(OKBtn);
            _cachedView.CancelBtn.onClick.AddListener(CancelBtn);
            _parentHeight = ((RectTransform) _cachedView.Content.parent).GetHeight();
        }

        protected override void OnOpen(object parameter)
        {
            base.OnOpen(parameter);
            _payRecords = parameter as List<PayRecord>;
            RefreshView();
        }

        private void RefreshView()
        {
            _curPayTypeCount = _payRecords.Count;
            for (int i = 0; i < _payRecords.Count; i++)
            {
                var item = GetItem(i);
                item.SetActive(true);
                item.SetData(_payRecords[i]);
            }

            for (int i = _curPayTypeCount; i < _items.Count; i++)
            {
                _items[i].SetActive(false);
            }
        }

        private void SaveData()
        {
            if (_curPayTypeCount != _payRecords.Count || CheckInfoChanged())
            {
                _payRecords.Clear();
                for (int i = 0; i < _items.Count; i++)
                {
                    if (_items[i].IsActive)
                    {
                        _payRecords.Add(_items[i].GetData());
                    }
                    else
                    {
                        break;
                    }
                }
                
                Messenger.Broadcast(EMessengerType.OnPayInfoChanged);
            }
        }

        private bool CheckInfoChanged()
        {
            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i].IsDirty)
                {
                    return true;
                }
            }

            return false;
        }

        private void AddBtn()
        {
            var item = GetItem(_curPayTypeCount);
            item.SetActive(true);
            item.SetData(PayRecord.CreateNew());
            _curPayTypeCount++;
            ScrollToEnd();
        }

        private void ScrollToEnd()
        {
            CoroutineProxy.Instance.StartCoroutine(CoroutineProxy.RunNextFrame(() =>
            {
                var height = _cachedView.Content.GetHeight();
                _cachedView.Content.anchoredPosition = new Vector2(0, Mathf.Max(0, height - _parentHeight));
            }));
        }

        private void DeleteBtn()
        {
            if (_curPayTypeCount > 0)
            {
                _curPayTypeCount--;
                var item = GetItem(_curPayTypeCount);
                item.SetActive(false);
                ScrollToEnd();
            }
        }

        private void OKBtn()
        {
            SaveData();
            SocialGUIManager.Instance.CloseUI<UICtrlPay>();
        }

        private void CancelBtn()
        {
            SocialGUIManager.Instance.CloseUI<UICtrlPay>();
        }

        private UMCtrlPayItem GetItem(int index)
        {
            if (index < _items.Count)
            {
                return _items[index];
            }
            else
            {
                var item = new UMCtrlPayItem();
                item.Init(_cachedView.Content);
                _items.Add(item);
                return item;
            }
        }
    }
}