using MyTools;
using UITools;

namespace Sale
{
    [UIAutoSetup(EUIAutoSetupType.Create)]
    public class UICtrlCollect : UICtrlTapBase<UICtrlCollect, UIViewCollect, UPCtrlCollectBase>
    {
        protected override void OnViewCreated()
        {
            base.OnViewCreated();
            _cachedView.CloseBtn.onClick.AddListener(CloseBtn);
            _menuCtrlArray = new UPCtrlCollectBase[(int) EMenu.Max];
            _menuCtrlArray[(int) EMenu.Day] = CreateUpCtrl<UPCtrlCollectDay>((int) EMenu.Day);
            _menuCtrlArray[(int) EMenu.Month] = CreateUpCtrl<UPCtrlCollectMonth>((int) EMenu.Month);
        }

        protected override void InitEventListener()
        {
            base.InitEventListener();
            RegisterEvent(EMessengerType.OnPayTypeChanged, OnPayTypeChanged);
        }

        protected override void InitGroupId()
        {
            _groupId = (int) EUIGroupType.Pop1;
        }

        protected override void SetPartAnimations()
        {
            base.SetPartAnimations();
            SetPart(_cachedView.PannelRtf, EAnimationType.MoveFromDown);
            SetPart(_cachedView.BGRtf, EAnimationType.Fade);
        }

        private void OnPayTypeChanged()
        {
            ((UPCtrlCollectDay) _menuCtrlArray[(int) EMenu.Day]).InitPayTypes();
        }

        private void CloseBtn()
        {
            SocialGUIManager.Instance.CloseUI<UICtrlCollect>();
        }

        public enum EMenu
        {
            None = -1,
            Day,
            Month,
            Max
        }
    }
}