using System;
using System.Collections.Generic;
using MyTools;
using UITools;
using UnityEngine;
using UnityEngine.UI;

namespace Sale
{
    public class SocialGUIManager : GUIManager
    {
        public static SocialGUIManager Instance;

        void Awake()
        {
            Instance = this;
            InitUIRoot<UIRoot>(GetType().Name, 999, (int) EUIGroupType.Max);
            UIRoot.Canvas.pixelPerfect = false;

            CanvasScaler cs = UIRoot.GetComponent<CanvasScaler>();
            cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            cs.referenceResolution = new Vector2(UIConstDefine.UINormalScreenWidth, UIConstDefine.UINormalScreenHeight);
            cs.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            cs.matchWidthOrHeight = 0.432f;

            InitUI(GetType());
            StartCoroutine(ProcessUIAutoSetup());
        }

        public override T OpenUI<T>(object value = null)
        {
            if (UIRoot == null) return null;
            var ui = UIRoot.GetUI(typeof(T));
            if (null == ui) return null;
            if (!ui.IsViewCreated)
            {
                UIRoot.CreateUI(typeof(T));
            }

            return base.OpenUI<T>(value);
        }

        public static void ShowPopupDialogFormat(string format, params object[] val)
        {
            ShowPopupDialog(string.Format(format, val));
        }

        public static void ShowPopupDialog(string msg, string title = null,
            params KeyValuePair<string, Action>[] btnParam)
        {
            Messenger<string, string, KeyValuePair<string, Action>[]>.Broadcast(EMessengerType.ShowDialog, msg, title,
                btnParam);
        }
    }

    public enum EUIGroupType
    {
        None = -1,
        MainUI,
        Pop1,
        Pop2,
        Pop3,
        PopUpDialog,
        Max
    }
}