using UnityEngine;

public class ResettableObject : MonoBehaviour
{
    Vector3 _startPos;
    Quaternion _startRot;

    void Start()
    {
        _startPos = transform.position;
        _startRot = transform.rotation;
    }

    public void ResetObject()
    {
        transform.position = _startPos;
        transform.rotation = _startRot;

        var rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
