using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;

    public float runSpeed = 40f;
    private Vector2 respawnPoint;

    RespawnPointHolder respawnScript;

    bool jump = false;
    bool dash = false;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    private Rigidbody2D rb;
    private PlayerInput playerInput;
    private PlayerInputActions playerInputActions;


    private void Awake()
    {
        SoundManager.Initialize();  // Maybe move this to a more apropriate place later

        respawnPoint = transform.position;

        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Jump.performed += Jump;
        playerInputActions.Player.Dash.performed += Dash;
    }

    private void FixedUpdate()
    {
        Vector2 inputVector = playerInputActions.Player.Movement.ReadValue<Vector2>();

        controller.Move(inputVector.x * Time.fixedDeltaTime * runSpeed, inputVector.y, jump, dash, inputVector.x);
        dash = false;
        jump = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Better jumping related stuff
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !jump) // originally was !Input.GetButton("Jump")
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)  // true if the button was just hit
        {
            jump = true;
        }
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed)  // true if the button was just hit
        {
            dash = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "DeathZone")
        {
            transform.position = respawnPoint;
        }
        else if (collision.tag == "CheckPoint")
        {
            respawnScript = collision.gameObject.GetComponent<RespawnPointHolder>();
            respawnPoint = respawnScript.getPoint();
        }
    }
}