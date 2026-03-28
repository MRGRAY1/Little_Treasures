using System;
using Player;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Components")] [SerializeField]
    private PlayerMovement movement;

    [SerializeField] private PlayerInput input;

    [SerializeField] private MouseLook mouseLook;
    //[SerializeField] private PlayerHealth health;
    //[SerializeField] private PlayerInventory inventory;


    /// <summary>
    /// Player Character
    /// </summary>
    [Header("Character Attributes")] [SerializeField, Tooltip("Player ID")]
    private int player_ID;

    [SerializeField, Tooltip("Player Name")]
    private string player_Name;

    [SerializeField, Tooltip("Player Information")]
    private Player_SO player_SO;

    public int PlayerId { get; private set; }

    private void Awake()
    {
        // Components find their hub instead of the whole scene
        Initialize();
        movement.Initialize();
        mouseLook.Initialize();
        input.Initialize();


        //health.Initialize(this);
        //inventory.Initialize(this);
    }

    private void Initialize()
    {
        if (player_SO != null)
        {
            player_ID = player_SO.Player_Id;
            player_Name = player_SO.Player_Name;
        }
        else
        {
            player_ID = 9999;
            player_Name = "Player Name";
        }
    }

    // Global game events still flow through GameEvents
    // but player-specific stuff stays here
    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }


    // Player Movement Events
    public Action<Vector2> OnMovePerformed;
    public Action OnMoveCanceled;
    public Action OnJumpPressed;
    public Action OnCrouchPressed;
    public Action OnCrouchReleased;
    public Action OnSprintPressed;
    public Action OnSprintReleased;
    public Action<Vector2> OnLookPerformed;
    public Action OnLookCanceled;

    public Action onMouseClicked;
    public Action onMouseReleased;
}