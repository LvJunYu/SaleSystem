using System;
using System.Collections.Generic;
using MyTools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UITools
{
    [Serializable]
    public class UIRoot : UIBehaviour
    {
        public const int UIGroupThickness = 100;
        protected Dictionary<string, UICtrlBase> _allUIs = new Dictionary<string, UICtrlBase>();
        protected Dictionary<string, GameObject> _cachedItemObjects = new Dictionary<string, GameObject>();
        protected CanvasScaler _canvasScaler;
        protected GraphicRaycaster _graphicRaycaster;
        protected RectTransform _trans;
        protected int _uiWidth;
        protected Camera _uiCamera;
        [SerializeField] protected Canvas _canvas;
        [SerializeField] protected SoyUIGroup[] _uiGroups;


        private int _baseSortingOrder;

        public Canvas Canvas
        {
            get { return _canvas; }
        }

        internal void Init(int sortOrder, int groupCount, Transform parent)
        {
            _trans = gameObject.AddComponent<RectTransform>();
            _trans.SetParent(parent);

            InitUICanvas(sortOrder);

            //GraphicRaycaster
            _graphicRaycaster = gameObject.AddComponent<GraphicRaycaster>();
            _graphicRaycaster.ignoreReversedGraphics = true;
            _graphicRaycaster.blockingObjects = GraphicRaycaster.BlockingObjects.None;

            //创建UIGroupObject
            _uiGroups = new SoyUIGroup[groupCount];
            for (int i = 0; i < groupCount; i++)
            {
                var g = new SoyUIGroup();
                g.Trans = UGUITools.CreateUIGroupObject(_trans);
                g.Trans.name = "Depth:" + i;
                var c = g.Trans.gameObject.AddComponent<Canvas>();
                g.Trans.gameObject.AddComponent<GraphicRaycaster>();
                g.RenderCanvas = c;
                g.GroupIndex = i;
                g.StartSortingOrder = _baseSortingOrder + i * UIGroupThickness;
                g.Init();
                _uiGroups[i] = g;
            }

        }

        protected virtual void InitUICanvas(int sortOrder)
        {
            _baseSortingOrder = sortOrder;
            //Canvas
            _canvas = gameObject.AddComponent<Canvas>();
            _canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            _canvas.pixelPerfect = false;
            _canvas.sortingOrder = sortOrder;

            //CanvasScaler
            _canvasScaler = gameObject.AddComponent<CanvasScaler>();
            _canvasScaler.referenceResolution = new Vector2(UIConstDefine.UINormalScreenWidth,
                UIConstDefine.UINormalScreenHeight);
            if (Screen.width <= UIConstDefine.UINormalScreenWidth)
            {
                _uiWidth = UIConstDefine.UINormalScreenWidth;
            }
            else
            {
                _uiWidth = Screen.width;
            }

            _canvasScaler.referencePixelsPerUnit = 128;
            InitUICamera(_canvas);
        }

        private void InitUICamera(Canvas c)
        {
            Transform trans = new GameObject("SocialUICamera").transform;

            _uiCamera = trans.gameObject.AddComponent<Camera>();
            _uiCamera.orthographic = true;
            CoroutineProxy.Instance.StartCoroutine(CoroutineProxy.RunNextFrame(() =>
            {
                _uiCamera.orthographicSize = _trans.GetHeight() * 0.5f;
            }));
            _uiCamera.farClipPlane = 1000;
            _uiCamera.nearClipPlane = -1000;
            _uiCamera.cullingMask = 1 << (int) ELayer.UI;
            _uiCamera.clearFlags = CameraClearFlags.Depth;
            _uiCamera.depth = (int) ECameraLayer.AppUICamera;
            trans.localPosition = new Vector3(-500, -500, 0);

            c.renderMode = RenderMode.ScreenSpaceCamera;
            c.worldCamera = _uiCamera;
            c.planeDistance = 20;
        }

        internal void Clear()
        {
            using (var allUiEnumerator = _allUIs.GetEnumerator())
            {
                while (allUiEnumerator.MoveNext())
                {
                    var item = allUiEnumerator.Current.Value;
                    if (item.IsViewCreated)
                    {
                        if (item.IsOpen)
                        {
                            item.Close();
                        }

                        item.Destroy();
                    }

                    item.OnCtrlDestroy();
                }
            }

            _allUIs.Clear();

            foreach (var itemObject in _cachedItemObjects)
            {
                if (itemObject.Value != null)
                {
                    Destroy(itemObject.Value);
                }
            }

            _cachedItemObjects.Clear();
        }

        public SoyUIGroup GetUIGroup(int index)
        {
            if (_uiGroups == null || index < 0 || index >= _uiGroups.Length)
            {
                return null;
            }

            return _uiGroups[index];
        }

        public T GetUI<T>() where T : UICtrlBase
        {
            return GetUI(typeof(T)) as T;
        }

        public UICtrlBase GetUI(Type type)
        {
            if (type == null)
            {
                LogHelper.Error("GetUI called but type is null!!");
                return null;
            }

            string typeName = type.ToString();
            UICtrlBase x;
            if (_allUIs.TryGetValue(typeName, out x))
            {
                return x;
            }

            return null;
        }

        public T CreateCtrl<T>() where T : UICtrlBase
        {
            return CreateCtrl(typeof(T)) as T;
        }

        public virtual UICtrlBase CreateCtrl(Type type)
        {
            if (type == null)
            {
                LogHelper.Error("CreateCtrl called but type is null!!");
                return null;
            }

            string typeName = type.ToString();
            UICtrlBase ctrl = GetUI(type);
            if (ctrl != null)
            {
                LogHelper.Error("CreateCtrl T {0} but has exist!", typeName);
                return ctrl;
            }

            ctrl = (UICtrlBase) Activator.CreateInstance(type);
            ctrl.Awake();
            _allUIs.Add(typeName, ctrl);
            return ctrl;
        }

        public T CreateUI<T>() where T : UICtrlBase
        {
            return CreateUI(typeof(T)) as T;
        }

        public virtual UICtrlBase CreateUI(Type type)
        {
            UICtrlBase ctrl = GetUI(type);
            //还没创建ctrl 
            if (ctrl == null)
            {
                ctrl = CreateCtrl(type);
            }
            else
            {
                if (ctrl.IsViewCreated)
                {
                    return ctrl;
                }
            }

            if (ctrl == null)
            {
                //做一些处理
                return null;
            }

            UIViewBase view = InstanceView(type);
            if (view == null)
            {
                return null;
            }

            //放在group里面
            CommonTools.SetParent(view.Trans, _uiGroups[ctrl.GroupId].Trans);
            ctrl.SetView(view);
            return ctrl;
        }

        /// <summary>
        ///     暴露给UICtrl,创建UIView的接口 稍后处理这块   创建完毕的view 默认是关闭状态
        /// </summary>
        /// <param name="uictrlType"></param>
        /// <returns></returns>
        protected virtual UIViewBase InstanceView(Type uictrlType)
        {
            var path = uictrlType.Name;
            GameObject go = InstancePrefab(path);
            if (go == null)
            {
                LogHelper.Error("prefab is null");
                return null;
            }

            var view = go.GetComponent<UIViewBase>();
            view.Init();
            view.Trans.SetParent(_trans, false);
            return view;
        }

        /// <summary>
        ///     初始化单项的显示
        /// </summary>
        /// <returns></returns>
        public virtual UMViewBase InstanceItemView(string path)
        {
            GameObject go = InstancePrefab(path, true);
            if (go == null)
            {
                LogHelper.Error("prefab is null");
                return null;
            }

            return go.GetComponent<UMViewBase>();
        }

        /// <summary>
        ///     实例化prefab
        /// </summary>
        /// <returns></returns>
        private GameObject InstancePrefab(string path, bool isItem = false)
        {
            if (string.IsNullOrEmpty(path))
            {
                LogHelper.Error("InstancePrefab called but path IsNullOrEmpty!");
                return null;
            }

            GameObject prefab;
            if (isItem)
            {
                if (!_cachedItemObjects.TryGetValue(path, out prefab) || prefab == null)
                {
                    prefab = (GameObject) Resources.Load(UIConstDefine.UIItemPath + path);
                    if (prefab == null)
                    {
                        LogHelper.Error("{0} prefab doesn't exist.", UIConstDefine.UIItemPath + path);
                        return null;
                    }

                    _cachedItemObjects[path] = prefab;
                    ////释放掉prefab
                    //Resources.UnloadAsset(prefab);
                }

                return Instantiate(prefab);
            }

            prefab = (GameObject) Resources.Load(UIConstDefine.UIPath + path);
            if (prefab == null)
            {
                LogHelper.Error("prefab is null {0} ", path);
                return null;
            }

            return Instantiate(prefab);
        }

        public void SetGroupActive(int layer, bool active)
        {
            if (layer < 0 || layer >= _uiGroups.Length)
            {
                LogHelper.Warning("SetGroupActive, layer error");
                return;
            }

            if (_uiGroups[layer] == null)
            {
                LogHelper.Warning("SetGroupActive, group is null");
                return;
            }

            _uiGroups[layer].Trans.gameObject.SetActive(active);
        }

        public virtual void OnUpdate()
        {
            using (var enumerator = _allUIs.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    var item = enumerator.Current.Value;
                    if (item.IsViewCreated && item.IsOpen)
                    {
                        item.OnUpdate();
                    }
                }
            }
        }
    }
}