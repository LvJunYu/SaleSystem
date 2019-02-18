using System.Collections.Generic;
using System;
using MyTools;
using UITools;
using UnityEngine;

namespace Sale
{
    [UIAutoSetup(EUIAutoSetupType.Create)]
    public class UICtrlPopupDialog : UICtrlGenericBase<UIViewPopupDialog>
    {
        private Stack<UMCtrlDialog> _ctrlDialogsDic = new Stack<UMCtrlDialog>();
        private bool _justShow; // 刚显示的那一帧不检查关闭

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (_justShow)
            {
                _justShow = false;
            }
            else
            {
                if (_ctrlDialogsDic.Count > 0)
                {
                    if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
                    {
                        _ctrlDialogsDic.Pop().Destroy();
                    }
                }
                else
                {
                    SocialGUIManager.Instance.CloseUI<UICtrlPopupDialog>();
                }
            }
        }

        protected override void InitGroupId()
        {
            _groupId = (int) EUIGroupType.PopUpDialog;
        }

        protected override void InitEventListener()
        {
            base.InitEventListener();
            RegisterEvent<string, string, KeyValuePair<string, Action>[]>(EMessengerType.ShowDialog,
                MessengerShowDialogHandler);
        }

        protected override void OnClose()
        {
            _ctrlDialogsDic.Clear();
            base.OnClose();
        }

        private void MessengerShowDialogHandler(string msg, string title, KeyValuePair<string, Action>[] btnParam)
        {
            if (!IsOpen)
            {
                SocialGUIManager.Instance.OpenUI<UICtrlPopupDialog>();
            }

            ShowDialog(msg, title, btnParam);
            _justShow = true;
        }

        private void ShowDialog(string msg, string title, params KeyValuePair<string, Action>[] btnParam)
        {
            if (!CheckParam(btnParam))
            {
                LogHelper.Warning("Show Dialog Param error, msg: " + msg);
                return;
            }

            if (!_isOpen)
            {
                SocialGUIManager.Instance.OpenUI<UICtrlPopupDialog>();
            }

            UMCtrlDialog ctrl = new UMCtrlDialog();
            ctrl.Init(_cachedView.ContentDock);
            ctrl.Set(msg, title, btnParam);
            _ctrlDialogsDic.Push(ctrl);
        }

        private bool CheckParam(KeyValuePair<string, Action>[] btnParam)
        {
            if (btnParam.Length > 3)
            {
                return false;
            }

            for (int i = 0; i < btnParam.Length; i++)
            {
                if (string.IsNullOrEmpty(btnParam[i].Key))
                {
                    return false;
                }
            }

            return true;
        }
    }
}