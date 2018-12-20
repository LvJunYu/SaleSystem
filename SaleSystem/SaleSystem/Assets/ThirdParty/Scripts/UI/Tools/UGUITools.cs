using MyTools;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UITools
{
    public class UGUITools
    {
        /// <summary>
        ///     创建一个UI空物体 制定父物体
        /// </summary>
        /// <param name="trans"></param>
        /// <returns></returns>
        private static GameObject CreateEmptyUIChild(Transform trans)
        {
            var go = new GameObject();
            CommonTools.SetParent(go.transform, trans);
            go.AddComponent<RectTransform>();
            CommonTools.SetAllLayerIncludeHideObj(go.transform, trans.gameObject.layer);
            return go;
        }

        public static RectTransform CreateUIGroupObject(Transform parent)
        {
            if (parent == null)
            {
                LogHelper.Error("CreateUIGroupObject called but parent is null!");
                return null;
            }
            var go = CreateEmptyUIChild(parent);
            var trans = go.GetComponent<RectTransform>();
            trans.pivot = new UnityEngine.Vector2(0.5f, 0.5f);
            trans.anchorMin = UnityEngine.Vector2.zero;
            trans.anchorMax = UnityEngine.Vector2.one;
            trans.sizeDelta = UnityEngine.Vector2.zero;
            trans.localPosition = Vector3.zero;
            return trans;
        }

        /// <summary>
        ///     创建一个ui子物体 并且添加脚本 返回脚本
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="trans"></param>
        /// <returns></returns>
        public static T AddUIChildTo<T>(Transform trans) where T : UIBehaviour
        {
            GameObject go = CreateEmptyUIChild(trans);
            if (go == null)
            {
                LogHelper.Error("AddUIChildTo called but CreateEmptyUIChild return null!");
                return null;
            }
            return go.AddComponent<T>();
        }
    }
}