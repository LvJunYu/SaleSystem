using System.Collections.Generic;
using MyTools;
using UITools;
using UnityEngine;

namespace Sale
{
    public class UMCtrlCollectData : UMCtrlGenericBase<UMViewCollectData>
    {
        private List<UMCtrlCollectDataItem> _items = new List<UMCtrlCollectDataItem>();

        public void SetData(List<string> infos, List<int> nums, int maxNum)
        {
            var count = infos.Count;
            if (nums.Count != count)
            {
                LogHelper.Error("count is not equal, info length {0}, nums length {1}", infos.Count, nums.Count);
                return;
            }

            SetItemCount(count);
            var width = CalculateWidth(count);
            for (int i = 0; i < count; i++)
            {
                _items[i].SetWith(width);
                _items[i].SetData(infos[i], nums[i], maxNum);
            }
        }

        public void SetLineName(string horizontalName, string verticalName)
        {
            _cachedView.HorizontalTxt.text = horizontalName;
            _cachedView.VerticalTxt.text = verticalName;
        }

        private void SetItemCount(int count)
        {
            for (int i = 0; i < count; i++)
            {
                if (i < _items.Count)
                {
                    _items[i].SetActive(true);
                }
                else
                {
                    _items.Add(CreateItem());
                }
            }

            for (int i = count; i < _items.Count; i++)
            {
                _items[i].SetActive(false);
            }
        }

        private UMCtrlCollectDataItem CreateItem()
        {
            var item = new UMCtrlCollectDataItem();
            item.Init(_cachedView.ItemContent);
            return item;
        }

        private int CalculateWidth(int count)
        {
            var dockWidth = _cachedView.ItemContent.GetWidth();
            return Mathf.Min(80, (int) (dockWidth / (count * 2f)));
        }

        public void SetRaw(int count)
        {
            for (int i = 0; i < count; i++)
            {
                _items[i].HideInfo();
            }
        }
    }
}