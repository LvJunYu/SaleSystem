using System;
using System.Collections.Generic;
using MyTools;
using UITools;
using UnityEngine.UI;

namespace Sale
{
    [UIAutoSetup(EUIAutoSetupType.Create)]
    public class UICtrlCreateRecord : UICtrlAnimationBase<UIViewCreateRecord>
    {
        protected static List<string> _roomNames = new List<string>();

        private static List<string> States = new List<string>
        {
            ERoomerState.预定.ToString(),
            ERoomerState.入住.ToString(),
            ERoomerState.退房.ToString(),
        };

        protected USCtrlInfo _recordId;
        protected UMCtrlDropdown _roomCtrl;
        protected UMCtrlInfoItem _priceCtrl;
        protected UMCtrlDate _checkInCtrl;
        protected UMCtrlDate _checkOutCtrl;
        protected UMCtrlInfoItem _roomerCtrl;
        protected UMCtrlInfoItem _roomerNumCtrl;
        protected UMCtrlDropdown _stateCtrl;
        protected UMCtrlPayInfo _payCtrl;
        protected RoomRecordData _data;

        protected override void OnViewCreated()
        {
            base.OnViewCreated();
            _cachedView.CloseBtn.onClick.AddListener(CloseBtn);
            _cachedView.OKBtn.onClick.AddListener(OKBtn);
            _recordId = new USCtrlInfo();
            _recordId.Init(_cachedView.RecordIdView);
            _recordId.SetTitle("订单号：");

            _roomCtrl = new UMCtrlDropdown();
            _roomCtrl.Init(_cachedView.InfoContent);
            _roomCtrl.SetTitle("房间：");
            _roomCtrl.AddListener(OnRoomValChanged);

            _priceCtrl = new UMCtrlInfoItem();
            _priceCtrl.Init(_cachedView.InfoContent);
            _priceCtrl.SetTitle("价格：");
            _priceCtrl.SetContentType(InputField.ContentType.IntegerNumber);

            _checkInCtrl = new UMCtrlDate();
            _checkInCtrl.Init(_cachedView.InfoContent);
            _checkInCtrl.SetTitle("入住时间：");
            _checkOutCtrl = new UMCtrlDate();
            _checkOutCtrl.Init(_cachedView.InfoContent);
            _checkOutCtrl.SetTitle("退房时间：");

            _roomerCtrl = new UMCtrlInfoItem();
            _roomerCtrl.Init(_cachedView.InfoContent);
            _roomerCtrl.SetTitle("房客姓名：");
            _roomerCtrl.SetGuidContent("请输入房客姓名");
            _roomerCtrl.SetGuidActive(true);
            _roomerNumCtrl = new UMCtrlInfoItem();
            _roomerNumCtrl.Init(_cachedView.InfoContent);
            _roomerNumCtrl.SetTitle("身份证号：");
            _roomerNumCtrl.SetGuidContent("请输入身份证号");
            _roomerNumCtrl.SetGuidActive(true);
            _roomerNumCtrl.SetContentType(InputField.ContentType.IntegerNumber);

            _stateCtrl = new UMCtrlDropdown();
            _stateCtrl.Init(_cachedView.InfoContent);
            _stateCtrl.SetTitle("房客状态：");
            _stateCtrl.SetOptions(States);

            _payCtrl = new UMCtrlPayInfo();
            _payCtrl.Init(_cachedView.InfoContent);
            _payCtrl.SetTitle("已付款：");
            _payCtrl.AddBtnListener(PayBtn);
            InitRoomData();
        }

        protected override void OnOpen(object parameter)
        {
            base.OnOpen(parameter);
            RefreshView(parameter);
        }

        protected override void InitEventListener()
        {
            base.InitEventListener();
            RegisterEvent(EMessengerType.OnRoomChanged, InitRoomData);
            RegisterEvent(EMessengerType.OnPayInfoChanged, OnPayInfoChanged);
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

        protected virtual void RefreshView(object parameter)
        {
            _data = new RoomRecordData();
            _recordId.SetContent(SaleDataManager.Instance.RecordIndex.ToString());
            _roomCtrl.SetOptions(_roomNames);
            _roomCtrl.SetCurVal(0);
            OnRoomValChanged(0);
            _checkInCtrl.SetDate(DateTime.Now);
            _checkOutCtrl.SetDate(DateTime.Now.AddDays(1));
            _roomerCtrl.SetContent(string.Empty);
            _roomerNumCtrl.SetContent(string.Empty);
            _stateCtrl.SetCurVal((int) ERoomerState.入住);
            RefreshPayInfo();
        }

        protected virtual void SaveData()
        {
            var roomIndex = _roomCtrl.GetVal();
            var room = SaleDataManager.Instance.Rooms[roomIndex];
            _data.Id = SaleDataManager.Instance.RecordIndex;
            _data.CreateDate = DateTime.Now;
            _data.CheckInDate = _checkInCtrl.GetDateTime();
            _data.CheckOutDate = _checkOutCtrl.GetDateTime();
            _data.RoomIndex = room.Index;
            _data.RoommerName = _roomerCtrl.GetContent();
            _data.RoommerNum = int.Parse(_roomerCtrl.GetContent());
            _data.State = (ERoomerState) _stateCtrl.GetVal();
            var price = _priceCtrl.GetContent();
            _data.Price = int.Parse(price);
            SaleDataManager.Instance.AddRoomRecord(_data);
            room.AddRecord(_data);
            Messenger.Broadcast(EMessengerType.OnRoomRecordChanged);
        }

        protected virtual void CloseBtn()
        {
            SocialGUIManager.Instance.CloseUI<UICtrlCreateRecord>();
        }

        private void OnPayInfoChanged()
        {
            if (!_isOpen) return;
            RefreshPayInfo();
        }

        protected void RefreshPayInfo()
        {
            var payInfos = _data.PayRecords;
            int pay = 0;
            for (int i = 0; i < payInfos.Count; i++)
            {
                pay += payInfos[i].PayNum;
            }

            _payCtrl.SetContent(pay.ToString());
        }

        private void PayBtn()
        {
            SocialGUIManager.Instance.OpenUI<UICtrlPay>(_data.PayRecords);
        }

        private void OKBtn()
        {
            if (!UserData.Instance.CheckIdentity(false))
            {
                CloseBtn();
                return;
            }

            if (!CheckDataValid()) return;
            SaveData();
            CloseBtn();
        }

        private bool CheckDataValid()
        {
            var checkInData = _checkInCtrl.GetDateTime();
            var checkOutDate = _checkOutCtrl.GetDateTime();
            if (checkInData.GetDays() >= checkOutDate.GetDays())
            {
                SocialGUIManager.ShowPopupDialog("订单时间少于1天");
                return false;
            }

            var roomIndex = _roomCtrl.GetVal();
            var room = SaleDataManager.Instance.Rooms[roomIndex];
            if (room.CheckDateConflict(_data, checkInData, checkOutDate))
            {
                SocialGUIManager.ShowPopupDialogFormat("房间{0}已经被预定", room.Name);
                return false;
            }

            var roomerName = _roomerCtrl.GetContent();
            if (string.IsNullOrEmpty(roomerName))
            {
                SocialGUIManager.ShowPopupDialog("请填写房客姓名");
                return false;
            }

            var roomerNum = _roomerNumCtrl.GetContent();
            if (string.IsNullOrEmpty(roomerNum))
            {
                SocialGUIManager.ShowPopupDialog("请填写房客身份证号");
                return false;
            }

            return true;
        }

        private void InitRoomData()
        {
            var rooms = SaleDataManager.Instance.Rooms;
            _roomNames.Clear();
            for (int i = 0; i < rooms.Count; i++)
            {
                _roomNames.Add(string.Format("{0}:{1}", rooms[i].Index + 1, rooms[i].Name));
            }
        }

        private void OnRoomValChanged(int val)
        {
            var room = SaleDataManager.Instance.Rooms[val];
            _priceCtrl.SetContent(room.Price.ToString());
        }
    }
}