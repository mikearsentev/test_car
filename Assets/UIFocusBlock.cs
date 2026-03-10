using UnityEngine;
using UnityEngine.EventSystems;

public class UIFocusBlock : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        UIState.IsUsingUI = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIState.IsUsingUI = false;
    }
}
