using UnityEngine;
using UnityEngine.EventSystems;   // важно!

public class GarageCameraa : MonoBehaviour
{
    [SerializeField] float moveSpeed = 25f;
    [SerializeField] float edgeSize  = 30f;
    [SerializeField] float limitLeft  = -10f;
    [SerializeField] float limitRight = 40f;

    void Update()
    {
        // если курсор над любым UI-элементом И зажата ЛКМ — камеру не двигаем
        if (EventSystem.current != null &&
            EventSystem.current.IsPointerOverGameObject() &&  // мышь над UI [web:37][web:40]
            Input.GetMouseButton(0))                          // тащим что-то
        {
            return;
        }

        Vector3 pos = transform.position;

        if (Input.mousePosition.x > Screen.width - edgeSize)
            pos += Vector3.right * moveSpeed * Time.deltaTime;
        else if (Input.mousePosition.x < edgeSize)
            pos += Vector3.left * moveSpeed * Time.deltaTime;

        pos.x = Mathf.Clamp(pos.x, limitLeft, limitRight);
        transform.position = pos;
    }
}
