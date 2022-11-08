using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

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
        Vector2 inputMoveDirection = InputManager.Instance.GetCameraMoveVector();
        Vector3 moveVector = transform.forward * inputMoveDirection.y + transform.right * inputMoveDirection.x;
        transform.position += moveVector * moveSpeed * Time.deltaTime;
    }

    public void HandleRotation()
    {
        Vector3 rotationVector = Vector3.zero;
        rotationVector.y = InputManager.Instance.GetCameraRotateAmount();
        transform.eulerAngles += rotationVector * rotationSpeed * Time.deltaTime;
    }

    public void HandleZoom()
    {
        targetFollowOffset.y += InputManager.Instance.GetCameraZoomAmount() * zoomSensitivity;
        targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_Y_OFFSET);
        transposer.m_FollowOffset = Vector3.Lerp(transposer.m_FollowOffset, targetFollowOffset, zoomSpeed * Time.deltaTime);
    }
}
