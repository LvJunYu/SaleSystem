#region
/*************************************************************************************
   * filename:          UMViewBase
   * date:              3/16/2015 4:07:22 PM
   * author:            ake

    * 修改时间:
    * 修 改 人:
    * 
    ************************************************************************************/
#endregion

using UnityEngine;
using UnityEngine.EventSystems;


namespace UITools
{
    public class UMViewBase : UIBehaviour
    {
        #region 变量

        protected RectTransform _trans;

        #endregion

        #region 属性

        public RectTransform Trans
        {
            get
            {
                return _trans;
            }
        }

        #endregion

        #region 方法

        public void Init()
        {
            _trans = GetComponent<RectTransform>();
        }

        #endregion
    }
}