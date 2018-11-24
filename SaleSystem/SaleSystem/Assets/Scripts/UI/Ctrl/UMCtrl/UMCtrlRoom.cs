using UITools;

namespace Sale
{
    public class UMCtrlRoom : UMCtrlGenericBase<UMViewRoom>
    {
        private USCtrlInfo[] _infos;

        protected override void OnViewCreated()
        {
            base.OnViewCreated();
            var infoViews = _cachedView.GetComponentsInChildren<USViewInfo>(true);
            _infos = new USCtrlInfo[infoViews.Length];
            for (int i = 0; i < infoViews.Length; i++)
            {
                _infos[i] = new USCtrlInfo();
                _infos[i].Init(infoViews[i]);
            }
        }
    }
}