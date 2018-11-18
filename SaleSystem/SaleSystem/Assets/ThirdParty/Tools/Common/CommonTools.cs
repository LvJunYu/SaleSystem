using UnityEngine;
using Object = UnityEngine.Object;

namespace MyTools
{
    public abstract class CommonTools
    {
        public static GameObject InstantiateObject(Object asset)
        {
            return (GameObject)Object.Instantiate(asset);
        }

        public static void SetAllLayerIncludeHideObj(Transform root, int layer)
        {
            if (root == null)
            {
                LogHelper.Info("SetAllLayerIncludeHideObj, Root is Null!");
                return;
            }
            int tmplayer = layer;
            //if (layer > (int)SceneLayer.Error || layer < 0)
            //{
            //    tmplayer = (int)SceneLayer.Error;
            //}
            root.gameObject.layer = tmplayer;

            int count = root.childCount;
            Transform tmpObj;
            for (int i = 0; i < count; i++)
            {
                tmpObj = root.GetChild(i);
                SetAllLayerIncludeHideObj(tmpObj, tmplayer);
            }
        }
        /// <summary>
        /// 初始化Transform
        /// </summary>
        public static void SetParent(Transform trans, Transform parent)
        {
            if (trans != null && parent != null && trans.parent != parent)
            {
                trans.SetParent(parent);
                ResetTransform(trans);
            }
        }

        /// <summary>
        /// 重置Transform
        /// </summary>
        /// <param name="trans"></param>
        public static void ResetTransform(Transform trans)
        {
            if (trans == null)
            {
                return;
            }
            trans.localPosition = Vector3.zero;
            trans.localRotation = Quaternion.identity;
            trans.localScale = Vector3.one;
        }

        public static void SetActive(GameObject go, bool value)
        {
            if (go == null)
            {
                return;
            }
            go.SetActive(value);
        }

        public static Transform FindChildDeep(Transform root,string name)
        {
            if (root == null)
            {
                return null;
            }
            for (int i = 0; i < root.childCount; i++)
            {
                var trans = root.GetChild(i);
                if (trans.name == name)
                {
                    return trans;
                }
                FindChildDeep(trans, name);
            }
            return null;
        }

        public static T GetOrAddComponent<T>(GameObject go) where T : Component
        {
            T t = go.GetComponent<T>();
            if (t == null)
            {
                t = go.AddComponent<T>();
            }
            return t;
        }
    }
}
