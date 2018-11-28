using System;
using System.Collections.Generic;
using MyTools;
using UITools;
using UnityEngine.UI;

namespace Sale
{
    [UIAutoSetup]
    public class UICtrlCreateRecord : UICtrlGenericBase<UIViewCreateRecord>
    {
        private USCtrlInfo _recordId;
        private UMCtrlDropdown _roomCtrl;
        private UMCtrlInfoItem _priceCtrl;
        private UMCtrlDate _checkInCtrl;
        private UMCtrlDate _checkOutCtrl;
        private UMCtrlInfoItem _payCountCtrl;
        private UMCtrlDropdown _payTypeCtrl;
        private List<string> _roomNames = new List<string>();
        private RoomRecordData _data;

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
            InitRoomData();
        }

        protected override void OnOpen(object parameter)
        {
            base.OnOpen(parameter);
            RefreshView();
        }

        protected override void InitEventListener()
        {
            base.InitEventListener();
            RegisterEvent(EMessengerType.OnRoomChanged, InitRoomData);
        }

        protected override void InitGroupId()
        {
            _groupId = (int) EUIGroupType.Pop2;
        }

        private void InitRoomData()
        {
            var rooms = SaleDataManager.Instance.Rooms;
            _roomNames.Clear();
            for (int i = 0; i < rooms.Count; i++)
            {
                _roomNames.Add(rooms[i].Name);
            }
        }

        private void RefreshView()
        {
            _recordId.SetContent(SaleDataManager.Instance.RecordIndex.ToString());
            RefreshRoom();
            RefreshDate();
            RefreshPay();
        }

        private void RefreshRoom()
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
            var checkInData = _checkInCtrl.GetDateTime();
            var checkOutDate = _checkOutCtrl.GetDateTime();
            if (checkInData.GetDays() >= checkOutDate.GetDays())
            {
                SocialGUIManager.ShowPopupDialog("订单时间少于1天");
                return;
            }

            var roomIndex = _roomCtrl.GetVal();
            var room = SaleDataManager.Instance.Rooms[roomIndex];
            if (room.CheckDateConflict(checkInData, checkOutDate))
            {
                SocialGUIManager.ShowPopupDialog("房间{0}已经被预定", room.Name);
                return;
            }
            var data = new RoomRecordData();
            data.Id = SaleDataManager.Instance.RecordIndex;
            data.CreateDate = DateTime.Now;
            data.CheckInDate = checkInData;
            data.CheckOutDate = checkOutDate;
            data.RoomIndex = room.Index;
            var price = _priceCtrl.GetContent();
            data.Price = int.Parse(price);
            var payCount = _payCountCtrl.GetContent();
            var payType = (EPayType) _payTypeCtrl.GetVal();
            data.PayRecords.Add(new PayRecord(int.Parse(payCount), payType));
            SaleDataManager.Instance.AddRoomRecord(data);
            room.AddRecord(data);
            Messenger.Broadcast(EMessengerType.OnRoomRecordChanged);
            CloseBtn();
        }

        private void CloseBtn()
        {
            SocialGUIManager.Instance.CloseUI<UICtrlCreateRecord>();
        }
    }
}