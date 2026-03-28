using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [Header("Sensitivity")] [SerializeField, Tooltip("Mouse X sensitivity (yaw)")]
    private float xSensitivity = 1.5f;

    [SerializeField, Tooltip("Mouse Y sensitivity (pitch)")]
    private float ySensitivity = 1.5f;

    [Header("References")] [SerializeField, Tooltip("Transform of the camera for pitch rotation")]
    private Transform cameraTransform;

    [SerializeField] private PlayerManager player;

    [Header("Smoothing")] [SerializeField] private bool enableSmoothing = true;
    [SerializeField] private float smoothSpeed = 10f;

    private Vector2 mouseDelta;
    private Vector2 targetRotation;
    private Vector2 currentRotation;

    public void Initialize()
    {
        player = GetComponent<PlayerManager>();
        player.OnLookPerformed += HandleLook;
        player.OnLookCanceled += StopLook;
    }

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        if (player == null) return;
        player.OnLookPerformed -= HandleLook;
        player.OnLookCanceled -= StopLook;
    }

    private void HandleLook(Vector2 delta)
    {
        mouseDelta = delta;
    }

    private void StopLook()
    {
        mouseDelta = Vector2.zero;
    }

    private void LateUpdate()
    {
        targetRotation.x -= mouseDelta.y * ySensitivity;
        targetRotation.y += mouseDelta.x * xSensitivity;
        targetRotation.x = Mathf.Clamp(targetRotation.x, -90f, 90f);

        currentRotation = enableSmoothing
            ? Vector2.Lerp(currentRotation, targetRotation, smoothSpeed * Time.deltaTime)
            : targetRotation;

        cameraTransform.localRotation = Quaternion.Euler(currentRotation.x, 0f, 0f);
        transform.rotation = Quaternion.Euler(0f, currentRotation.y, 0f);
    }
}