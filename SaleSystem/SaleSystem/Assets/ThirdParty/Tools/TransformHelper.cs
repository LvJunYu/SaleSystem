using UnityEngine;

namespace MyTools
{
	public static class TransformHelper 
    {
        public static void DestroyChildren(this Transform trans)
        {
            foreach (Transform child in trans)
            {
                Object.Destroy(child.gameObject);
            }
        }
	}
}
