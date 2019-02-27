using System;
using MyTools;
using UITools;

namespace Sale
{
    [UIAutoSetup(EUIAutoSetupType.Create)]
    public class UICtrlCollect : UICtrlTapBase<UICtrlCollect, UIViewCollect, UPCtrlCollectBase>
    {
        private DateTime _curDate;

        public DateTime CurDate
        {
            get { return _curDate; }
        }

        protected override void OnViewCreated()
        {
            base.OnViewCreated();
            _cachedView.CloseBtn.onClick.AddListener(CloseBtn);
            _cachedView.LeftBtn.onClick.AddListener(OnPreMonthBtn);
            _cachedView.RightBtn.onClick.AddListener(OnNextMonthBtn);
            _menuCtrlArray = new UPCtrlCollectBase[(int) EMenu.Max];
            _menuCtrlArray[(int) EMenu.Day] = CreateUpCtrl<UPCtrlCollectDay>((int) EMenu.Day);
            _menuCtrlArray[(int) EMenu.Room] = CreateUpCtrl<UPCtrlCollectRoom>((int) EMenu.Room);
            _menuCtrlArray[(int) EMenu.PayType] = CreateUpCtrl<UPCtrlCollectPayType>((int) EMenu.PayType);
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

        protected override void OnOpen(object parameter)
        {
            SetDate(DateTime.Now);
            base.OnOpen(parameter);
        }

        private void SetDate(DateTime date)
        {
            _curDate = date;
            _cachedView.YearTxt.text = _curDate.Year.ToString();
            _cachedView.MonthTxt.text = _curDate.Month.ToString();
            var monthPayData = SaleDataManager.Instance.CollectHandler.GetMonthPayData(_curDate.GetMonths());
            _cachedView.TotalTxt.text = monthPayData == null ? "0" : monthPayData.GetTotalPay().ToString();
        }

        private void OnPreMonthBtn()
        {
            SetDate(_curDate.AddMonths(-1));
            _curMenuCtrl.RefreshView();
        }

        private void OnNextMonthBtn()
        {
            SetDate(_curDate.AddMonths(1));
            _curMenuCtrl.RefreshView();
        }

        private void OnPayTypeChanged()
        {
        }

        private void CloseBtn()
        {
            SocialGUIManager.Instance.CloseUI<UICtrlCollect>();
        }

        public enum EMenu
        {
            None = -1,
            Day,
            Room,
            PayType,
            Max
        }
    }
}