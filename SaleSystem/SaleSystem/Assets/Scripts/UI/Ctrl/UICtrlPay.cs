using System;
using System.Collections.Generic;
using MyTools;
using UITools;
using UnityEngine;

namespace Sale
{
    [UIAutoSetup(EUIAutoSetupType.Create)]
    public class UICtrlPay : UICtrlAnimationBase<UIViewPay>
    {
        private List<UMCtrlPayItem> _items = new List<UMCtrlPayItem>();
        private int _curPayTypeCount;
        private float _parentHeight;
        private RoomRecord _data;

        private List<PayRecord> _payRecords
        {
            get { return _data.ChangePayRecords ?? _data.PayRecords; }
        }

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
            _data = parameter as RoomRecord;
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

        private bool HasIdentity()
        {
            switch (_data.RecordPhase)
            {
                case ERecordPhase.Creating:
                    if (!UserData.Instance.CheckIdentity(EBehaviorType.CreateRecord)) return false;
                    break;
                case ERecordPhase.Valid:
                    if (!UserData.Instance.CheckIdentity(EBehaviorType.UpdateRecord)) return false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return true;
        }

        private void SaveData()
        {
            if (_curPayTypeCount != _payRecords.Count || CheckInfoChanged())
            {
                if(!HasIdentity()) return;
                if (_data.ChangePayRecords == null)
                {
                    _data.ChangePayRecords = new List<PayRecord>();
                }
                else
                {
                    _data.ChangePayRecords.Clear();
                }

                for (int i = 0; i < _items.Count; i++)
                {
                    if (_items[i].IsActive)
                    {
                        var payRecord = _items[i].GetData();
                        _data.ChangePayRecords.Add(payRecord);
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
            if (!HasIdentity()) return;
            var item = GetItem(_curPayTypeCount);
            item.SetActive(true);
            item.SetData(PayRecord.CreateNew(_data.Id));
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
            if (!HasIdentity()) return;
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