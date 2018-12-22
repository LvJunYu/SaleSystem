using UnityEngine;
using Object = UnityEngine.Object;

namespace UITools
{
    public static class CommonTools
    {
        public static GameObject InstantiateObject(Object asset)
        {
            return (GameObject)Object.Instantiate(asset);
        }

        public static void SetAllLayerIncludeHideObj(Transform root, int layer)
        {
            if (root == null)
            {
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
        
        public static void SetActiveEx(this GameObject go, bool value)
        {
            if (go != null && go.activeSelf != value)
            {
                go.SetActive(value);
            }
        }

        public static void SetActiveEx(this Component com, bool value)
        {
            if (com != null && com.gameObject && com.gameObject.activeSelf != value)
            {
                com.gameObject.SetActive(value);
            }
        }

        public static void SetEnableEx(this MonoBehaviour com, bool value)
        {
            if (com != null)
            {
                com.enabled = value;
            }
        }

    }
}
