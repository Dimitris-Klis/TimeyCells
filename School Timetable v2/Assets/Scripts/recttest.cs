using UnityEngine;

public class recttest : MonoBehaviour
{
    public RectTransform child;
    [ContextMenu("test")]
    public void TestPos()
    {
        child.localPosition = Vector3.zero;
    }
}