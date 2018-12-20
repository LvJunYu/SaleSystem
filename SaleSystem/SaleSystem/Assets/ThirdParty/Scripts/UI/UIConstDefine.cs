namespace UITools
{
    public class UIConstDefine
    {
        public const int EUILayer = (int)ELayer.UI;
        public const int UINormalScreenWidth = 1280;//     标准 屏幕宽
        public const int UINormalScreenHeight = 720;//     标准屏幕高
        public const string UIPath = "UI/";
        public const string UIItemPath = "UIItem/";
    }

    public enum ELayer
    {
        Default = 0,
        UI = 5,
        RenderUI = 8,
        LightDark = 16,
        Light = 17,
        Dark = 18,
    }

    public enum ECameraLayer
    {
        MainCamera = 0,
        GameUICamera = 2,
        AppUICamera = 5,
    }
}