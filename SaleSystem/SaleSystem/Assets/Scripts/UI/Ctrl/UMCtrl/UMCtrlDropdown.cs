using System.Collections.Generic;
using UITools;
using UnityEngine.Events;

namespace Sale
{
    public class UMCtrlDropdown : UMCtrlGenericBase<UMViewDropdown>
    {
        public void SetOptions(List<string> options)
        {
            _cachedView.Dropdown.ClearOptions();
            _cachedView.Dropdown.AddOptions(options);
        }

        public void SetTitle(string title)
        {
            _cachedView.Title.text = title;
        }

        public void SetCurVal(int val)
        {
            _cachedView.Dropdown.value = val;
        }

        public void AddListener(UnityAction<int> onValChanged)
        {
            _cachedView.Dropdown.onValueChanged.AddListener(onValChanged);
        }

        public int GetVal()
        {
            return _cachedView.Dropdown.value;
        }

        public string GetContent()
        {
            return _cachedView.Dropdown.itemText.text;
        }
    }
}