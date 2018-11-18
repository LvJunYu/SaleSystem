using System;
using MyTools;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UITools
{
    [Serializable]
    public class UMCtrlBase
    {
        protected bool _isViewCreated;

        /// <summary>
        ///     prefab 名称来源
        /// </summary>
        protected string _prefabName;

        protected UMViewBase _view;

        public UMCtrlBase()
        {
            _prefabName = GetType().Name;
        }

        protected virtual bool Init(RectTransform parent, Vector3 localpos, UIRoot uiRoot)
        {
            if (string.IsNullOrEmpty(_prefabName))
            {
                LogHelper.Error("_prefabName is nullOrEmpty");
                return false;
            }

            if (parent == null)
            {
                LogHelper.Error("parent == null");
                return false;
            }

            _view = uiRoot.InstanceItemView(_prefabName);
            if (_view == null)
            {
                return false;
            }

            if (!_view.gameObject.activeSelf)
            {
                _view.gameObject.SetActive(true);
            }

            CommonTools.SetParent(_view.transform, parent);
            _view.Init();
            _view.Trans.anchoredPosition = localpos;
            _isViewCreated = true;
            OnViewCreated();
            return true;
        }

        protected virtual void OnViewCreated()
        {
        }

        public void Destroy()
        {
            OnDestroy();
            if (_view != null)
            {
                Object.Destroy(_view.gameObject);
            }
        }

        protected virtual void OnDestroy()
        {
        }
    }
}