using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private PlayerInputs player_Inputs;


    /// <summary>
    /// Player Movement Events
    /// </summary>
    public event Action<Vector2> OnMovePerformed;
    public event Action OnMoveCanceled;
    public event Action OnJumpPressed;
    public event Action OnJumpReleased;
    public event Action OnCrouchPressed;
    public event Action OnCrouchReleased;
    public event Action OnSprintPressed;
    public event Action OnSprintReleased;

    public event Action onMouseClicked;
    public event Action onMouseReleased;

    public event Action OnNreleased;


    private void Awake()
    {
        this.player_Inputs = new PlayerInputs();

        /// <summary>
        /// Player Movement Events
        /// </summary>

        // Move
        this.player_Inputs.Main.Movement.performed += ctx => OnMovePerformed?.Invoke(ctx.ReadValue<Vector2>());
        this.player_Inputs.Main.Movement.canceled += ctx => OnMoveCanceled?.Invoke();

        // Jump
        this.player_Inputs.Main.Jump.performed += ctx => OnJumpPressed?.Invoke();
        this.player_Inputs.Main.Jump.canceled += ctx => OnJumpReleased?.Invoke();

        // Crouch
        this.player_Inputs.Main.Crouch.performed += ctx => OnCrouchPressed?.Invoke();
        this.player_Inputs.Main.Crouch.performed += ctx => OnCrouchReleased?.Invoke();

        // Sprint
        this.player_Inputs.Main.Sprint.performed += ctx => OnSprintPressed?.Invoke();
        this.player_Inputs.Main.Sprint.performed += ctx => OnSprintReleased?.Invoke();

        // Mouse Click
        this.player_Inputs.Main.MouseClick.performed += ctx => onMouseClicked?.Invoke();
        this.player_Inputs.Main.MouseClick.canceled += ctx => onMouseReleased?.Invoke();

        this.player_Inputs.Main.N.performed += ctx => OnNreleased?.Invoke();
    }

    private void OnEnable()
    {
        this.player_Inputs.Enable();
    }
    private void OnDisable()
    {
        this.player_Inputs.Disable();
    }
}