using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset = new Vector3(0, 2, -4);
    [SerializeField] float followSpeed = 10f;

    void LateUpdate()
    {
        Vector3 PositionCube = target.position + target.TransformDirection(offset);

        transform.position = Vector3.Lerp(
            transform.position,
            PositionCube,
            followSpeed * Time.deltaTime
        );
        transform.LookAt(target.position + Vector3.up * 1f);
    }
}