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
            ProcessUIAutoSetup();
        }

        public void ShowAppView()
        {
//            OpenUI<UICtrlTaskbar>();
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
    }

    public enum EUIGroupType
    {
        None = -1,
        MainUI,
        Pop1,
        Pop2,
        Max
    }
}