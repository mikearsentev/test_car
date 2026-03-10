using UnityEngine;
using UnityEngine.EventSystems;

public class UIResizeHandle : MonoBehaviour, IDragHandler
{
    public RectTransform target;                 // сюда перетащи CardList
    public Vector2 minSize = new Vector2(100, 100);
    public Vector2 maxSize = new Vector2(200, 300);

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 size = target.sizeDelta + new Vector2(eventData.delta.x, eventData.delta.y);

        size.x = Mathf.Clamp(size.x, minSize.x, maxSize.x);
        size.y = Mathf.Clamp(size.y, minSize.y, maxSize.y);

        target.sizeDelta = size;
    }
}
