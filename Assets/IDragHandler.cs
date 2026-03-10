using UnityEngine;
using UnityEngine.EventSystems;

public class UIDrag : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    private RectTransform rt;
    private Vector2 offset;

    void Awake() => rt = GetComponent<RectTransform>();

    public void OnBeginDrag(PointerEventData e)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rt.parent as RectTransform, e.position, e.pressEventCamera, out var localPoint);
        offset = (Vector2)rt.localPosition - localPoint;
    }

    public void OnDrag(PointerEventData e)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rt.parent as RectTransform, e.position, e.pressEventCamera, out var localPoint);
        rt.localPosition = localPoint + offset;
    }
}
