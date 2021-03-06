﻿namespace UITools
{
    public class UPCtrlBase<C, V> where C:UICtrlBase where V:UIViewBase
	{
		protected V _cachedView;
        protected C _mainCtrl;
        protected bool _isOpen = false;

        public virtual void Init(C ctrl, V view)
		{
			_cachedView = view;
            _mainCtrl = ctrl;
			OnViewCreated ();
		}
		
		protected virtual void OnViewCreated()
		{
		}

		public virtual void OnDestroy()
		{
			
		}

        public virtual void Open() { _isOpen = true; }
        public virtual void Close() { _isOpen = false; }
	}
}

