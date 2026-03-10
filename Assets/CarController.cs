using UnityEngine;

public class CarController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float turnSpeed = 100f;

    void Update()
    {
        float moveInput = 0f;
        if (Input.GetKey(KeyCode.W)) moveInput = 1f;
        if (Input.GetKey(KeyCode.S)) moveInput = -1f;

        float turnInput = 0f;
        if (Input.GetKey(KeyCode.D)) turnInput = 1f;
        if (Input.GetKey(KeyCode.A)) turnInput = -1f;

        transform.Translate(Vector3.forward * moveInput * moveSpeed * Time.deltaTime);
        transform.Rotate(Vector3.up * turnInput * turnSpeed * Time.deltaTime);
    }
}
