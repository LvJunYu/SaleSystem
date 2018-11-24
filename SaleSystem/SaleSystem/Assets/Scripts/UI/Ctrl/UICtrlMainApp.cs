using System.Collections;
using DG.Tweening;
using MyTools;
using UITools;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Sale
{
    [UIAutoSetup]
    public class UICtrlMainApp : UICtrlGenericBase<UIViewMainApp>
    {
        private string[] PointInAnimNames = {"Adventure", "Workshop", "World", "OnLineOpen"};
        private const float EyeInterval = 0.1f;
        private const float WindInterval = 0.04f;
        private const int WindRotate = 3;
        private const int ChilunSpeed = 180;
        private WaitForSeconds _interval = new WaitForSeconds(0.15f);
        private bool _isPointerDown;
        private bool _isPointerEnter;
        private bool _finishOpenShow;
        private Sequence _sequence;
        private float _rabbitEyeTimer = 5;
        private float _eyeCloseTimer;
        private float _windTimer;
        private Sequence[] _sequences;
        private float _notificationTimer;

        protected override void OnViewCreated()
        {
            base.OnViewCreated();
            _cachedView.LeftChiLun.SetActiveEx(false);
            _cachedView.RecordBtn.onClick.AddListener(OnRecordBtn);
            _cachedView.QueryBtn.onClick.AddListener(OnQueryBtn);
            CreateStartSequence();
            CreateCommonSequences();
            for (int i = 0; i < (int) EBtnType.Max; i++)
            {
                var type = (EBtnType) i;
                _cachedView.EventTriggerListeners[i]
                    .AddListener(EventTriggerType.PointerEnter, data => OnPointerEnter(type, true));
                _cachedView.EventTriggerListeners[i]
                    .AddListener(EventTriggerType.PointerExit, data => OnPointerEnter(type, false));
                _cachedView.EventTriggerListeners[i]
                    .AddListener(EventTriggerType.PointerDown, data => OnPointerDown(type, true));
                _cachedView.EventTriggerListeners[i]
                    .AddListener(EventTriggerType.PointerUp, data => OnPointerDown(type, false));
            }

            _windTimer = Random.Range(3, 6f);
        }

        protected override void OnOpen(object parameter)
        {
            base.OnOpen(parameter);
            if (!_finishOpenShow)
            {
                _cachedView.StartCoroutine(OpenShow());
                _finishOpenShow = true;
            }
        }

        protected override void InitGroupId()
        {
            _groupId = (int) EUIGroupType.MainUI;
        }

        public override void OnUpdate()
        {
            UpdateRabbitEye();
            UpdateWind();
            UpdateChiLun();
        }

        private void OnQueryBtn()
        {
        }

        private void OnRecordBtn()
        {
            SocialGUIManager.Instance.OpenUI<UICtrlRecord>();
        }

        private void UpdateChiLun()
        {
            if (_notificationTimer > 0)
            {
                _notificationTimer -= Time.deltaTime;
                _cachedView.RightDownChiLunRtf.Rotate(Vector3.forward, ChilunSpeed * Time.deltaTime);
            }
        }

        private void UpdateRabbitEye()
        {
            if (_rabbitEyeTimer > 0)
            {
                _rabbitEyeTimer -= Time.deltaTime;
            }
            else
            {
                if (_eyeCloseTimer > 0)
                {
                    _eyeCloseTimer -= Time.deltaTime;
                    if (_eyeCloseTimer <= 0)
                    {
                        _rabbitEyeTimer = Random.Range(2, 5f);
                        _cachedView.RabbitEye.SetActiveEx(true);
                    }
                }
                else
                {
                    _cachedView.RabbitEye.SetActiveEx(false);
                    _eyeCloseTimer = EyeInterval;
                }
            }
        }

        private void UpdateWind()
        {
            if (_windTimer > 0)
            {
                _windTimer -= Time.deltaTime;
            }
            else
            {
                _windTimer = Random.Range(2, 5f);
                _sequences[Random.Range(0, _sequences.Length)].PlayForward();
            }
        }

        private IEnumerator OpenShow()
        {
            yield return _interval;
            _cachedView.LeftChiLun.SetActiveEx(true);
            if (_sequence != null)
            {
                _sequence.Restart();
            }

            _cachedView.SkeletonGraphics[(int) EBtnType.Single].SetActiveEx(true);
            _cachedView.SkeletonGraphics[(int) EBtnType.WorkShop].SetActiveEx(true);
            yield return _interval;
            _cachedView.SkeletonGraphics[(int) EBtnType.World].SetActiveEx(true);
            yield return _interval;
            _cachedView.SkeletonGraphics[(int) EBtnType.Multi].SetActiveEx(true);
            _cachedView.RightChiLun.SetActiveEx(true);
        }

        private void CreateStartSequence()
        {
            _sequence = DOTween.Sequence();
            _sequence.Append(
                    _cachedView.Wind1Rtf.DOBlendableMoveBy(Vector3.up * 210, 0.5f).From().SetEase(Ease.OutBack))
                .Join(_cachedView.Wind2Rtf.DOBlendableMoveBy(Vector3.up * 120, 0.5f).From().SetEase(Ease.OutBack))
                .Join(_cachedView.Wind3Rtf.DOBlendableMoveBy(Vector3.up * 120, 0.4f).From().SetEase(Ease.OutBack))
                .SetAutoKill(false).Pause();
        }

        private void CreateCommonSequences()
        {
            _sequences = new Sequence[3];
            for (int i = 0; i < 3; i++)
            {
                var idx = i;
                var rtf = _cachedView.Winds[i];
                _sequences[i] = DOTween.Sequence();
                _sequences[i].Append(rtf.DOBlendableRotateBy(Vector3.forward * WindRotate, WindInterval))
                    .Append(rtf.DOBlendableRotateBy(Vector3.forward * 2 * -WindRotate, WindInterval * 2))
                    .Append(rtf.DOBlendableRotateBy(Vector3.forward * WindRotate, WindInterval))
                    .SetEase(Ease.Linear)
                    .SetLoops(3, LoopType.Restart)
                    .OnComplete(() => _sequences[idx].Rewind())
                    .Pause();
            }
        }

        private void SetBtnScale(EBtnType type, float scale)
        {
            _cachedView.BtnRtfs[(int) type].localScale = Vector3.one * scale;
        }

        private void OnPointerDown(EBtnType type, bool b)
        {
            _isPointerDown = b;
            RefreshBtnScale(type);
        }

        private void RefreshBtnScale(EBtnType btnType)
        {
            if (_isPointerDown)
            {
                SetBtnScale(btnType, 0.98f);
            }
            else if (_isPointerEnter)
            {
                SetBtnScale(btnType, 1.03f);
            }
            else
            {
                SetBtnScale(btnType, 1);
            }
        }

        private void OnPointerEnter(EBtnType btnType, bool b)
        {
            _isPointerEnter = b;
            if (!_isPointerDown)
            {
                RefreshBtnScale(btnType);
            }

            var index = (int) btnType;
            if (_cachedView.SkeletonGraphics[index].AnimationState == null)
            {
                return;
            }

            if (b)
            {
                var loop = btnType != EBtnType.Multi;
                _cachedView.SkeletonGraphics[index].AnimationState.SetAnimation(0, PointInAnimNames[index], loop);
            }
            else
            {
                if (btnType == EBtnType.Multi)
                {
                    _cachedView.SkeletonGraphics[index].AnimationState.SetAnimation(0, "OnLineClose", false);
                }
                else
                {
                    _cachedView.SkeletonGraphics[index].AnimationState.SetEmptyAnimation(0, 0.2f);
//                    track.trackTime = track.trackEnd;
                }
            }
        }

        private enum EBtnType
        {
            Single,
            WorkShop,
            World,
            Multi,
            Max
        }
    }
}