﻿using UITools;

namespace Sale
{
    [UIAutoSetup]
    public class UICtrlRecord : UICtrlGenericBase<UIViewRecord>
    {
        private UPCtrlRecordBase _curMenuCtrl;
        private UPCtrlRecordBase[] _menuCtrlArray;
        private EMenu _curMenu;

        protected override void OnViewCreated()
        {
            base.OnViewCreated();
            _cachedView.CloseBtn.onClick.AddListener(OnCloseBtn);
            _menuCtrlArray = new UPCtrlRecordBase[(int) EMenu.Max];
            _menuCtrlArray[(int) EMenu.Room] = CreateUpCtrl<UPCtrlRecordRoom>(EMenu.Room);
            for (int i = 0; i < _cachedView.MenuButtonAry.Length; i++)
            {
                var inx = i;
                _cachedView.TabGroup.AddButton(_cachedView.MenuButtonAry[i], _cachedView.MenuSelectedButtonAry[i],
                    b => ClickMenu(inx, b));
                if (i < _menuCtrlArray.Length && null != _menuCtrlArray[i])
                {
                    _menuCtrlArray[i].Close();
                }
            }
        }

        protected override void OnOpen(object parameter)
        {
            base.OnOpen(parameter);
            if (_curMenu == EMenu.None)
            {
                _curMenu = EMenu.Room;
            }

            _cachedView.TabGroup.SelectIndex((int) _curMenu, true);
        }

        protected override void InitGroupId()
        {
            _groupId = (int) EUIGroupType.Pop1;
        }

        private void ClickMenu(int idx, bool open)
        {
            if (open)
            {
                ChangeMenu((EMenu) idx);
            }
        }

        private void ChangeMenu(EMenu menu)
        {
            if (_curMenuCtrl != null)
            {
                _curMenuCtrl.Close();
            }

            _curMenu = menu;
            _curMenuCtrl = _menuCtrlArray[(int) _curMenu];
            if (_curMenuCtrl != null)
            {
                _curMenuCtrl.Open();
            }
        }

        private void OnCloseBtn()
        {
            SocialGUIManager.Instance.CloseUI<UICtrlRecord>();
        }

        private T CreateUpCtrl<T>(EMenu menu) where T : UPCtrlRecordBase, new()
        {
            var ctrl = new T();
            ctrl.Menu = menu;
            ctrl.Init(this, _cachedView);
            return ctrl;
        }

        public enum EMenu
        {
            None = -1,
            Room,
            Dinner,
            Max
        }
    }
}