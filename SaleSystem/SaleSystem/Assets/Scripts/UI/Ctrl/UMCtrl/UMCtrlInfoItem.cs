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

        public void SetContentType(InputField.ContentType contentType)
        {
            _cachedView.ContentInputField.contentType = contentType;
        }
        
        public void SetContent(string content)
        {
            _cachedView.ContentInputField.text = content;
        }

        public string GetContent()
        {
            return _cachedView.ContentInputField.text;
        }
    }
}