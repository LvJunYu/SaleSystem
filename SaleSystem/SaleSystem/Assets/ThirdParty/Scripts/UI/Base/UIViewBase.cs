using UnityEngine;
using UnityEngine.EventSystems;

namespace UITools
{
    /// <summary>
    ///     MVP 之 View
    /// </summary>
    public class UIViewBase : UIBehaviour
    {
        private RectTransform _trans;

        public int SortIndex = 0;

        public RectTransform Trans
        {
            get { return _trans; }
        }

        public void Init()
        {
            _trans = gameObject.GetComponent<RectTransform>();
        }
    }
}