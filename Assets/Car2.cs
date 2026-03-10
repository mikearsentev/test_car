using UnityEngine;

public class Car2 : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float turnSpeed = 100f;

    [Header("Wall check")]
    public LayerMask wallMask;      // слой стен
    public float checkRadius = 0.6f;
    public float checkDistance = 0.5f;
    public float checkHeight = 0.5f;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float moveInput = 0f;
        if (Input.GetKey(KeyCode.UpArrow))    moveInput = 1f;
        if (Input.GetKey(KeyCode.DownArrow))  moveInput = -1f;

        float turnInput = 0f;
        if (Input.GetKey(KeyCode.RightArrow)) turnInput = 1f;
        if (Input.GetKey(KeyCode.LeftArrow))  turnInput = -1f;

        // Поворот
        Quaternion turnRotation =
            Quaternion.Euler(0f, turnInput * turnSpeed * Time.fixedDeltaTime, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);

        bool blocked = false;

        if (moveInput != 0f)
        {
            Vector3 origin = rb.position + Vector3.up * checkHeight;
            Vector3 dir    = rb.transform.forward * Mathf.Sign(moveInput);
            float dist     = checkDistance;

            if (Physics.SphereCast(origin, checkRadius, dir,
                                   out RaycastHit hit, dist, wallMask))
            {
                blocked = true;
                // Debug.DrawRay(origin, dir * dist, Color.red); // можно включить для отладки
                // Debug.Log("Стена: " + hit.collider.name);
            }
        }

        // Двигаемся только если перед нами нет стены
        if (!blocked)
        {
            Vector3 move = rb.transform.forward *
                           moveInput * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + move);
        }
    }
}
