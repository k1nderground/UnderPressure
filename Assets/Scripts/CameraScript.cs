using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset = new Vector3(0, 2, -4);

    Vector3 velocity;

    void FixedUpdate()
    {
        Vector3 targetPos =
            target.position +
            target.forward * offset.z +
            Vector3.up * offset.y;

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPos,
            ref velocity,
            0.1f
        );

        transform.LookAt(target.position + Vector3.up * 1f);
    }
}