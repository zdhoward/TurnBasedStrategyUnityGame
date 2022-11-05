using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float zoomDefaultOffset = 8f;
    [SerializeField] private float zoomSpeed = 2f;
    [SerializeField] private float zoomSensitivity = 1f;
    private const float MIN_FOLLOW_Y_OFFSET = 2f;
    private const float MAX_FOLLOW_Y_OFFSET = 12f;
    private Vector3 targetFollowOffset;

    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    private CinemachineTransposer transposer;

    private void Awake()
    {
        transposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        transposer.m_FollowOffset.y = zoomDefaultOffset;
        targetFollowOffset = transposer.m_FollowOffset;
    }

    void Update()
    {
        HandlePosition();
        HandleRotation();
        HandleZoom();
    }

    public void HandlePosition()
    {
        Vector3 inputMoveDirection = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
            inputMoveDirection.z = 1f;
        if (Input.GetKey(KeyCode.A))
            inputMoveDirection.x = -1f;
        if (Input.GetKey(KeyCode.S))
            inputMoveDirection.z = -1f;
        if (Input.GetKey(KeyCode.D))
            inputMoveDirection.x = 1f;

        Vector3 moveVector = transform.forward * inputMoveDirection.z + transform.right * inputMoveDirection.x;
        transform.position += moveVector * moveSpeed * Time.deltaTime;
    }

    public void HandleRotation()
    {
        Vector3 rotationVector = Vector3.zero;
        if (Input.GetKey(KeyCode.Q))
            rotationVector.y = 1f;
        if (Input.GetKey(KeyCode.E))
            rotationVector.y = -1f;

        transform.eulerAngles += rotationVector * rotationSpeed * Time.deltaTime;
    }

    public void HandleZoom()
    {
        if (Input.mouseScrollDelta.y > 0)
            targetFollowOffset.y -= zoomSensitivity;
        if (Input.mouseScrollDelta.y < 0)
            targetFollowOffset.y += zoomSensitivity;

        targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_Y_OFFSET);

        transposer.m_FollowOffset = Vector3.Lerp(transposer.m_FollowOffset, targetFollowOffset, zoomSpeed * Time.deltaTime);
    }
}
