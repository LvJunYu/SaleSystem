using System.Collections.Generic;
using System.Linq;

namespace Sale
{
    public class UPCtrlCollectPayType : UPCtrlCollectBase
    {
        private List<int> _payTypeNum = new List<int>();
        private List<string> _payTypeName = new List<string>();
        private Dictionary<string, int> _name2Idx = new Dictionary<string, int>();
        private UMCtrlCollectData _collectCtrl;

        protected override void OnViewCreated()
        {
            base.OnViewCreated();
            _collectCtrl = new UMCtrlCollectData();
            _collectCtrl.Init(_cachedView.Pannels[Menu]);
            _collectCtrl.SetLineName("付款方式", "收入");
        }

        public override void RefreshView()
        {
            var curDate = _mainCtrl.CurDate;
            var monthPayData = SaleDataManager.Instance.CollectHandler.GetMonthPayData(curDate.GetMonths());
            _payTypeNum.Clear();
            _payTypeName.Clear();
            _name2Idx.Clear();
            var payTypes = SaleDataManager.Instance.PayTypes;
            var index = 0;
            foreach (var payType in payTypes)
            {
                _payTypeName.Add(payType);
                _payTypeNum.Add(0);
                _name2Idx.Add(payType, index);
                index++;
            }

            foreach (var payRecord in monthPayData.PayDatas)
            {
                int payTypeIndex;
                if (_name2Idx.TryGetValue(payRecord.PayType, out payTypeIndex))
                {
                    _payTypeNum[payTypeIndex] += payRecord.PayNum;
                }
            }

            var max = _payTypeNum.Max();
            if (max == 0)
            {
                max = 100;
            }

            _collectCtrl.SetData(_payTypeName, _payTypeNum, max);
        }
    }
}