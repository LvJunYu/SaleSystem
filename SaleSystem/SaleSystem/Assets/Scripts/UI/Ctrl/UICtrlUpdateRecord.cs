using System;
using System.Collections.Generic;
using MyTools;
using UITools;

namespace Sale
{
    [UIAutoSetup(EUIAutoSetupType.Create)]
    public class UICtrlUpdateRecord : UICtrlCreateRecord
    {
        protected override void OnViewCreated()
        {
            base.OnViewCreated();
            var view = _cachedView as UIViewUpdateRecord;
            if (view != null)
            {
                view.DeleteBtn.onClick.AddListener(DeleteBtn);
            }
        }

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
            _roomerNumCtrl.SetContent(_data.RoommerNum == 0 ? string.Empty : _data.RoommerNum.ToString());
            _stateCtrl.SetCurVal((int) _data.State);
            RefreshPayInfo();
        }

        protected override void SaveData()
        {
            var roomIndex = _roomCtrl.GetVal();
            var room = SaleDataManager.Instance.Rooms[roomIndex];
            var oldRoomIndex = _data.RoomIndex;
            _data.CheckInDate = _checkInCtrl.GetDateTime();
            _data.CheckOutDate = _checkOutCtrl.GetDateTime();
            _data.RoomIndex = room.Index;
            _data.RoommerName = _roomerCtrl.GetContent();
            _data.RoommerNum = int.Parse(_roomerNumCtrl.GetContent());
            _data.State = (ERoomerState) _stateCtrl.GetVal();
            _data.Price = int.Parse(_priceCtrl.GetContent());
            if (oldRoomIndex != roomIndex)
            {
                SaleDataManager.Instance.Rooms[oldRoomIndex].RemoveRecord(_data);
                room.AddRecord(_data);
            }

            Messenger.Broadcast(EMessengerType.OnRoomRecordChanged);
            SaleDataManager.Instance.SaveData();
        }

        protected override void CloseBtn()
        {
            SocialGUIManager.Instance.CloseUI<UICtrlUpdateRecord>();
        }

        private void DeleteBtn()
        {
            if (!UserData.Instance.CheckIdentity()) return;
            SocialGUIManager.ShowPopupDialog("确定删除该订单吗？", null, new KeyValuePair<string, Action>("确定", DeleteData),
                new KeyValuePair<string, Action>("取消", null));
        }

        private void DeleteData()
        {
            var room = SaleDataManager.Instance.Rooms[_data.RoomIndex];
            SaleDataManager.Instance.RemoveRoomRecord(_data);
            room.RemoveRecord(_data);
            Messenger.Broadcast(EMessengerType.OnRoomRecordChanged);
            SocialGUIManager.Instance.CloseUI<UICtrlUpdateRecord>();
        }
    }
}