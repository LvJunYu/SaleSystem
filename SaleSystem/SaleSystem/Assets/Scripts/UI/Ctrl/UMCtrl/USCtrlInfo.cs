using UITools;

namespace Sale
{
    public class USCtrlInfo : USCtrlBase<USViewInfo>
    {
        public void SetTitle(string title)
        {
            _cachedView.Title.text = title;
        }

        public void SetContent(string content)
        {
            _cachedView.Content.text = content;
        }

        public void SetInfo(string title, string content)
        {
            SetTitle(title);
            SetContent(content);
        }

        public void SetActive(bool b)
        {
            _cachedView.SetActiveEx(b);
        }
    }
}