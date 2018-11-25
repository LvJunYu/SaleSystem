using System;
using System.Collections.Generic;
using MyTools;
using UITools;
using UnityEngine.UI;

namespace Sale
{
    [UIAutoSetup]
    public class UICtrlCreateNew : UICtrlGenericBase<UIViewCreateNew>
    {
        private UMCtrlDropdown _roomCtrl;
        private UMCtrlInfoItem _priceCtrl;
        private UMCtrlDate _checkInCtrl;
        private UMCtrlDate _checkOutCtrl;
        private UMCtrlInfoItem _payCountCtrl;
        private UMCtrlDropdown _payTypeCtrl;
        private List<string> _roomNames = new List<string>();

        protected override void OnViewCreated()
        {
            base.OnViewCreated();
            _cachedView.CloseBtn.onClick.AddListener(CloseBtn);
            _cachedView.OKBtn.onClick.AddListener(OKBtn);
            _roomCtrl = new UMCtrlDropdown();
            _roomCtrl.Init(_cachedView.InfoContent);
            _roomCtrl.SetTitle("房间号：");
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

            _payCountCtrl = new UMCtrlInfoItem();
            _payCountCtrl.Init(_cachedView.InfoContent);
            _payCountCtrl.SetTitle("付款金额：");
            _payCountCtrl.SetContentType(InputField.ContentType.IntegerNumber);
            _payTypeCtrl = new UMCtrlDropdown();
            _payTypeCtrl.Init(_cachedView.InfoContent);
            _payTypeCtrl.SetTitle("付款方式：");
            _payTypeCtrl.SetOptions(SaleConstDefine.PayTypes);
            RefreshRoomData();
        }

        protected override void OnOpen(object parameter)
        {
            base.OnOpen(parameter);
            RefreshRooms();
            RefreshDate();
            RefreshPay();
        }

        protected override void InitEventListener()
        {
            base.InitEventListener();
            RegisterEvent(EMessengerType.OnRoomChanged, RefreshRoomData);
        }

        protected override void InitGroupId()
        {
            _groupId = (int) EUIGroupType.Pop2;
        }

        private void RefreshRoomData()
        {
            var rooms = SaleDataManager.Instance.Rooms;
            _roomNames.Clear();
            for (int i = 0; i < rooms.Count; i++)
            {
                _roomNames.Add(rooms[i].Name);
            }
        }

        private void RefreshRooms()
        {
            _roomCtrl.SetOptions(_roomNames);
            _roomCtrl.SetCurVal(0);
            OnRoomValChanged(0);
        }

        private void RefreshDate()
        {
            _checkInCtrl.SetDate(DateTime.Now);
            _checkOutCtrl.SetDate(DateTime.Now.AddDays(1));
        }

        private void RefreshPay()
        {
            _payCountCtrl.SetContent("0");
            _payTypeCtrl.SetCurVal(0);
        }

        private void OnRoomValChanged(int val)
        {
            var room = SaleDataManager.Instance.Rooms[val];
            _priceCtrl.SetContent(room.Price.ToString());
        }

        private void OKBtn()
        {
            //todo 判断订单是否合法（格式、日期是否冲突等）
            var data = new RoomRecord();
            data.CreateDate = DateTime.Now;
            data.CheckInDate = _checkInCtrl.GetDateTime();
            data.CheckOutDate = _checkOutCtrl.GetDateTime();
            var roomIndex = _roomCtrl.GetVal();
            var room = SaleDataManager.Instance.Rooms[roomIndex];
            data.RoomIndex = room.Index;
            var price = _priceCtrl.GetContent();
            data.Price = int.Parse(price);
            var payCount = _payCountCtrl.GetContent();
            var payType = (EPayType) _payTypeCtrl.GetVal();
            data.PayRecords.Add(new PayRecord(int.Parse(payCount), payType));
            room.AddRecord(data);
            SaleDataManager.Instance.AddRoomRecord(data);
            Messenger.Broadcast(EMessengerType.OnRoomRecordChanged);
            CloseBtn();
        }

        private void CloseBtn()
        {
            SocialGUIManager.Instance.CloseUI<UICtrlCreateNew>();
        }
    }
}