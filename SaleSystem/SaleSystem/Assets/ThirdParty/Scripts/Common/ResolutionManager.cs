using System.Collections.Generic;
using UnityEngine;

namespace MyTools
{
    public class ResolutionManager
    {
        public static Resolution[] StandardResolutions =
        {
            new Resolution
            {
                width = 1920,
                height = 1080
            },
            new Resolution
            {
                width = 1920,
                height = 1200
            },
            new Resolution
            {
                width = 1600,
                height = 900
            },
            new Resolution
            {
                width = 1600,
                height = 1000
            },
            new Resolution
            {
                width = 1440,
                height = 810
            },
            new Resolution
            {
                width = 1440,
                height = 900
            },
            new Resolution
            {
                width = 1280,
                height = 720
            },
            new Resolution
            {
                width = 1280,
                height = 800
            },
            new Resolution
            {
                width = 1024,
                height = 576
            },
            new Resolution
            {
                width = 1024,
                height = 640
            },
            new Resolution
            {
                width = 960,
                height = 540
            },
            new Resolution
            {
                width = 960,
                height = 600
            },
        };

        private const int WindowEdgeHeight = 80; //窗口边缘高度，用于计算窗口高度最大值，防止超出屏幕
        private static ResolutionManager _instance;

        public static ResolutionManager Instance
        {
            get { return _instance ?? (_instance = new ResolutionManager()); }
        }

        private List<Resolution> _allResolutions = new List<Resolution>();
        private List<Resolution> _fullScreenResolutions = new List<Resolution>();
        private Resolution _curResolution;
        private int _curResolutionIndex;
        private bool _fullScreen;
        private bool _beyondBoard;

        private const string FullScreenTag = "FullScreenTag";
        private bool _selectFullScreen;

        private int _selectIndex;

        public Resolution CurRealResolution
        {
            get { return _curResolution; }
        }

        public List<Resolution> AllResolutionOptions
        {
            get
            {
                if (_selectFullScreen)
                {
                    return _fullScreenResolutions;
                }

                return _allResolutions;
            }
        }

        public bool FullScreen
        {
            get { return _fullScreen; }
        }

        public int SelectIndex
        {
            get { return _selectIndex; }
        }

        private ResolutionManager()
        {
            GetAllResolutions();
            Load();
            ClearChange();
        }

        private void GetAllResolutions()
        {
            var fullScreenResolution = Screen.currentResolution;
            if (!CheckResolution(fullScreenResolution.width, fullScreenResolution.height))
            {
                ClampScreen(ref fullScreenResolution);
            }

            _fullScreenResolutions.Add(fullScreenResolution);
            LogHelper.Debug("Screen.currentResolution is {0}", Screen.currentResolution);
            for (int i = 0; i < StandardResolutions.Length; i++)
            {
                int width = StandardResolutions[i].width;
                int height = StandardResolutions[i].height;
                //分辨率高度必须小于屏幕分辨率高度，否则窗口会超出屏幕
                if (!CheckResolution(width, height))
                {
                    continue;
                }

                bool hasAdd = false;
                //检查是否重复
                for (int j = 0; j < _allResolutions.Count; j++)
                {
                    if (CheckSameResolution(StandardResolutions[i], _allResolutions[j]))
                    {
                        hasAdd = true;
                        break;
                    }
                }

                if (!hasAdd)
                {
                    _allResolutions.Add(StandardResolutions[i]);
                }
            }

            LogHelper.Debug("Get {0} suitable resolutions", _allResolutions.Count);
            if (_allResolutions.Count == 0)
            {
                LogHelper.Warning("Can not find a suitable resolutions");
            }

            _allResolutions.Sort((p, q) => q.width * 1000 + p.height - p.width * 1000 - q.height);
        }

        private void ClampScreen(ref Resolution fullScreenResolution)
        {
            int height = fullScreenResolution.height / 90 * 90;
            int width = height / 9 * 16;
            if (width > fullScreenResolution.width)
            {
                width = fullScreenResolution.width / 160 * 160;
                height = width / 16 * 9;
            }

            fullScreenResolution.height = height;
            fullScreenResolution.width = width;
        }

        private void Load()
        {
            if (PlayerPrefs.HasKey(FullScreenTag))
            {
                _fullScreen = PlayerPrefs.GetInt(FullScreenTag) != 0;
            }
            else
            {
                _fullScreen = false;
            }

            if (_fullScreen)
            {
                _curResolutionIndex = 0;
                _curResolution = _fullScreenResolutions[0];
                return;
            }

            _curResolution = new Resolution();
            //设置默认分辨率
            _curResolution.width = Screen.width;
            _curResolution.height = Screen.height;
            _curResolutionIndex = IndexOfResolutions(_curResolution, _allResolutions);
            if (_curResolutionIndex >= 0)
            {
                _curResolution = _allResolutions[_curResolutionIndex];
                LogHelper.Info("Cur resolution is {0} ", _curResolution);
            }
            else
            {
                if (_allResolutions.Count == 0)
                {
                    LogHelper.Warning("Can not find a suitable resulution, Screen.currentResolution is {0}",
                        Screen.currentResolution);
                    var resolution = Screen.currentResolution;
                    resolution.height -= WindowEdgeHeight;
                    ClampScreen(ref resolution);
                    _allResolutions.Add(resolution);
                    _curResolutionIndex = 0;
                    SetResolution(resolution, _fullScreen);
                }
                else
                {
                    _curResolutionIndex = 0;
                    if (_fullScreen)
                    {
                        SetResolution(_fullScreenResolutions[_curResolutionIndex], _fullScreen);
                    }
                    else
                    {
                        SetResolution(_allResolutions[_curResolutionIndex], _fullScreen);
                    }
                }

                LogHelper.Info("Set resolution {0} ", _curResolution);
            }
        }

        private bool CheckResolution(int width, int height)
        {
            return width <= Screen.currentResolution.width &&
                   height <= Screen.currentResolution.height - WindowEdgeHeight;
        }

        public void Init()
        {
        }

        public void Save()
        {
            bool needSave = false;
            if (_selectFullScreen != _fullScreen)
            {
                needSave = true;
                _fullScreen = _selectFullScreen;
                PlayerPrefs.SetInt(FullScreenTag, _fullScreen ? 1 : 0);
                if (_selectFullScreen)
                {
                    _curResolution = _fullScreenResolutions[0];
                    _curResolutionIndex = 0;
                }
                else
                {
                    _curResolution = _allResolutions[_selectIndex];
                    _curResolutionIndex = _selectIndex;
                }
            }
            else if (!_selectFullScreen && _selectIndex != _curResolutionIndex)
            {
                needSave = true;
                _curResolutionIndex = _selectIndex;
                _curResolution = _allResolutions[_curResolutionIndex];
            }

            if (needSave)
            {
                SetResolution(_curResolution, _fullScreen);
            }
        }

        public void ClearChange()
        {
            _selectIndex = _curResolutionIndex;
            _selectFullScreen = _fullScreen;
        }

        public void SetFullScreen(bool value)
        {
//            if (_selectFullScreen != value)
            {
                _selectFullScreen = value;
                if (value)
                {
                    _selectIndex = 0;
                }
                else
                {
                    _selectIndex = _curResolutionIndex;
                }
            }
        }

        public void SetResolution(Resolution resolution, bool fullScreen)
        {
            _curResolution = resolution;
            _fullScreen = fullScreen;
            Screen.SetResolution(resolution.width, resolution.height, fullScreen);
        }

        public void SetResolution(int index)
        {
            if (index >= _allResolutions.Count)
            {
                LogHelper.Error("resolutionIndex > _allResolutions.Count");
                return;
            }

            _selectIndex = index;
//            _curResolutionIndex = index;
//            SetResolution(_allResolutions[index], _fullScreen);
        }

        private bool CheckSameResolution(Resolution r1, Resolution r2)
        {
            return r1.width == r2.width && r1.height == r2.height;
        }

        private int IndexOfResolutions(Resolution r, List<Resolution> resolutions)
        {
            for (int i = 0; i < resolutions.Count; i++)
            {
                if (CheckSameResolution(resolutions[i], r))
                {
                    return i;
                }
            }

            return -1;
        }
    }
}