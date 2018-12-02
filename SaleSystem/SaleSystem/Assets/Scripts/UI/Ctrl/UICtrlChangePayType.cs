using System.Collections.Generic;
using MyTools;
using UITools;
using UnityEngine;

namespace Sale
{
    [UIAutoSetup]
    public class UICtrlChangePayType : UICtrlAnimationBase<UIViewChangeRoom>
    {
        private List<UMCtrlPayType> _items = new List<UMCtrlPayType>();
        private int _curPayTypeCount;
        private float _parentHeight;

        protected override void InitGroupId()
        {
            _groupId = (int) EUIGroupType.Pop2;
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
            RefreshView();
        }

        protected override void SetPartAnimations()
        {
            base.SetPartAnimations();
            SetPart(_cachedView.PannelRtf, EAnimationType.MoveFromDown);
            SetPart(_cachedView.BGRtf, EAnimationType.Fade);
        }

        private void RefreshView()
        {
            var payTypes = SaleDataManager.Instance.PayTypes;
            _curPayTypeCount = payTypes.Count;
            for (int i = 0; i < payTypes.Count; i++)
            {
                var item = GetItem(i);
                item.SetActive(true);
                item.SetData(payTypes[i]);
            }

            for (int i = _curPayTypeCount; i < _items.Count; i++)
            {
                _items[i].SetActive(false);
            }
        }

        private void SaveData()
        {
            if (_curPayTypeCount != SaleDataManager.Instance.PayTypes.Count || CheckInfoChanged())
            {
                var payTypes = new List<string>();
                for (int i = 0; i < _items.Count; i++)
                {
                    if (_items[i].IsActive)
                    {
                        payTypes.Add(_items[i].GetData());
                    }
                    else
                    {
                        break;
                    }
                }

                SaleDataManager.Instance.PayTypes = payTypes;
                Messenger.Broadcast(EMessengerType.OnPayTypeChanged);
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
            if (_curPayTypeCount == 0)
            {
                SocialGUIManager.ShowPopupDialog("至少有一种付款方式");
                return;
            }

            SaveData();
            SocialGUIManager.Instance.CloseUI<UICtrlChangePayType>();
        }

        private void CancelBtn()
        {
            SocialGUIManager.Instance.CloseUI<UICtrlChangePayType>();
        }

        private UMCtrlPayType GetItem(int index)
        {
            if (index < _items.Count)
            {
                return _items[index];
            }
            else
            {
                var item = new UMCtrlPayType();
                item.Init(_cachedView.Content);
                _items.Add(item);
                return item;
            }
        }
    }
}