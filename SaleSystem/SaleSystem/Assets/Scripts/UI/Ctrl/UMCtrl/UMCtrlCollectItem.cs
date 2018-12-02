using MyTools;
using UITools;
using UnityEngine.UI;

namespace Sale
{
    public class UMCtrlCollectItem : UMCtrlGenericBase<UMViewCollectItem>, IDataItemRenderer
    {
        private CollectData _data;
        private Text[] _payTypes;

        protected override void OnViewCreated()
        {
            base.OnViewCreated();
            _payTypes = _cachedView.PayTypeDock.GetComponentsInChildren<Text>(true);
        }

        private void RefreshView()
        {
            _cachedView.TimeTxt.text = DateTimeHelper.GetDateTime(_data.Days).GetDateStr();
            var payTypes = SaleDataManager.Instance.PayTypes;
            for (int i = 0; i < _payTypes.Length; i++)
            {
                _payTypes[i].SetActiveEx(i < payTypes.Count);
            }

            var total = 0;
            var payTypeCount = new int[payTypes.Count];
            for (int i = 0; i < _data.PayRecords.Count; i++)
            {
                var payRecord = _data.PayRecords[i];
                total += payRecord.PayNum;
                var index = payTypes.IndexOf(payRecord.PayType);
                if (index >= 0)
                {
                    payTypeCount[index] += payRecord.PayNum;
                }
            }

            _cachedView.TotalTxt.text = total.ToString();
            for (int i = 0; i < payTypeCount.Length; i++)
            {
                if (i < _payTypes.Length)
                {
                    _payTypes[i].text = payTypeCount[i].ToString();
                }
            }
        }

        public int Index { get; set; }

        public object Data
        {
            get { return _data; }
        }

        public void Set(object data)
        {
            _data = data as CollectData;
            if (_data == null) return;
            RefreshView();
        }

        public void Unload()
        {
        }
    }
}