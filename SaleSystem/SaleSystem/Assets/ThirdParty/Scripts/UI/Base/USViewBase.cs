using UnityEngine;
using UnityEngine.EventSystems;

namespace UITools
{
    public class USViewBase : UIBehaviour
    {
        protected RectTransform _trans;

        public RectTransform Trans
        {
            get
            {
                if (_trans == null)
                {
                    Awake();
                }

                return _trans;
            }
        }

        protected override void Awake()
        {
            _trans = GetComponent<RectTransform>();
        }
    }
}