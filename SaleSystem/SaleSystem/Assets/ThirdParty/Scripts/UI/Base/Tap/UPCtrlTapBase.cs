namespace UITools
{
    public class UPCtrlTapBase<C, V> : UPCtrlBase<C, V> where C : UICtrlBase where V : UIViewTapBase
    {
        public int Menu;

        public override void Open()
        {
            base.Open();
            _cachedView.Pannels[Menu].SetActiveEx(true);
        }

        public override void Close()
        {
            _cachedView.Pannels[Menu].SetActiveEx(false);
            base.Close();
        }
    }
}