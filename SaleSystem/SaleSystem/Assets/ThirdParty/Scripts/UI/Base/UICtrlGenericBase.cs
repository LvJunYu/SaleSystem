using MyTools;

namespace UITools
{
    public abstract class UICtrlGenericBase<T> : UICtrlBase where T : UIViewBase
    {
        protected T _cachedView;

        protected override void OnViewCreated()
        {
            _cachedView = _view as T;
            if (_cachedView == null)
            {
                LogHelper.Error("{0} OnViewCreated failed _view is invalid!", GetType());
            }
        }

        protected override void OnDestroy()
        {
            _cachedView = null;
            base.OnDestroy();
        }
    }
}