﻿using System;
using UITools;
using UnityEngine;

namespace Sale
{
    [UIAutoSetup]
    public class UICtrlLogin : UICtrlGenericBase<UIViewLogin>
    {
        private EFocusType _focusType;

        protected override void InitGroupId()
        {
            _groupId = (int) EUIGroupType.Pop2;
        }

        protected override void OnViewCreated()
        {
            base.OnViewCreated();
            _cachedView.Guest.onClick.AddListener(GuestBtn);
            _cachedView.Login.onClick.AddListener(LoginBtn);
        }

        protected override void OnOpen(object parameter)
        {
            base.OnOpen(parameter);
            if (_focusType == EFocusType.None)
            {
                _focusType = EFocusType.Name;
            }

            RefreshFocus();
        }

        private void LoginBtn()
        {
            if (UserData.Instance.TryAdminLogin(_cachedView.Name.text, _cachedView.Pwd.text))
            {
                SocialGUIManager.Instance.CloseUI<UICtrlLogin>();
                SocialGUIManager.Instance.OpenUI<UICtrlMainApp>();
            }
        }

        private void GuestBtn()
        {
            UserData.Instance.GuestLogin();
            SocialGUIManager.Instance.CloseUI<UICtrlLogin>();
            SocialGUIManager.Instance.OpenUI<UICtrlMainApp>();
        }

        public override void OnUpdate()
        {
            if (Input.GetKeyUp(KeyCode.Tab))
            {
                switch (_focusType)
                {
                    case EFocusType.None:
                        _focusType = EFocusType.Name;
                        break;
                    case EFocusType.Name:
                        _focusType = EFocusType.Password;
                        break;
                    case EFocusType.Password:
                        _focusType = EFocusType.Name;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                RefreshFocus();
            }
        }

        private void RefreshFocus()
        {
            switch (_focusType)
            {
                case EFocusType.Name:
                    _cachedView.Name.Select();
                    break;
                case EFocusType.Password:
                    _cachedView.Pwd.Select();
                    break;
            }
        }

        private enum EFocusType
        {
            None,
            Name,
            Password
        }
    }
}