using UnityEngine;

/// <summary>
/// Handles mouse-based camera look with optional smoothing.
/// Pitch (up/down) affects the camera, yaw (left/right) affects the player body.
/// </summary>
public class MouseLook : MonoBehaviour
{
    [Header("Sensitivity")]
    [SerializeField, Tooltip("Mouse X sensitivity (yaw)")] private float xSensitivity = 1.5f;
    [SerializeField, Tooltip("Mouse Y sensitivity (pitch)")] private float ySensitivity = 1.5f;

    [Header("References")]
    [SerializeField, Tooltip("Transform of the camera for pitch rotation")] private Transform cameraTransform;

    [Header("Smoothing")]
    [SerializeField, Tooltip("Enable smooth camera movement")] private bool enableSmoothing = true;
    [SerializeField, Tooltip("Speed at which smoothing interpolates towards target rotation. Higher = snappier")] private float smoothSpeed = 10f;

    // Input system
    private PlayerInputs playerInputs;

    // Raw mouse delta from input
    private Vector2 mouseDelta;

    // Target rotation values based on input
    private Vector2 targetRotation;

    // Smoothed rotation applied to camera/player
    private Vector2 currentRotation;

    /// <summary>
    /// Initialize input callbacks
    /// </summary>
    private void Awake()
    {
        playerInputs = new PlayerInputs();

        // Update mouse delta when Look input is performed
        playerInputs.Main.Look.performed += ctx => mouseDelta = ctx.ReadValue<Vector2>();

        // Reset mouse delta to zero when input stops
        playerInputs.Main.Look.canceled += _ => mouseDelta = Vector2.zero;
    }

    /// <summary>
    /// Enable input and lock/hide the cursor
    /// </summary>
    private void OnEnable()
    {
        playerInputs.Enable();
        Cursor.lockState = CursorLockMode.Locked; // Lock cursor to center
        Cursor.visible = false;                    // Hide cursor for FPS feel
    }

    /// <summary>
    /// Disable input when object is disabled
    /// </summary>
    private void OnDisable()
    {
        playerInputs.Disable();
    }

    /// <summary>
    /// Called every frame after Update, applies mouse look
    /// </summary>
    private void LateUpdate()
    {
        // ---------------------------
        // 1. Update target rotation based on mouse input
        // ---------------------------
        targetRotation.x -= mouseDelta.y * ySensitivity; // Pitch (up/down)
        targetRotation.y += mouseDelta.x * xSensitivity; // Yaw (left/right)

        // Clamp vertical rotation to prevent flipping
        targetRotation.x = Mathf.Clamp(targetRotation.x, -90f, 90f);

        // ---------------------------
        // 2. Smooth rotation if enabled
        // ---------------------------
        if (enableSmoothing)
        {
            // Interpolate current rotation towards target
            currentRotation = Vector2.Lerp(currentRotation, targetRotation, smoothSpeed * Time.deltaTime);
        }
        else
        {
            // No smoothing, apply target directly
            currentRotation = targetRotation;
        }

        // ---------------------------
        // 3. Apply rotations
        // ---------------------------
        // Pitch (X axis) only affects camera
        cameraTransform.localRotation = Quaternion.Euler(currentRotation.x, 0f, 0f);

        // Yaw (Y axis) only affects player body
        transform.rotation = Quaternion.Euler(0f, currentRotation.y, 0f);
    }
}