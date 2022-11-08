#define USE_NEW_INPUT_SYSTEM
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one instance of ImputManager in this scene! " + transform.position + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
    }

    public Vector2 GetMouseScreenPosition()
    {
#if USE_NEW_INPUT_SYSTEM
        return Mouse.current.position.ReadValue();
#else
        return Input.mousePosition;
#endif
    }

    public bool IsLeftMouseButtonDownThisFrame()
    {
#if USE_NEW_INPUT_SYSTEM
        return playerInputActions.Player.LeftClick.WasPressedThisFrame();
#else
        return Input.GetMouseButton(0);
#endif
    }

    public bool IsRightMouseButtonDownThisFrame()
    {
#if USE_NEW_INPUT_SYSTEM
        return playerInputActions.Player.RightClick.WasPressedThisFrame();
#else
        return Input.GetMouseButton(1);
#endif
    }

    public bool IsTestButtonDownThisFrame()
    {
#if USE_NEW_INPUT_SYSTEM
        return playerInputActions.Player.TestButton.WasPressedThisFrame();
#else
        return Input.GetKeyDown(KeyCode.T);
#endif
    }

    public Vector2 GetCameraMoveVector()
    {
#if USE_NEW_INPUT_SYSTEM
        return playerInputActions.Player.CameraMovement.ReadValue<Vector2>();
#else
        Vector2 inputMoveDirection = Vector2.zero;
        if (Input.GetKey(KeyCode.W))
            inputMoveDirection.y = 1f;
        if (Input.GetKey(KeyCode.A))
            inputMoveDirection.x = -1f;
        if (Input.GetKey(KeyCode.S))
            inputMoveDirection.y = -1f;
        if (Input.GetKey(KeyCode.D))
            inputMoveDirection.x = 1f;

        return inputMoveDirection;
#endif
    }

    public float GetCameraRotateAmount()
    {
#if USE_NEW_INPUT_SYSTEM
        return playerInputActions.Player.CameraRotate.ReadValue<float>();
#else
        float rotateAmount = 0f;

        if (Input.GetKey(KeyCode.Q))
            rotateAmount = 1f;
        if (Input.GetKey(KeyCode.E))
            rotateAmount = -1f;

        return rotateAmount;
#endif
    }

    public float GetCameraZoomAmount()
    {
#if USE_NEW_INPUT_SYSTEM
        return playerInputActions.Player.CameraZoom.ReadValue<float>();
#else
        float cameraZoomAmount = 0f;

        if (Input.mouseScrollDelta.y > 0)
            cameraZoomAmount -= zoomSensitivity;
        if (Input.mouseScrollDelta.y < 0)
            cameraZoomAmount += zoomSensitivity;

        return cameraZoomAmount;
#endif
    }
}
