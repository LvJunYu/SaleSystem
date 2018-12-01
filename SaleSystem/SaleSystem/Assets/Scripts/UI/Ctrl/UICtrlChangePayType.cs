using System.Collections.Generic;
using MyTools;
using UITools;
using UnityEngine;

namespace Sale
{
    [UIAutoSetup]
    public class UICtrlChangePayType : UICtrlGenericBase<UIViewChangeRoom>
    {
        private List<UMCtrlPayType> _items = new List<UMCtrlPayType>();
        private int _cuuPayTypeCount;
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

        private void RefreshView()
        {
            var payTypes = SaleDataManager.Instance.PayTypes;
            _cuuPayTypeCount = payTypes.Count;
            for (int i = 0; i < payTypes.Count; i++)
            {
                var item = GetItem(i);
                item.SetActive(true);
                item.SetData(payTypes[i]);
            }

            for (int i = _cuuPayTypeCount; i < _items.Count; i++)
            {
                _items[i].SetActive(false);
            }
        }

        private void SaveData()
        {
            if (_cuuPayTypeCount != SaleDataManager.Instance.PayTypes.Count || CheckInfoChanged())
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
            var item = GetItem(_cuuPayTypeCount);
            item.SetActive(true);
            _cuuPayTypeCount++;
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
            if (_cuuPayTypeCount > 0)
            {
                _cuuPayTypeCount--;
                var item = GetItem(_cuuPayTypeCount);
                item.SetActive(false);
                ScrollToEnd();
            }
        }

        private void OKBtn()
        {
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