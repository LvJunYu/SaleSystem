using UnityEngine;
using UnityEngine.EventSystems;

namespace UITools
{
    [System.Serializable]
    public class UIEventSystem
    {
        [SerializeField]
        private EventSystem _eventSystem;
        [SerializeField]
        private StandaloneInputModule _standaloneInputModule;
        [SerializeField]
        private Transform _trans;

        public Transform Trans
        {
            get { return _trans; }
        }

        public EventSystem EventSystem
        {
            get { return _eventSystem; }
        }

        public StandaloneInputModule StandaloneInputModule
        {
            get { return _standaloneInputModule; }
        }

        public void Init()
        {
            var go = new GameObject("EventSystem") { layer = UIConstDefine.EUILayer };
            _trans = go.transform;
            _eventSystem = go.AddComponent<EventSystem>();
            _eventSystem.sendNavigationEvents = false;
            _eventSystem.pixelDragThreshold = 5;
            _standaloneInputModule = go.AddComponent<StandaloneInputModule>();
            _standaloneInputModule.horizontalAxis = "Horizontal";
            _standaloneInputModule.verticalAxis = "Vertical";
            _standaloneInputModule.submitButton = "Submit";
            _standaloneInputModule.cancelButton = "Cancel";
            _standaloneInputModule.inputActionsPerSecond = 10;
            _standaloneInputModule.forceModuleActive = false;
        }
    }
}