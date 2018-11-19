using UnityEngine;

public static class TransformHelper
{
    public static void SetPositionAndRotation(this Transform transform, Vector3 pos, Quaternion quaternion)
    {
        transform.position = pos;
        transform.rotation = quaternion;
    }
}