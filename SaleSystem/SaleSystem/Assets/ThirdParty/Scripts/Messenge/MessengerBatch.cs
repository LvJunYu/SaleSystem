using System;
using System.Collections.Generic;

namespace MyTools
{
    public class MessengerEventBatch
    {
        private static MessengerEventBatch _current;
        private static readonly List<Action> UnRegisterActionTempList = new List<Action>(16);
        private Action[] _unRegisterActionAry;

        private MessengerEventBatch()
        {
        }

        public static MessengerEventBatch Begin()
        {
            if (_current != null)
            {
                LogHelper.Error("MessengerBatch has Began");
                return null;
            }

            _current = new MessengerEventBatch();
            return _current;
        }

        public MessengerEventBatch RegisterEvent(int mt, Callback callback)
        {
            if (_current != this)
            {
                LogHelper.Error("MessengerEventBatch RegisterEvent Without Begin");
                return this;
            }

            if (callback == null)
            {
                LogHelper.Error("string.IsNullOrEmpty(messenger) || callback == null");
                return this;
            }

            Messenger.AddListener(mt, callback);
            UnRegisterActionTempList.Add(() => Messenger.RemoveListener(mt, callback));
            return this;
        }

        public MessengerEventBatch RegisterEvent<A>(int mt, Callback<A> callback)
        {
            if (_current != this)
            {
                LogHelper.Error("MessengerEventBatch RegisterEvent Without Begin");
                return this;
            }

            if (callback == null)
            {
                LogHelper.Error("string.IsNullOrEmpty(messenger) || callback == null");
                return this;
            }

            Messenger<A>.AddListener(mt, callback);
            UnRegisterActionTempList.Add(() => Messenger<A>.RemoveListener(mt, callback));
            return this;
        }

        public MessengerEventBatch RegisterEvent<A, B>(int mt, Callback<A, B> callback)
        {
            if (_current != this)
            {
                LogHelper.Error("MessengerEventBatch RegisterEvent Without Begin");
                return this;
            }

            if (callback == null)
            {
                LogHelper.Error("string.IsNullOrEmpty(messenger) || callback == null");
                return this;
            }

            Messenger<A, B>.AddListener(mt, callback);
            UnRegisterActionTempList.Add(() => Messenger<A, B>.RemoveListener(mt, callback));
            return this;
        }

        public MessengerEventBatch RegisterEvent<A, B, C>(int mt, Callback<A, B, C> callback)
        {
            if (_current != this)
            {
                LogHelper.Error("MessengerEventBatch RegisterEvent Without Begin");
                return this;
            }

            if (callback == null)
            {
                LogHelper.Error("string.IsNullOrEmpty(messenger) || callback == null");
                return this;
            }

            Messenger<A, B, C>.AddListener(mt, callback);
            UnRegisterActionTempList.Add(() => Messenger<A, B, C>.RemoveListener(mt, callback));
            return this;
        }

        public MessengerEventBatch End()
        {
            if (_current != this)
            {
                LogHelper.Error("MessengerEventBatch End Without Begin");
                return this;
            }

            if (UnRegisterActionTempList.Count != 0)
            {
                _unRegisterActionAry = UnRegisterActionTempList.ToArray();
                UnRegisterActionTempList.Clear();
            }

            _current = null;
            return this;
        }

        public void UnregisterAllEvent()
        {
            if (_unRegisterActionAry == null)
            {
                return;
            }

            for (int i = 0; i < _unRegisterActionAry.Length; i++)
            {
                try
                {
                    _unRegisterActionAry[i].Invoke();
                }
                catch (Exception e)
                {
                    LogHelper.Warning(e.ToString());
                }
            }

            _unRegisterActionAry = null;
        }
    }
}