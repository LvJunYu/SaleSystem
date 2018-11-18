using System;
using System.Collections.Generic;
using UnityEngine;

namespace UITools
{
    public class GUIManager : MonoBehaviour
    {
        protected readonly Dictionary<Type, UIAutoSetupAttribute> UITypeAttributeDict =
            new Dictionary<Type, UIAutoSetupAttribute>();

        public UIRoot UIRoot { get; protected set; }

        protected void InitUIRoot<T>(string rootName, int sortOrder, int groupCount) where T : UIRoot
        {
            //初始化UIRoot
            UIRoot = new GameObject(rootName) {layer = UIConstDefine.EUILayer}.AddComponent<T>();
            UIRoot.Init(sortOrder, groupCount, transform);
        }

        protected virtual void InitUI(Type t)
        {
            Type attributeType = typeof(UIAutoSetupAttribute);
            Type[] types = t.Assembly.GetTypes();
            for (int i = 0; i < types.Length; i++)
            {
                Type type = types[i];
                if (Attribute.IsDefined(type, attributeType) && type.Namespace == t.Namespace)
                {
                    Attribute[] attributes = Attribute.GetCustomAttributes(type, attributeType);
                    int attributeLength = attributes.Length;
                    for (int j = 0; j < attributeLength; j++)
                    {
                        var attribute = attributes[j] as UIAutoSetupAttribute;
                        if (attribute != null)
                        {
                            UITypeAttributeDict.Add(type, attribute);
                            //LogHelper.Debug(type.ToString());
                        }
                    }
                }
            }

            //第一遍把所有Ctrl都创建了 第二遍创建视图 第三遍打开UI 防止中间互相引用导致错误
            using (var itor = UITypeAttributeDict.GetEnumerator())
            {
                while (itor.MoveNext())
                {
                    CreateCtrl(itor.Current.Key);
                }
            }
        }

        protected virtual void ProcessUIAutoSetup()
        {
            //第一遍把所有Ctrl都创建了 第二遍创建视图 第三遍打开UI 防止中间互相引用导致错误
            using (var itor = UITypeAttributeDict.GetEnumerator())
            {
                while (itor.MoveNext())
                {
                    var autoSetup = itor.Current.Value;
                    if (autoSetup.AutoSetupType == EUIAutoSetupType.Create
                        || autoSetup.AutoSetupType == EUIAutoSetupType.Show)
                    {
                        CreateView(itor.Current.Key);
                    }
                }
            }

            using (var itor = UITypeAttributeDict.GetEnumerator())
            {
                while (itor.MoveNext())
                {
                    var autoSetup = itor.Current.Value;
                    if (autoSetup.AutoSetupType == EUIAutoSetupType.Show)
                    {
                        OpenUI(itor.Current.Key);
                    }
                }
            }
        }

        protected virtual void Update()
        {
            UIRoot.OnUpdate();
        }

        /// <summary>
        ///     清除所有UI
        /// </summary>
        public void Clear()
        {
            if (UIRoot != null)
            {
                UIRoot.Clear();
            }
        }

        /// <summary>
        ///     通过类型获取对象 默认自动创建
        /// </summary>
        /// <returns></returns>
        public T GetUI<T>() where T : UICtrlBase
        {
            return GetUI(typeof(T)) as T;
        }

        public UICtrlBase GetUI(Type ctrlType)
        {
            if (UIRoot == null)
            {
                return null;
            }

            return UIRoot.GetUI(ctrlType);
        }

        public virtual T OpenUI<T>(object value = null) where T : UICtrlBase
        {
            return OpenUI(typeof(T), value) as T;
        }

        public virtual UICtrlBase OpenUI(Type ctrlType, object value = null)
        {
            if (UIRoot == null)
            {
                return null;
            }

            UICtrlBase ctrl = UIRoot.GetUI(ctrlType);
            if (ctrl.IsViewCreated)
            {
                ctrl.Open(value);
            }
            else
            {
                UIRoot.CreateUI(ctrlType);
                ctrl.Open(value);
            }

            return ctrl;
        }

        public virtual T CloseUI<T>() where T : UICtrlBase
        {
            return CloseUI(typeof(T)) as T;
        }

        public virtual UICtrlBase CloseUI(Type ctrlType)
        {
            if (UIRoot == null)
            {
                return null;
            }

            UICtrlBase ctrl = UIRoot.GetUI(ctrlType);
            if (ctrl.IsViewCreated)
            {
                ctrl.Close();
            }

            return ctrl;
        }

        /// <summary>
        ///     创建ctrl
        /// </summary>
        /// <typeparam path="T"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T CreateCtrl<T>() where T : UICtrlBase
        {
            return CreateCtrl(typeof(T)) as T;
        }

        public UICtrlBase CreateCtrl(Type ctrlType)
        {
            if (UIRoot == null)
            {
                return null;
            }

            return UIRoot.CreateCtrl(ctrlType);
        }

        /// <summary>
        ///     创建Ui, 如果 创建ctrl 的话 会一并创建 view
        /// </summary>
        /// <returns></returns>
        public T CreateView<T>() where T : UICtrlBase
        {
            return CreateView(typeof(T)) as T;
        }

        public UICtrlBase CreateView(Type ctrlType)
        {
            if (UIRoot == null)
            {
                return null;
            }

            return UIRoot.CreateUI(ctrlType);
        }

        /// <summary>
        ///     清除索引
        /// </summary>
        /// <param path="type"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public UICtrlBase Remove(Type type)
        {
            //if (type == null)
            //{
            //    LogHelper.Error("type is null");
            //    return null;
            //}
            //UICtrlBase uiCtrlBase = null;
            //if (_ui.TryGetValue(type, out uiCtrlBase))
            //{
            //    _ui.Remove(type);
            //    uiCtrlBase.OnDestroy();
            //    return uiCtrlBase;
            //}
            return null;
        }

        /// <summary>
        ///     清除索引
        /// </summary>
        /// <typeparam path="T"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Remove<T>() where T : UICtrlBase
        {
            return (T) Remove(typeof(T));
        }

        public static UIAutoSetupAttribute GetUIAutoSetupAttribute(Type type)
        {
            var list = type.GetCustomAttributes(false);
            for (int i = 0; i < list.Length; i++)
            {
                var obj = list[i];
                var attribute = obj as UIAutoSetupAttribute;
                if (attribute != null)
                {
                    return attribute;
                }
            }

            return null;
        }
    }
}