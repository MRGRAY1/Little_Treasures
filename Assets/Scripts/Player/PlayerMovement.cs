using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    #region Variables

    [SerializeField] private bool enableDebug;

    [SerializeField, Tooltip("Player Manager")]
    private PlayerManager player;

    /// <summary>
    /// Base
    /// </summary>
    [Header("Base Variables")] [SerializeField, Tooltip("Base Player Speed")]
    private float player_Speed = 5.0f;

    private Vector3 move_Direction = Vector3.zero;
    private Rigidbody Rigidbody;
    private Vector2 input;

    /// <summary>
    /// Gravity/Jumping
    /// </summary>
    [Header("Jumping")] [SerializeField, Tooltip("Jump Force")]
    private float jump_Force = 5f;

    [SerializeField, Tooltip("Layer for Ground")]
    private List<string> ground_Check_List;

    [SerializeField, Tooltip("Ground Check Gameobject on player")]
    private Transform ground_Check;

    //[SerializeField, Tooltip("Ground Distance")]
    //private float ground_Distance = 0.3f;
    [SerializeField] private bool is_grounded;

    [SerializeField] private float raycast_Dist = .5f;

    /// <summary>
    /// Sprinting
    /// </summary>
    [Header("Sprinting")] [SerializeField, Tooltip("Sprint Modifyer Speed")]
    private float sprint_Mod_Speed = 10f;

    private bool is_Sprinting = false;
    private bool wantsToSprint = false;

    /// <summary>
    /// Stamina
    /// </summary>
    [Header("Stamina")] [SerializeField, Tooltip("Max Stamina")]
    private float max_Stamina = 10f;

    [SerializeField, Tooltip("Stamina Depletion Rate")]
    private float stamina_Depletion = 2f;

    [SerializeField, Tooltip("Stamina Regen Rate")]
    private float stamina_Regen_Rate = 1f;

    [SerializeField, Tooltip("Current Stamina")]
    private float current_Stamina;

    [SerializeField, Tooltip("Stamina Reset Flag")]
    private bool reset_Stamina = false;

    /// <summary>
    /// Crouching
    /// </summary>
    [Header("Crouching")] [SerializeField, Tooltip("Player Camera Height")]
    private Transform player_Height_Pos;

    [SerializeField, Tooltip("Player Standing Height")]
    private float original_Height;

    [SerializeField, Tooltip("Player Crouching Height")]
    private float crouch_Height = 1f;

    [SerializeField, Tooltip("Movement Speed while Crouched")]
    private float crouch_Speed_Modifier = 0.5f;

    [SerializeField, Tooltip("Jump Height while crouched")]
    private float crouch_Jump_Modifyer = 0.5f;

    [SerializeField, Tooltip("Standing/crouch change speed")]
    private float crouch_Lerp_Speed = 5f;

    [SerializeField, Tooltip("Is Crouching?")]
    private bool is_Crouching = false;

    #endregion

    #region Functions

    public void Initialize()
    {
        Rigidbody = GetComponent<Rigidbody>();
        player = GetComponent<PlayerManager>();
        input = new Vector2();

        original_Height = player_Height_Pos.localPosition.y;
        current_Stamina = max_Stamina;

        player.OnMovePerformed += HandleMove;
        player.OnMoveCanceled += StopMove;
        player.OnCrouchPressed += HandleCrouch;
        player.OnCrouchReleased += StopCrouch;
        player.OnSprintPressed += HandleSprint;
        player.OnSprintReleased += StopSprint;
        player.OnJumpPressed += HandleJump;
    }

    private void HandleSprint()
    {
        // Sprint Behavior
        wantsToSprint = true;
    }

    private void StopSprint()
    {
        wantsToSprint = false;
    }

    private void HandleCrouch()
    {
        is_Crouching = true;
    }

    private void StopCrouch()
    {
        is_Crouching = false;
    }

    private void HandleMove(Vector2 playerInput)
    {
        input = playerInput;
    }

    private void StopMove()
    {
        input = Vector2.zero;
    }

    private void HandleJump()
    {
        // Jump Behavior
        if (is_grounded)
        {
            float jumpModifyer = !is_Crouching ? 1 : crouch_Jump_Modifyer;
            Rigidbody.AddForce(Vector3.up * jump_Force * jumpModifyer, ForceMode.Impulse);
        }
    }

    /// <summary>
    /// On Disable
    /// </summary>
    private void OnDisable()
    {
        if (player == null) return;
        player.OnMovePerformed -= HandleMove;
        player.OnMoveCanceled -= StopMove;
        player.OnCrouchPressed -= HandleCrouch;
        player.OnCrouchReleased -= StopCrouch;
        player.OnSprintPressed -= HandleSprint;
        player.OnSprintReleased -= StopSprint;
        player.OnJumpPressed -= HandleJump;
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        RaycastHit ground_hit;
        Debug.DrawRay(ground_Check.position, Vector3.down * raycast_Dist, Color.green);
        if (Physics.Raycast(ground_Check.position, Vector3.down, out ground_hit, 0.1f))
        {
            foreach (string tag in ground_Check_List)
            {
                if (ground_hit.collider.CompareTag(tag))
                {
                    if (enableDebug)
                    {
                        Debug.Log("Player is above a valid ground layer");
                    }

                    is_grounded = true;
                    break; // Stop checking if one layer matched
                }
            }
        }
        else
        {
            is_grounded = false;
        }

        //Check if input is moving forward
        bool isMovingForward = input.y > 0.1f;
        // Sprint blocked until stamina recovers past threshold
        if (reset_Stamina)
        {
            is_Sprinting = false;

            // Recover stamina
            if (current_Stamina < max_Stamina)
            {
                current_Stamina += stamina_Regen_Rate * Time.deltaTime;
                if (current_Stamina >= max_Stamina * 0.5f)
                {
                    reset_Stamina = false; // allow sprinting again
                }
            }
        }
        else
        {
            if (wantsToSprint && isMovingForward && current_Stamina > 0 && (is_grounded || is_Sprinting))
            {
                is_Sprinting = true;
                current_Stamina -= stamina_Depletion * Time.deltaTime;

                if (current_Stamina <= 0)
                {
                    current_Stamina = 0;
                    reset_Stamina = true;
                    is_Sprinting = false;
                }
            }
            else
            {
                is_Sprinting = false;

                // Recover stamina normally
                if (current_Stamina < max_Stamina)
                {
                    current_Stamina += stamina_Regen_Rate * Time.deltaTime;
                    if (current_Stamina > max_Stamina)
                    {
                        current_Stamina = max_Stamina;
                    }
                }
            }
        }


        float targetPos = !is_Crouching ? original_Height : crouch_Height;
        Vector3 currentPos = player_Height_Pos.localPosition;
        float newY = Mathf.MoveTowards(currentPos.y, targetPos, crouch_Lerp_Speed * Time.deltaTime);
        player_Height_Pos.localPosition = new Vector3(currentPos.x, newY, currentPos.z);
    }

    /// <summary>
    /// Fixed Update is called every fixed framerate
    /// </summary>
    private void FixedUpdate()
    {
        // Movement Behavior
        float current_Speed = player_Speed;
        if (is_Crouching)
        {
            current_Speed *= crouch_Speed_Modifier;
        }
        else if (is_Sprinting)
        {
            current_Speed *= sprint_Mod_Speed;
        }

        Vector3 move = new Vector3(input.x, 0.0f, input.y).normalized;

        Vector3 targetVelocity = transform.TransformDirection(move) * current_Speed;
        Vector3 velocity = Rigidbody.linearVelocity;
        velocity.x = targetVelocity.x;
        velocity.z = targetVelocity.z;

        Rigidbody.linearVelocity = velocity;
    }

    #endregion
}