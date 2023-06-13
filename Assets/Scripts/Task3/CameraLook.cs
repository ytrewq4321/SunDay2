using UnityEngine;
using Cinemachine;

public class CameraLook : MonoBehaviour
{
    [SerializeField] private InputManager input;
    [SerializeField] private float lookSpeedX;
    [SerializeField] private Transform target;
    private CinemachineFreeLook cinemachine;

    private void Start()
    {
        cinemachine = GetComponent<CinemachineFreeLook>();
    }

    private void Update()
    {
        Vector2 delta = input.Look;
        cinemachine.m_XAxis.Value += delta.x * lookSpeedX * Time.deltaTime;
    }
}
