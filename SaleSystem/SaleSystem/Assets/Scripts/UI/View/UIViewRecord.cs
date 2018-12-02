using UITools;
using UnityEngine;
using UnityEngine.UI;

namespace Sale
{
    public class UIViewRecord : UIViewBase
    {
        public RectTransform PannelRtf;
        public RectTransform BGRtf;
        public Button CloseBtn;
        public Button CreateRoomRecordBtn;
        public Button UpdateRoomRecordBtn;
        public Button ChangeRoomBtn;
        public UITabGroup TabGroup;
        public Button[] MenuButtonAry;
        public Button[] MenuSelectedButtonAry;
        public GameObject[] Pannels;
        public GridDataScroller[] GridDataScrollers;
    }
}