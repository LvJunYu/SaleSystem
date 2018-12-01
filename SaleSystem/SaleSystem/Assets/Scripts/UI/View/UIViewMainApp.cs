using Spine.Unity;
using UITools;
using UnityEngine;
using UnityEngine.UI;

namespace Sale
{
    public class UIViewMainApp : UIViewBase
    {
        public EventTriggerListener[] EventTriggerListeners;
        public RectTransform[] BtnRtfs;
        public SkeletonGraphic[] SkeletonGraphics;
        public RectTransform Wind1Rtf;
        public RectTransform Wind2Rtf;
        public RectTransform Wind3Rtf;
        public SkeletonGraphic LeftChiLun;
        public SkeletonGraphic RightChiLun;
        public RectTransform RightDownChiLunRtf;
        public RectTransform RabbitEye;
        public RectTransform[] Winds;
        public Button RecordBtn;
        public Button QueryBtn;
        public Button SettingBtn;
    }
}
