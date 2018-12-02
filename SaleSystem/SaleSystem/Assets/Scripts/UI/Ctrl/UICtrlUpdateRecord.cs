using System;
using MyTools;
using UITools;

namespace Sale
{
    [UIAutoSetup]
    public class UICtrlUpdateRecord : UICtrlCreateRecord
    {
        protected override void RefreshView(object parameter)
        {
            _data = parameter as RoomRecordData;
            if (_data == null)
            {
                SocialGUIManager.Instance.CloseUI<UICtrlUpdateRecord>();
                SocialGUIManager.ShowPopupDialog("订单数据错误");
                return;
            }

            _recordId.SetContent(_data.Id.ToString());
            _roomCtrl.SetOptions(_roomNames);
            _roomCtrl.SetCurVal(_data.RoomIndex);
            _priceCtrl.SetContent(_data.Price.ToString());
            _checkInCtrl.SetDate(_data.CheckInDate);
            _checkOutCtrl.SetDate(_data.CheckOutDate);
            _roomerCtrl.SetContent(_data.RoommerName);
            _stateCtrl.SetCurVal((int) _data.State);
            RefreshPayInfo();
        }

        protected override void SaveData()
        {
            var roomIndex = _roomCtrl.GetVal();
            var room = SaleDataManager.Instance.Rooms[roomIndex];
            var roomerName = _roomerCtrl.GetContent();
            var data = new RoomRecordData();
            data.Id = SaleDataManager.Instance.RecordIndex;
            data.CreateDate = DateTime.Now;
            data.CheckInDate = _checkInCtrl.GetDateTime();
            data.CheckOutDate = _checkOutCtrl.GetDateTime();
            data.RoomIndex = room.Index;
            data.RoommerName = roomerName;
            data.State = (ERoomerState) _stateCtrl.GetVal();
            var price = _priceCtrl.GetContent();
            data.Price = int.Parse(price);
            SaleDataManager.Instance.AddRoomRecord(data);
            room.AddRecord(data);
            Messenger.Broadcast(EMessengerType.OnRoomRecordChanged);
        }

        protected override void CloseBtn()
        {
            SocialGUIManager.Instance.CloseUI<UICtrlUpdateRecord>();
        }
    }
}