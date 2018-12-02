using UITools;
using UnityEngine.Events;

namespace Sale
{
    public class UMCtrlPayInfo : UMCtrlGenericBase<UMViewPayInfo>
    {
        public void SetTitle(string title)
        {
            _cachedView.Title.text = title;
        }

        public void SetContent(string content)
        {
            _cachedView.CountTxt.text = content;
        }

        public void AddBtnListener(UnityAction action)
        {
            _cachedView.PayBtn.onClick.AddListener(action);
        }
    }
}