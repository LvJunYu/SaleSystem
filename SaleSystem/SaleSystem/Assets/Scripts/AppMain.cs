using UITools;
using UnityEngine;

namespace Sale
{
    public class AppMain : MonoBehaviour
    {
        public static AppMain Instance;
        private UIEventSystem _eventSystem;
        private bool _run;

        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            _eventSystem = new UIEventSystem();
            _eventSystem.Init();
            _eventSystem.Trans.SetParent(transform);
            gameObject.AddComponent<SocialGUIManager>();
            SocialGUIManager.Instance.OpenUI<UICtrlLogin>();
            _run = true;
        }

        void Update()
        {
            if (!_run)
            {
                return;
            }
            
        }
    }
}