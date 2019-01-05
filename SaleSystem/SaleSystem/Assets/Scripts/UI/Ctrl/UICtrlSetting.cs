using MyTools;
using UITools;

namespace Sale
{
    [UIAutoSetup(EUIAutoSetupType.Create)]
    public class UICtrlSetting : UICtrlAnimationBase<UIViewSetting>
    {
        private UPCtrlWinScreenSetting _upScreenSetting;

        protected override void InitGroupId()
        {
            _groupId = (int) EUIGroupType.Pop1;
        }

        protected override void OnViewCreated()
        {
            base.OnViewCreated();
            _cachedView.CloseBtn.onClick.AddListener(CloseBtn);
            _cachedView.PayTypeSetting.onClick.AddListener(PayTypeSetting);
            _cachedView.RoomSetting.onClick.AddListener(RoomSetting);
            _cachedView.QuitBtn.onClick.AddListener(QuitBtn);
            _cachedView.LoginBtn.onClick.AddListener(LoginBtn);
            _cachedView.OKBtn.onClick.AddListener(OKBtn);
            _upScreenSetting = new UPCtrlWinScreenSetting();
            _upScreenSetting.Init(this, _cachedView);
        }

        private void LoginBtn()
        {
            SocialGUIManager.Instance.CloseUI<UICtrlSetting>();
            SocialGUIManager.Instance.OpenUI<UICtrlLogin>();
        }

        protected override void SetPartAnimations()
        {
            base.SetPartAnimations();
            SetPart(_cachedView.PannelRtf, EAnimationType.MoveFromDown);
            SetPart(_cachedView.BGRtf, EAnimationType.Fade);
        }

        protected override void OnOpen(object parameter)
        {
            base.OnOpen(parameter);
            _upScreenSetting.Open();
        }

        protected override void OnClose()
        {
            base.OnClose();
            _upScreenSetting.Close();
        }

        private void PayTypeSetting()
        {
            SocialGUIManager.Instance.OpenUI<UICtrlChangePayType>();
        }

        private void RoomSetting()
        {
            SocialGUIManager.Instance.OpenUI<UICtrlChangeRoom>();
        }

        private void QuitBtn()
        {
            AppMain.Instance.QuitGame();
        }

        private void OKBtn()
        {
            ResolutionManager.Instance.Save();
            SocialGUIManager.Instance.CloseUI<UICtrlSetting>();
        }

        private void CloseBtn()
        {
            SocialGUIManager.Instance.CloseUI<UICtrlSetting>();
        }
    }
}