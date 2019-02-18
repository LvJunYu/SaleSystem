using UITools;
using UnityEngine.UI;

namespace Sale
{
    public class UMCtrlInfoItem : UMCtrlGenericBase<UMViewInfoItem>
    {
        public void SetTitle(string title)
        {
            _cachedView.Title.text = title;
        }

        public void SetGuidContent(string content)
        {
            _cachedView.GuidTxt.text = content;
        }

        public void SetGuidActive(bool b)
        {
            _cachedView.GuidTxt.SetActiveEx(b);
        }

        public void SetContentType(InputField.ContentType contentType)
        {
            _cachedView.ContentInputField.contentType = contentType;
        }
        
        public void SetContent(string content)
        {
            _cachedView.ContentInputField.text = content;
        }

        public void SetCharLimit(int limit)
        {
            _cachedView.ContentInputField.characterLimit = limit;
        }

        public string GetContent()
        {
            return _cachedView.ContentInputField.text;
        }
    }
}