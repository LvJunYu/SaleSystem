using MyTools;
using Sale;
using UnityEngine;

namespace UITools
{
    public class UMCtrlGenericBase<T> : UMCtrlBase where T : UMViewBase
    {
        protected T _cachedView;

        protected override void OnViewCreated()
        {
            base.OnViewCreated();
            _cachedView = (T) _view;
            if (_cachedView == null)
            {
                LogHelper.Error("{0} SetView failed _view is invalid!", GetType());
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _cachedView = null;
        }

        public void Init(RectTransform parent)
        {
            Init(parent, Vector3.zero, SocialGUIManager.Instance.UIRoot);
        }
    }
}