using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

[CanEditMultipleObjects]
[CustomEditor(typeof(CanvasRenderer), true)]
public class CanvasRendererInspector : Editor
{
    private RectTransform _rectTransform;
    private Image _image;
    private MonoBehaviour _behaviour;

    public override void OnInspectorGUI()
    {
        if (_rectTransform == null)
        {
            _rectTransform = ((CanvasRenderer) target).GetComponent<RectTransform>();
        }

        DrawDefaultInspector();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("All", GUILayout.Width(30f)))
        {
            _rectTransform.localPosition = RoundToInt(_rectTransform.localPosition);
            _rectTransform.localScale = Vector3.one;
            _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,
                Mathf.RoundToInt(_rectTransform.rect.width / 2) * 2);
            _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,
                Mathf.RoundToInt(_rectTransform.rect.height / 2) * 2);
        }

        GUILayout.EndHorizontal();
    }

    private Vector3 RoundToInt(Vector3 vector3)
    {
        return new Vector3(Mathf.RoundToInt(vector3.x), Mathf.RoundToInt(vector3.y), Mathf.RoundToInt(vector3.z));
    }
}