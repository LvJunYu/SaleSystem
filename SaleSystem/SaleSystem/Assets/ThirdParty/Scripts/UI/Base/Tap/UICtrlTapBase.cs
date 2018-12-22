namespace UITools
{
    /// <summary>
    /// 多Menu基类
    /// </summary>
    public abstract class UICtrlTapBase<TC, TV, TP> : UICtrlAnimationBase<TV> where TV : UIViewTapBase
        where TC : UICtrlTapBase<TC, TV, TP>
        where TP : UPCtrlTapBase<TC, TV>, new()
    {
        protected TP[] _menuCtrlArray;
        protected TP _curMenuCtrl;
        protected int _curMenu;

        protected override void OnViewCreated()
        {
            base.OnViewCreated();
            for (int i = 0; i < _cachedView.MenuButtonAry.Length; i++)
            {
                var inx = i;
                _cachedView.TabGroup.AddButton(_cachedView.MenuButtonAry[i], _cachedView.MenuSelectedButtonAry[i],
                    b => ClickMenu(inx, b));
            }
        }

        protected override void OnOpen(object parameter)
        {
            base.OnOpen(parameter);
            if (_curMenu < 0)
            {
                _curMenu = 0;
            }

            _cachedView.TabGroup.SelectIndex(_curMenu, true);
        }

        private void ClickMenu(int idx, bool open)
        {
            if (open)
            {
                ChangeMenu(idx);
            }
        }

        private void ChangeMenu(int menu)
        {
            if (_curMenuCtrl != null)
            {
                _curMenuCtrl.Close();
            }

            _curMenu = menu;
            _curMenuCtrl = _menuCtrlArray[_curMenu];
            if (_curMenuCtrl != null)
            {
                _curMenuCtrl.Open();
            }
        }

        protected T CreateUpCtrl<T>(int menu) where T : TP, new()
        {
            var ctrl = new T();
            ctrl.Menu = menu;
            ctrl.Init(this as TC, _cachedView);
            ctrl.Close();
            return ctrl;
        }
    }
}