using UITools;
using UnityEngine;

namespace Sale
{
    public class UMCtrlCollectDataItem : UMCtrlGenericBase<UMViewCollectDataItem>
    {
        private float _maxHeight;

        protected override void OnViewCreated()
        {
            base.OnViewCreated();
            _maxHeight = ((RectTransform) _cachedView.Trans.parent).GetHeight();
        }

        public void SetData(string info, int num, int maxNum)
        {
            _cachedView.InfoTxt.text = info;
            _cachedView.NumTxt.text = num == 0? string.Empty: num.ToString();
            _cachedView.Trans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _maxHeight * num / maxNum);
        }

        public void SetActive(bool b)
        {
            _cachedView.SetActiveEx(b);
        }

        public void SetWith(int width)
        {
            _cachedView.Trans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        }

        public void HideInfo()
        {
            _cachedView.NumTxt.text = string.Empty;
        }
    }
}