using System.Collections.Generic;
using MyTools;
using UITools;
using UnityEngine;
using UnityEngine.UI;

namespace Sale
{
    public class UPCtrlWinScreenSetting : UPCtrlBase<UICtrlSetting, UIViewSetting>
    {
        private List<Dropdown.OptionData> _optionDatas;

        protected override void OnViewCreated()
        {
            base.OnViewCreated();
            _cachedView.FullScreenToggle.onValueChanged.AddListener(OnFullScreenToggleValueChanged);
            _cachedView.ResolutionDropdown.onValueChanged.AddListener(OnResolutionDropdownValueChanged);
        }

        public override void Open()
        {
            base.Open();
            UpdateScreenSettingView();
        }

        private void OnResolutionDropdownValueChanged(int arg0)
        {
            ResolutionManager.Instance.SetResolution(arg0);
        }

        private void OnFullScreenToggleValueChanged(bool arg0)
        {
            ResolutionManager.Instance.SetFullScreen(arg0);
            RefreshOptions(ResolutionManager.Instance.AllResolutionOptions);
            _cachedView.ResolutionDropdown.options = _optionDatas;
            _cachedView.ResolutionDropdown.value = ResolutionManager.Instance.SelectIndex;
        }

        private void UpdateScreenSettingView()
        {
            bool fullScreen = ResolutionManager.Instance.FullScreen;
            _cachedView.WindowScreenToggle.isOn = !fullScreen;
            _cachedView.FullScreenToggle.isOn = fullScreen;
            RefreshOptions(ResolutionManager.Instance.AllResolutionOptions);
            _cachedView.ResolutionDropdown.options = _optionDatas;
            _cachedView.ResolutionDropdown.value = ResolutionManager.Instance.SelectIndex;
        }

        private void RefreshOptions(List<Resolution> resolutions)
        {
            if (_optionDatas == null)
            {
                _optionDatas = new List<Dropdown.OptionData>(resolutions.Count);
            }

            for (int i = 0; i < resolutions.Count; i++)
            {
                var str = ResolutionToString(resolutions[i]);
                if (i < _optionDatas.Count)
                {
                    _optionDatas[i].text = str;
                }
                else
                {
                    _optionDatas.Add(new Dropdown.OptionData(str));
                }
            }

            for (int i = _optionDatas.Count - 1; i >= 0; i--)
            {
                if (i >= resolutions.Count)
                {
                    _optionDatas.RemoveAt(i);
                }
            }
        }

        private string ResolutionToString(Resolution resolution)
        {
            return string.Format("{0}×{1}", resolution.width, resolution.height);
        }
    }
}