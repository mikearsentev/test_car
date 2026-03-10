using UnityEngine;

public class ReloadCubesButton : MonoBehaviour
{
    public ResettableObject[] objectsToReset;

    public void ResetAll()
    {
        foreach (var obj in objectsToReset)
            obj.ResetObject();
    }
}
