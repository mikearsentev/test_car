using UnityEngine.EventSystems;

public static class UIState
{
    public static bool IsUsingUI = false;

    public static bool IsPointerOverUI()
    {
        return EventSystem.current != null &&
               EventSystem.current.IsPointerOverGameObject();
    }
}
