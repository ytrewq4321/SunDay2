using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float rotationSmoothing;
    [SerializeField] private Transform target; 

    private void LateUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime*speed);
        transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, rotationSmoothing);
        transform.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y, 0));
    }
}
