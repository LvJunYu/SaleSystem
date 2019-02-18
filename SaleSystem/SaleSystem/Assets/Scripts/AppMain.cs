using System.Collections;
using MyTools;
using UITools;
using UnityEngine;

namespace Sale
{
    public class AppMain : MonoBehaviour
    {
        public static AppMain Instance;
        private UIEventSystem _eventSystem;
        private bool _run;
        private bool _isQuiting;
        private bool _allowQuit;
        [SerializeField] private LogHelper.ELogLevel _logLevel = LogHelper.ELogLevel.Info;

        void Awake()
        {
            Instance = this;
            ResolutionManager.Instance.Init();
            LogHelper.LogLevel = _logLevel;
        }

        void Start()
        {
            _eventSystem = new UIEventSystem();
            _eventSystem.Init();
            _eventSystem.Trans.SetParent(transform);
            SaleDataManager.Instance.Init();
            gameObject.AddComponent<SocialGUIManager>();
            SocialGUIManager.Instance.OpenUI<UICtrlMainApp>();
            _run = true;
            LogHelper.Info("App Run!");
//            if (!Application.isEditor)
//            {
//                Application.wantsToQuit += OnWantsQuit;
//            }
        }

        void OnApplicationFocus(bool hasFocus)
        {
            if (_run && !hasFocus)
            {
//                SaleDataManager.Instance.SaveData();
            }
        }

        IEnumerator OnStart()
        {
            yield return null;
            yield return null;
            SocialGUIManager.Instance.OpenUI<UICtrlMainApp>();
            _run = true;
        }

        IEnumerator SaveDataAndQuit()
        {
            _run = false;
//            SaleDataManager.Instance.SaveData();
            yield return new WaitForSeconds(1);
            _allowQuit = true;
            Application.Quit();
        }

        bool OnWantsQuit()
        {
            if (_allowQuit)
            {
                return true;
            }

            if (!_isQuiting)
            {
                QuitGame();
                _isQuiting = true;
            }

            return false;
        }

        public void QuitGame()
        {
            Application.Quit();
//            StartCoroutine(SaveDataAndQuit());
        }
    }
}