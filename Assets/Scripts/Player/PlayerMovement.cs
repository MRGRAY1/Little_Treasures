using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    #region Variables

    [SerializeField]
    private bool enableDebug;

    /// <summary>
    /// Base
    /// </summary>
    [Header("Base Variables")]
    [SerializeField, Tooltip("Base Player Speed")]
    private float player_Speed = 5.0f;
    private Vector3 move_Direction = Vector3.zero;
    private Rigidbody Rigidbody;
    private InputManager inputManager;
    private Vector2 input;

    /// <summary>
    /// Player Character
    /// </summary>
    [SerializeField, Tooltip("Player ID")]
    private int player_ID;
    [SerializeField, Tooltip("Player Name")]
    private string player_Name;
    [SerializeField, Tooltip("Player Information")]
    private Player_SO player_SO;

    /// <summary>
    /// Gravity/Jumping
    /// </summary>
    [Header("Jumping")]
    [SerializeField, Tooltip("Jump Force")]
    private float jump_Force = 5f;
    [SerializeField, Tooltip("Layer for Ground")]
    private List<string> ground_Check_List;
    [SerializeField, Tooltip("Ground Check Gameobject on player")]
    private Transform ground_Check;
    [SerializeField, Tooltip("Ground Distance")]
    private float ground_Distance = 0.3f;
    [SerializeField]
    private bool is_grounded;

    [SerializeField]
    private float raycast_Dist = .5f;

    /// <summary>
    /// Sprinting
    /// </summary>
    [Header("Sprinting")]
    [SerializeField, Tooltip("Sprint Modifyer Speed")]
    private float sprint_Mod_Speed = 10f;
    private bool is_Sprinting = false;
    private bool wantsToSprint = false;

    /// <summary>
    /// Stamina
    /// </summary>
    [Header("Stamina")]
    [SerializeField, Tooltip("Max Stamina")]
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
    [Header("Crouching")]
    [SerializeField, Tooltip("Player Camera Height")]
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
    private void InitializeCharacter()
    {
        this.original_Height = this.player_Height_Pos.localPosition.y;
        this.current_Stamina = this.max_Stamina;

        if (this.player_SO != null)
        {
            this.player_ID = this.player_SO.Player_Id;
            this.player_Name = this.player_SO.Player_Name;
        }
        else
        {
            this.player_ID = 9999;
            this.player_Name = "Player Name";
        }
    }

    /// <summary>
    /// On Enable
    /// </summary>
    private void OnEnable()
    {
        this.inputManager = FindObjectOfType<InputManager>();
        this.inputManager.OnMovePerformed += HandleMove;
        this.inputManager.OnMoveCanceled += StopMove;
        this.inputManager.OnJumpPressed += HandleJump;
        this.inputManager.OnCrouchPressed += HandleCrouch;
        this.inputManager.OnCrouchReleased += StopCrouch;
        this.inputManager.OnSprintPressed += HandleSprint;
        this.inputManager.OnSprintReleased += StopSprint;
        this.Rigidbody = GetComponent<Rigidbody>();
        this.input = new Vector2();
        this.InitializeCharacter();
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
        this.is_Crouching = true;
    }

    private void StopCrouch()
    {
        this.is_Crouching = true;
    }
    private void HandleMove(Vector2 input)
    {
        this.input = input;
    }
    private void StopMove()
    {
        this.input = Vector2.zero;
    }
    private void HandleJump()
    {
        // Jump Behavior
        if (this.is_grounded)
        {
            float jumpModifyer = !this.is_Crouching ? 1 : this.crouch_Jump_Modifyer;
            this.Rigidbody.AddForce(Vector3.up * this.jump_Force * jumpModifyer, ForceMode.Impulse);
        }
    }

    /// <summary>
    /// On Disable
    /// </summary>
    private void OnDisable()
    {
        this.inputManager = FindObjectOfType<InputManager>();
        if (this.inputManager == null) return;

        this.inputManager.OnMovePerformed -= HandleMove;
        this.inputManager.OnMoveCanceled -= StopMove;
        this.inputManager.OnJumpPressed -= HandleJump;
        this.inputManager.OnCrouchPressed -= HandleCrouch;
        this.inputManager.OnCrouchReleased -= StopCrouch;
        this.inputManager.OnSprintPressed -= HandleSprint;
        this.inputManager.OnSprintReleased -= StopSprint;

    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {

        RaycastHit ground_hit;
        Debug.DrawRay(this.ground_Check.position, Vector3.down * this.raycast_Dist, Color.green);
        if (Physics.Raycast(this.ground_Check.position, Vector3.down, out ground_hit, 0.1f))
        {
            foreach (string tag in this.ground_Check_List)
            {
                if (ground_hit.collider.CompareTag(tag))
                {
                    if (this.enableDebug)
                    {
                        Debug.Log("Player is above a valid ground layer");
                    }
                    this.is_grounded = true;
                    break; // Stop checking if one layer matched
                }

            }
        }
        else
        {
            this.is_grounded = false;
        }

        //Check if input is moving forward
        bool isMovingForward = this.input.y > 0.1f;
        // Sprint blocked until stamina recovers past threshold
        if (this.reset_Stamina)
        {
            this.is_Sprinting = false;

            // Recover stamina
            if (this.current_Stamina < this.max_Stamina)
            {
                this.current_Stamina += this.stamina_Regen_Rate * Time.deltaTime;
                if (this.current_Stamina >= this.max_Stamina * 0.5f)
                {
                    this.reset_Stamina = false; // allow sprinting again
                }
            }
        }
        else
        {
            if (wantsToSprint && isMovingForward && this.current_Stamina > 0 && (this.is_grounded || this.is_Sprinting))
            {
                this.is_Sprinting = true;
                this.current_Stamina -= this.stamina_Depletion * Time.deltaTime;

                if (this.current_Stamina <= 0)
                {
                    this.current_Stamina = 0;
                    this.reset_Stamina = true;
                    this.is_Sprinting = false;
                }
            }
            else
            {
                this.is_Sprinting = false;

                // Recover stamina normally
                if (this.current_Stamina < this.max_Stamina)
                {
                    this.current_Stamina += this.stamina_Regen_Rate * Time.deltaTime;
                    if (this.current_Stamina > this.max_Stamina)
                    {
                        this.current_Stamina = this.max_Stamina;
                    }
                }
            }
        }



        float targetPos = !this.is_Crouching ? this.original_Height : this.crouch_Height;
        Vector3 currentPos = this.player_Height_Pos.localPosition;
        float newY = Mathf.MoveTowards(currentPos.y, targetPos, this.crouch_Lerp_Speed * Time.deltaTime);
        this.player_Height_Pos.localPosition = new Vector3(currentPos.x, newY, currentPos.z);

    }

    /// <summary>
    /// Fixed Update is called every fixed framerate
    /// </summary>
    private void FixedUpdate()
    {
        // Movement Behavior
        float current_Speed = this.player_Speed;
        if (this.is_Crouching)
        {
            current_Speed *= this.crouch_Speed_Modifier;
        }
        else if (this.is_Sprinting)
        {
            current_Speed *= this.sprint_Mod_Speed;
        }

        Vector3 move = new Vector3(this.input.x, 0.0f, this.input.y).normalized;

        Vector3 targetVelocity = this.transform.TransformDirection(move) * current_Speed;
        Vector3 velocity = this.Rigidbody.velocity;
        velocity.x = targetVelocity.x;
        velocity.z = targetVelocity.z;

        this.Rigidbody.velocity = velocity;
    }
    #endregion

}
