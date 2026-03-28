using UnityEngine;

namespace Player
{
    public class PlayerInput : MonoBehaviour
    {
        private PlayerInputs playerInputs;

        [SerializeField] private PlayerManager player;

        // Raw mouse delta from input
        private Vector2 mouseDelta;

        public void Initialize()
        {
            playerInputs = new PlayerInputs();
            player = GetComponent<PlayerManager>();

            playerInputs.Enable();

            playerInputs.Main.Movement.performed += ctx => player.OnMovePerformed?.Invoke(ctx.ReadValue<Vector2>());
            playerInputs.Main.Movement.canceled += ctx => player.OnMoveCanceled?.Invoke();

            playerInputs.Main.Jump.performed += ctx => player.OnJumpPressed?.Invoke();
            playerInputs.Main.Crouch.performed += ctx => player.OnCrouchPressed?.Invoke();
            playerInputs.Main.Crouch.canceled += ctx => player.OnCrouchReleased?.Invoke();
            playerInputs.Main.Sprint.performed += ctx => player.OnSprintPressed?.Invoke();
            playerInputs.Main.Sprint.canceled += ctx => player.OnSprintReleased?.Invoke();
            playerInputs.Main.MouseClick.performed += ctx => player.onMouseClicked?.Invoke();
            playerInputs.Main.MouseClick.canceled += ctx => player.onMouseReleased?.Invoke();
            playerInputs.Main.N.performed += ctx => GameEvents.OnNreleased?.Invoke();

            playerInputs.Main.Look.performed += ctx => player.OnLookPerformed?.Invoke(ctx.ReadValue<Vector2>());
            playerInputs.Main.Look.canceled += _ => player.OnLookCanceled?.Invoke();
        }

        private void OnDestroy()
        {
            playerInputs?.Disable();
            playerInputs?.Dispose();
        }
    }
}