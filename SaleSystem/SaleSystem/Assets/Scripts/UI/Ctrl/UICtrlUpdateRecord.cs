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
            _data.CheckInDate = _checkInCtrl.GetDateTime();
            _data.CheckOutDate = _checkOutCtrl.GetDateTime();
            _data.RoomIndex = room.Index;
            _data.RoommerName = roomerName;
            _data.State = (ERoomerState) _stateCtrl.GetVal();
            _data.Price = int.Parse(_priceCtrl.GetContent());
            SaleDataManager.Instance.RefreshRoomRecords();
            Messenger.Broadcast(EMessengerType.OnRoomRecordChanged);
            SaleDataManager.Instance.SaveData();
        }

        protected override void CloseBtn()
        {
            SocialGUIManager.Instance.CloseUI<UICtrlUpdateRecord>();
        }
    }
}