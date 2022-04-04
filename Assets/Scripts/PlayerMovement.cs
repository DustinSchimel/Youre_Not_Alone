using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Yarn.Unity;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public Animator animator;
    private TrailRenderer trail;

    public float runSpeed = 40f;
    private Vector2 respawnPoint;

    PauseMenu pauseScript;

    RespawnPointHolder respawnScript;
    public Text checkpointText;
    string lastCheckpointReached;

    bool jump = false;
    bool dash = false;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    CollectibleTally collectibleScript;
    bool inCollectible = false;
    GameObject collectible = null;

    private Rigidbody2D rb;
    private PlayerInputActions playerInputActions;

    private DialogueRunner dialogueRunner = null;
    private DialogueControls dialogueControls = null;
    private float interactionRadius = 5f;

    public GameObject scriptHolder;

    public AudioManager audioPlayer;



    private void Awake()
    {
        dialogueRunner = FindObjectOfType<DialogueRunner>();
        //dialogueControls = FindObjectOfType<DialogueControls>();
        dialogueControls = scriptHolder.GetComponent<DialogueControls>();

        //pauseScript = this.gameObject.GetComponent<PauseScript>;//collision.gameObject.GetComponent<RespawnPointHolder>();
        pauseScript = this.gameObject.GetComponent<PauseMenu>();

        lastCheckpointReached = "";
        checkpointText.enabled = false;

        audioPlayer = AudioManager.instance;        

        respawnPoint = transform.position;

        rb = GetComponent<Rigidbody2D>();
        trail = GetComponent<TrailRenderer>();

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Jump.performed += Jump;
        playerInputActions.Player.Dash.performed += Dash;
        playerInputActions.Player.Interact.performed += Interact;
        playerInputActions.Player.Pause.performed += Pause;

        playerInputActions.Dialogue.MoveLeft.performed += MoveLeft;
        playerInputActions.Dialogue.MoveRight.performed += MoveRight;
        playerInputActions.Dialogue.Skip.performed += Skip;
        playerInputActions.Dialogue.SelectOption.performed += SelectOption;

        
    }

    private void FixedUpdate()
    {
        Vector2 inputVector = playerInputActions.Player.Movement.ReadValue<Vector2>();

        if (!dialogueRunner.IsDialogueRunning)  // Player is not currently talking, so they can move
        {
            playerInputActions.Player.Enable();
            controller.Move(inputVector.x * Time.fixedDeltaTime * runSpeed, inputVector.y, jump, dash, inputVector.x);
            dash = false;
            jump = false;
        }
        else
        {
            controller.Move(inputVector.x * Time.fixedDeltaTime * runSpeed, inputVector.y, jump, dash, 0);  // Stops the moving animation for the player
            rb.velocity = new Vector2(0f, rb.velocity.y);   // Stops the players horizontal velocity once they enter dialogue

            playerInputActions.Player.Disable();    // Disables player movement
            playerInputActions.Dialogue.Enable();   // Enables dialogue controls
        }
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
            //animator.SetBool("IsJumping", true);
            //Debug.Log("Jumped");
        }
    }

    public void UponLanding () //jumping animation call
    {
        //animator.SetBool("IsJumping", false);
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed)  // true if the button was just hit
        {
            dash = true;
        }
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (inCollectible)
        {
            collectible.SetActive(false);

            if (collectible.name == "Merchant_Cup")
            {
                collectibleScript = rb.gameObject.GetComponent<CollectibleTally>();
                collectibleScript.setMerchantCup(true);
            }
        }
        else
        {
            CheckForNearbyNPC();
        }
    }

    /// Find all DialogueParticipants
    /** Filter them to those that have a Yarn start node and are in range; 
     * then start a conversation with the first one
     */
    public void CheckForNearbyNPC()
    {
        var allParticipants = new List<NPC>(FindObjectsOfType<NPC>()); // Retrieves all NPC in the scene
        var player = FindObjectOfType<PlayerMovement>(); // Retrieves the player in the scene
        var target = allParticipants.Find(delegate (NPC p) { // Returns the NPC
            return string.IsNullOrEmpty(p.talkToNode) == false && // has a conversation node?
            (p.transform.position - player.transform.position).magnitude <= interactionRadius; // is in range?
        });
        if (target != null)
        {
            // Kick off the dialogue at this node.
            dialogueRunner.StartDialogue(target.talkToNode);
        }
    }

    public void Pause(InputAction.CallbackContext context)
    {
        if (context.performed)  // true if the button was just hit
        {
            pauseScript.Pause(context);
        }
    }

    public void MoveLeft(InputAction.CallbackContext context)
    {
        if (context.performed)  // true if the button was just hit
        {
            dialogueControls.ChangeOption("Left");
        }
    }

    public void MoveRight(InputAction.CallbackContext context)
    {
        if (context.performed)  // true if the button was just hit
        {
            dialogueControls.ChangeOption("Right");
        }
    }

    public void Skip(InputAction.CallbackContext context)
    {
        if (context.performed)  // true if the button was just hit
        {
            dialogueControls.SkipDialogue();
        }
    }

    public void SelectOption(InputAction.CallbackContext context)
    {
        if (context.performed)  // true if the button was just hit
        {
            dialogueControls.SelectOption();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "DeathZone")
        {
            trail.emitting = false;
            transform.position = respawnPoint;
        }
        else if (collision.tag == "Enemy" || collision.tag == "Projectile")
        {
            trail.emitting = false;
            transform.position = respawnPoint;
        }
        else if (collision.tag == "CheckPoint")
        {
            respawnScript = collision.gameObject.GetComponent<RespawnPointHolder>();
            respawnPoint = respawnScript.getPoint();
            
            if (!lastCheckpointReached.Equals(collision.gameObject.name))   // If the last checkpoint reached is not the same as the last one reached
            {
                checkpointText.enabled = true;
                lastCheckpointReached = collision.gameObject.name;  // Set it as the last checkpoint reached
            }

            StartCoroutine(Coroutine());
        }
        else if (collision.tag == "Collectible")
        {
            inCollectible = true;
            collectible = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Collectible")
        {
            inCollectible = false;
        }
    }

    IEnumerator Coroutine()
    {
        yield return new WaitForSeconds(4f);

        checkpointText.enabled = false;
    }
}