using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.UI;
using Cinemachine;

public class DialogueEventsManager : MonoBehaviour
{
    public DialogueRunner dr;
    private DialogueUI2 dialogueUI;
    public DialogueControls dialogueControls;
    public GameObject Timeline_Merch;

    public GameObject merch;
    public GameObject merch0;
    public GameObject merch2;
    public GameObject merch3;

    public GameObject friend1;
    public GameObject friend2;

    public GameObject sky;

    private bool didPlayerLie = false;

    public PlayerMovement movement;
    private CharacterController2D movement2D;

    private Camera cam;

    public GameObject teleporter0;
    public GameObject friendWall;
    public GameObject merchantWall;

    public Image merchantCupImage;
    public bool hasMerchantCup = false;

    public LevelLoader loader;

    public Animator animator;

    private void Awake()
    {
        movement2D = FindObjectOfType<CharacterController2D>();
        dialogueUI = FindObjectOfType<DialogueUI2>();
        cam = Camera.main;
        merchantCupImage.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Merchant stuff
        dr.AddCommandHandler("enable_teleporter0", enableTeleporter0);

        dr.AddCommandHandler("startFirstCutscene", startFirstCutscene);
        dr.AddCommandHandler("endFirstCutscene", endFirstCutscene);

        // Friend stuff
        dr.AddCommandHandler("playerLied", playerLied);
        dr.AddCommandHandler("disable_friendWall", disableFriendWall);

        // Everything
        dr.AddCommandHandler("setOptionCountTo3", setOptionCountTo3);
        dr.AddCommandHandler("setOptionCountTo2", setOptionCountTo3);

        dr.AddFunction("checkObtainedCup", 1, delegate (Yarn.Value[] parameters)
        {
            if (hasMerchantCup == true)
            {
                merchantCupImage.enabled = false;
                hasMerchantCup = false;
                return 1;
            }
            else
            {
                return 0;
            }
        });
    }

    IEnumerator delayEnableCutscene1()
    {
        yield return new WaitForSeconds(1f);

        Timeline_Merch.SetActive(true);
        movement.enableDialogueProgress();
    }

    IEnumerator delayDisableCutscene1()
    {
        yield return new WaitForSeconds(1f);

        animator.Rebind();
        animator.Update(0f);
        merch3.SetActive(false);
        merch2.SetActive(true);
        Timeline_Merch.SetActive(false);
        //enable controls
    }

    private void enableTeleporter0(string[] arr)
    {
        teleporter0.SetActive(true);
        merch.SetActive(false);
        merch0.SetActive(true);
    }

    private void startFirstCutscene(string[] arr)
    {
        // start transition
        loader.Transition();
        // Disable controls
        movement.disableDialogueProgress();

        StartCoroutine(delayEnableCutscene1());
    }

    private void endFirstCutscene(string[] arr)
    {
        // start transition
        loader.Transition();
        // Disable controls

        StartCoroutine(delayDisableCutscene1());
    }

    public void setOptionCountTo3(string[] arr)
    {
        dialogueControls = FindObjectOfType<DialogueControls>();
        dialogueControls.SetOptions(3);
    }

    public void setOptionCountTo2(string[] arr)
    {
        dialogueControls = FindObjectOfType<DialogueControls>();
        dialogueControls.SetOptions(2);
    }

    public void disableFriendWall(string[] arr)
    {
        friendWall.SetActive(false);
        friend1.SetActive(false);
        friend2.SetActive(true);
        sky.SetActive(false);
    }

    public void disableMerchantWall(string[] arr)
    {
        merchantWall.SetActive(false);
        movement2D.EnableDoubleJump();
    }

    public void playerLied(string[] arr)
    {
        didPlayerLie = true;
        Debug.Log("Player lied");
    }

    public void setMerchantCup(bool value)
    {
        hasMerchantCup = value;

        if (value)
        {
            merchantCupImage.enabled = true;
        }
        else
        {
            merchantCupImage.enabled = false;
        }
    }
    public bool getMerchantCup()
    {
        return hasMerchantCup;
    }
}
