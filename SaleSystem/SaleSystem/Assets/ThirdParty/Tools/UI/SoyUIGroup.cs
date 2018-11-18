using MyTools;
using UnityEngine;

namespace UITools
{
	public interface IUISortingOrderExtendItem
	{
		int GetIndex();
		void SetIndex(int v);
		void SetSortingOrder(int value);
		void Clear();
	}

	public class SoyUIGroup
	{
		public const int UISortingOrderExtendItemMaxCount = 40;
		public RectTransform Trans;
		public Canvas RenderCanvas;

		public int GroupIndex;

		public int StartSortingOrder;

		private IUISortingOrderExtendItem[] _itemArray;
		private int _curItemCount;

		public void Init()
		{
			if (RenderCanvas != null)
			{
				RenderCanvas.overrideSorting = true;
				RenderCanvas.sortingOrder = StartSortingOrder;
			}
			_itemArray = new IUISortingOrderExtendItem[UISortingOrderExtendItemMaxCount];
		}

		public bool AddSortingLayerItem(IUISortingOrderExtendItem item)
		{
			if (item == null)
			{
				return false;
			}
			if (_curItemCount >= UISortingOrderExtendItemMaxCount)
			{
				LogHelper.Error("SoyUIGroup {0} item is equal to max {1},can't add more.",GroupIndex, _curItemCount);
				return false;
			}
			
			_itemArray[_curItemCount] = item;
			item.SetIndex(_curItemCount);
			item.SetSortingOrder(StartSortingOrder + item.GetIndex() +1);

			_curItemCount ++;
			return true;
		}

		public void RemoveSortingLayerItem(IUISortingOrderExtendItem item)
		{
			if (item == null)
			{
				return;
			}
			int index = item.GetIndex();
			if (index < 0 || index >= _curItemCount)
			{
				LogHelper.Error("item {0} index is invalid {1},group item count is {2}", item.ToString(),index,_curItemCount);
				return;
			}
			for (int i = index; i < _curItemCount -1 ; i++)
			{
				_itemArray[i] = _itemArray[i + 1];
				_itemArray[i].SetIndex(i);
				_itemArray[i].SetSortingOrder(StartSortingOrder + i + 1);
			}
			_itemArray[_curItemCount - 1] = null;
			item.Clear();
			_curItemCount --;
		}
	}

}