using System;
using MyTools;
using UnityEngine;
using Object = System.Object;

namespace UITools
{
    /// <summary>
    ///     MVP 之 Presenter
    /// </summary>
    [Serializable]
    public abstract class UICtrlBase
    {
        #region 变量

        [SerializeField] protected UIViewBase _view;

        protected RectTransform _uiTrans;

        protected bool _isViewCreated;

        protected int _groupId = 0;

        protected bool _isOpen;

        protected object _openParam;

        private MessengerEventBatch _messengerEventBatch;

        #endregion

        #region 属性

        public bool IsViewCreated
        {
            get { return _isViewCreated; }
        }

        public bool IsOpen
        {
            get { return _isOpen; }
        }

        public RectTransform UITrans
        {
            get { return _uiTrans; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int GroupId
        {
            get { return _groupId; }
        }

        public int OrderOfView
        {
            get { return _groupId * 100 + _view.transform.GetSiblingIndex(); }
        }

        #endregion

        #region 方法

        /// <summary>
        /// 相当于构造函数
        /// </summary>
        public void Awake()
        {
            OnAwake();
            InitGroupId();
            InitEventListener();
            AddAllListeners();
        }

        /// <summary>
        ///     view created
        /// </summary>
        public void SetView(UIViewBase view)
        {
            if (view == null)
            {
                return;
            }

            _view = view;
            _uiTrans = _view.Trans;
            _isViewCreated = true;
            if (!_view.gameObject.activeSelf)
            {
                _view.gameObject.SetActive(true);
            }

            OnViewCreated();
            _view.gameObject.SetActive(false);
            if (_isOpen)
            {
                Open(_openParam);
            }
        }

        /// <summary>
        ///     删除UI
        /// </summary>
        public void Destroy()
        {
            OnDestroy();
            if (_view != null && _view.gameObject)
            {
                UnityEngine.Object.Destroy(_view.gameObject);
                _view = null;
                _isViewCreated = false;
            }
        }

        public void OnCtrlDestroy()
        {
            RemoveAllListeners();
        }

        public virtual void Open(object parameter)
        {
            if (!_isViewCreated)
            {
                return;
            }

            _isOpen = true;
            _openParam = parameter;
            _view.gameObject.SetActive(true);
            OnOpen(parameter);
        }

        public virtual void Close()
        {
            _isOpen = false;
            _view.gameObject.SetActive(false);
            OnClose();
        }

        #endregion

        #region 回调

        protected virtual void OnAwake()
        {
        }

        protected abstract void InitGroupId();

        protected virtual void InitEventListener()
        {
        }

        protected virtual void OnViewCreated()
        {
        }

        public virtual void OnUpdate()
        {
        }

        protected virtual void OnOpen(Object parameter)
        {
        }

        protected virtual void OnClose()
        {
        }

        protected virtual void OnDestroy()
        {
        }

        #endregion

        #region 事件监听

        protected void RegisterEvent(int mt, Callback callback)
        {
            if (callback == null)
            {
                LogHelper.Error("string.IsNullOrEmpty(messenger) || callback == null");
                return;
            }

            if (_messengerEventBatch == null)
            {
                _messengerEventBatch = MessengerEventBatch.Begin();
            }

            _messengerEventBatch.RegisterEvent(mt, callback);
        }

        protected void RegisterEvent<A>(int mt, Callback<A> callback)
        {
            if (callback == null)
            {
                LogHelper.Error("string.IsNullOrEmpty(messenger) || callback == null");
                return;
            }

            if (_messengerEventBatch == null)
            {
                _messengerEventBatch = MessengerEventBatch.Begin();
            }

            _messengerEventBatch.RegisterEvent(mt, callback);
        }

        protected void RegisterEvent<A, B>(int mt, Callback<A, B> callback)
        {
            if (callback == null)
            {
                LogHelper.Error("string.IsNullOrEmpty(messenger) || callback == null");
                return;
            }

            if (_messengerEventBatch == null)
            {
                _messengerEventBatch = MessengerEventBatch.Begin();
            }

            _messengerEventBatch.RegisterEvent(mt, callback);
        }

        protected void RegisterEvent<A, B, C>(int mt, Callback<A, B, C> callback)
        {
            if (callback == null)
            {
                LogHelper.Error("string.IsNullOrEmpty(messenger) || callback == null");
                return;
            }

            if (_messengerEventBatch == null)
            {
                _messengerEventBatch = MessengerEventBatch.Begin();
            }

            _messengerEventBatch.RegisterEvent(mt, callback);
        }

        private void AddAllListeners()
        {
            if (_messengerEventBatch != null)
            {
                _messengerEventBatch.End();
            }
        }

        private void RemoveAllListeners()
        {
            if (_messengerEventBatch != null)
            {
                _messengerEventBatch.UnregisterAllEvent();
            }
        }

        #endregion
    }
}