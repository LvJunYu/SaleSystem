using System.Collections.Generic;

namespace MyTools
{
    public delegate void Callback();

    public delegate void Callback<T>(T arg1);

    public delegate void Callback<T, U>(T arg1, U arg2);

    public delegate void Callback<T, U, V>(T arg1, U arg2, V arg3);


    public static class Messenger
    {
        #region 常量与字段

        private static readonly Dictionary<int, Callback> _dict = new Dictionary<int, Callback>();

        #endregion

        #region 方法

        public static void AddListener(int eventType, Callback handler)
        {
            Callback c = null;
            if (_dict.TryGetValue(eventType, out c))
            {
                _dict[eventType] = c + handler;
            }
            else
            {
                _dict.Add(eventType, handler);
            }
        }

        public static void RemoveListener(int eventType, Callback handler)
        {
            Callback c = null;
            if (!_dict.TryGetValue(eventType, out c))
            {
                LogHelper.Error("Attempting to remove listener with for event type {0} but current listener is null.",
                    eventType);
                return;
            }
            _dict[eventType] = c - handler;
        }

        public static void Broadcast(int eventType)
        {
            Callback c = null;
            if (_dict.TryGetValue(eventType, out c) && c != null)
            {
                c.Invoke();
            }
        }

        #endregion
    }

    public static class Messenger<T>
    {
        #region 常量与字段

        private static readonly Dictionary<int, Callback<T>> _dict = new Dictionary<int, Callback<T>>();

        #endregion

        #region 方法

        public static void AddListener(int eventType, Callback<T> handler)
        {
            Callback<T> c = null;
            if (_dict.TryGetValue(eventType, out c))
            {
                _dict[eventType] = c + handler;
            }
            else
            {
                _dict.Add(eventType, handler);
            }
        }

        public static void RemoveListener(int eventType, Callback<T> handler)
        {
            Callback<T> c = null;
            if (!_dict.TryGetValue(eventType, out c))
            {
                LogHelper.Error("Attempting to remove listener with for event type {0} but current listener is null.",
                    eventType);
                return;
            }
            _dict[eventType] = c - handler;
        }

        public static void Broadcast(int eventType, T arg1)
        {
            Callback<T> c = null;
            if (_dict.TryGetValue(eventType, out c) && c != null)
            {
                c.Invoke(arg1);
            }
        }

        #endregion
    }

    public static class Messenger<T, U>
    {
        #region 常量与字段

        private static readonly Dictionary<int, Callback<T, U>> _dict = new Dictionary<int, Callback<T, U>>();

        #endregion

        #region 方法

        public static void AddListener(int eventType, Callback<T, U> handler)
        {
            Callback<T, U> c = null;
            if (_dict.TryGetValue(eventType, out c))
            {
                _dict[eventType] = c + handler;
            }
            else
            {
                _dict.Add(eventType, handler);
            }
        }

        public static void RemoveListener(int eventType, Callback<T, U> handler)
        {
            Callback<T, U> c = null;
            if (!_dict.TryGetValue(eventType, out c))
            {
                LogHelper.Error("Attempting to remove listener with for event type {0} but current listener is null.",
                    eventType);
                return;
            }
            _dict[eventType] = c - handler;
        }

        public static void Broadcast(int eventType, T arg1, U arg2)
        {
            Callback<T, U> c = null;
            if (_dict.TryGetValue(eventType, out c) && c != null)
            {
                c.Invoke(arg1, arg2);
            }
        }

        #endregion
    }

    public static class Messenger<T, U, V>
    {
        #region 常量与字段

        private static readonly Dictionary<int, Callback<T, U, V>> _dict = new Dictionary<int, Callback<T, U, V>>();

        #endregion

        #region 方法

        public static void AddListener(int eventType, Callback<T, U, V> handler)
        {
            Callback<T, U, V> c = null;
            if (_dict.TryGetValue(eventType, out c))
            {
                _dict[eventType] = c + handler;
            }
            else
            {
                _dict.Add(eventType, handler);
            }
        }

        public static void RemoveListener(int eventType, Callback<T, U, V> handler)
        {
            Callback<T, U, V> c = null;
            if (!_dict.TryGetValue(eventType, out c))
            {
                LogHelper.Error("Attempting to remove listener with for event type {0} but current listener is null.",
                    eventType);
                return;
            }
            _dict[eventType] = c - handler;
        }

        public static void Broadcast(int eventType, T arg1, U arg2, V arg3)
        {
            Callback<T, U, V> c = null;
            if (_dict.TryGetValue(eventType, out c) && c != null)
            {
                c.Invoke(arg1, arg2, arg3);
            }
        }

        #endregion
    }
}