using System.Collections.Generic;
using UITools;
using UnityEngine.Events;

namespace Sale
{
    public class UMCtrlDropdown : UMCtrlGenericBase<UMViewDropdown>
    {
        private int _value;
        private bool _onlyView;

        protected override void OnViewCreated()
        {
            base.OnViewCreated();
            _cachedView.Dropdown.onValueChanged.AddListener(val =>
            {
                if (!_onlyView)
                {
                    _value = val;
                }
            });
        }

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
            _onlyView = true;
            _cachedView.Dropdown.value = val;
            _value = val;
            _onlyView = false;
        }

        public void AddListener(UnityAction<int> onValChanged)
        {
            _cachedView.Dropdown.onValueChanged.AddListener(val =>
            {
                if (!_onlyView)
                {
                    onValChanged.Invoke(val);
                }
            });
        }

        public int GetVal()
        {
            return _value;
        }

        public string GetContent()
        {
            return _cachedView.Dropdown.itemText.text;
        }
    }
}