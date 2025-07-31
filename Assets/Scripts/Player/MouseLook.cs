using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{

    #region Variables
    [SerializeField, Tooltip("Mouse X Sensitivity")]
    private float x_mouse_Sensitivity = 1.5f;
    [SerializeField, Tooltip("Mouse Y Sensitivity")]
    private float y_mouse_Sensitivity = 1.5f;
    [SerializeField, Tooltip("Camera Object")]
    private Transform camera_Transform;
    [SerializeField, Tooltip("XY Rotation")]
    private Vector2 XY_Rotation;
    [SerializeField, Tooltip("Camera Smoothing Speed")]
    private float smooth_Speed = 10f;

    private PlayerInputs player_Inputs;
    private Vector2 mouse_Delta;
    private Vector2 smooth_Delta;
    private float pitch = 0f;

    #endregion

    private void Awake()
    {
        this.player_Inputs = new PlayerInputs();

    }
    private void OnEnable()
    {
        this.player_Inputs.Enable();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
    }
    private void OnDisable()
    {
        this.player_Inputs.Disable();
    }


    //Update is called once per frame
    private void LateUpdate()
    {
        this.mouse_Delta = this.player_Inputs.Main.Look.ReadValue<Vector2>();
        this.XY_Rotation.x -= this.mouse_Delta.y * this.y_mouse_Sensitivity;
        this.XY_Rotation.y += this.mouse_Delta.x * this.x_mouse_Sensitivity;

        this.XY_Rotation.x = Mathf.Clamp(this.XY_Rotation.x, -90f, 90f);

        // Rotate player left/right (yaw)
        this.camera_Transform.localEulerAngles = new Vector3(this.XY_Rotation.x, 0f, 0f);
        // Rotate camera up/down (pitch)
        this.transform.eulerAngles = new Vector3(0f, this.XY_Rotation.y, 0f);
    }
}
