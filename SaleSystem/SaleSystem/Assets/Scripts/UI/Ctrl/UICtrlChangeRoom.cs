using System.Collections.Generic;
using MyTools;
using UITools;
using UnityEngine;

namespace Sale
{
    [UIAutoSetup]
    public class UICtrlChangeRoom : UICtrlAnimationBase<UIViewChangeRoom>
    {
        private List<UMCtrlRoomRaw> _rooms = new List<UMCtrlRoomRaw>();
        private int _curRoomCount;
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
            var rooms = SaleDataManager.Instance.Rooms;
            _curRoomCount = rooms.Count;
            for (int i = 0; i < _curRoomCount; i++)
            {
                var item = GetItem(i);
                item.SetActive(true);
                item.SetData(rooms[i]);
            }

            for (int i = _curRoomCount; i < _rooms.Count; i++)
            {
                _rooms[i].SetActive(false);
            }

            ScrollToEnd();
        }

        private void SaveRoomData()
        {
            if (_curRoomCount != SaleDataManager.Instance.Rooms.Count ||
                CheckRoomInfoChanged())
            {
                var rooms = SaleDataManager.Instance.Rooms;
                rooms.Clear();
                for (int i = 0; i < _rooms.Count; i++)
                {
                    if (_rooms[i].IsActive)
                    {
                        rooms.Add(_rooms[i].GetRoomData());
                    }
                    else
                    {
                        break;
                    }
                }

                SaleDataManager.Instance.RefreshRoomRecords();
                Messenger.Broadcast(EMessengerType.OnRoomChanged);
                SaleDataManager.Instance.ChangeRooms();
            }
        }

        private bool CheckRoomInfoChanged()
        {
            for (int i = 0; i < _rooms.Count; i++)
            {
                if (_rooms[i].IsDirty)
                {
                    return true;
                }
            }

            return false;
        }

        private void AddBtn()
        {
            var item = GetItem(_curRoomCount);
            item.SetActive(true);
            item.SetData(new Room(_curRoomCount));
            item.IsDirty = true;
            _curRoomCount++;
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
            if (_curRoomCount > 0)
            {
                _curRoomCount--;
                var item = GetItem(_curRoomCount);
                item.SetActive(false);
                ScrollToEnd();
            }
        }

        private void OKBtn()
        {
            SaveRoomData();
            SocialGUIManager.Instance.CloseUI<UICtrlChangeRoom>();
        }

        private void CancelBtn()
        {
            SocialGUIManager.Instance.CloseUI<UICtrlChangeRoom>();
        }

        private UMCtrlRoomRaw GetItem(int index)
        {
            if (index < _rooms.Count)
            {
                return _rooms[index];
            }
            else
            {
                var item = new UMCtrlRoomRaw();
                item.Init(_cachedView.Content);
                _rooms.Add(item);
                return item;
            }
        }
    }
}