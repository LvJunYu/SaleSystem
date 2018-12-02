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
            SaleDataManager.Instance.LoadData();
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
            SaleDataManager.Instance.SaveData();
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