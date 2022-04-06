using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.UI;

public class DialogueEventsManager : MonoBehaviour
{
    public DialogueRunner dr;
    private DialogueUI2 dialogueUI;
    private DialogueControls dialogueControls;
    public GameObject Timeline_Merch;
    public GameObject merch2;
    public GameObject merch3;

    private Camera cam;

    public GameObject teleporter0;

    public Image merchantCupImage;
    public bool hasMerchantCup = false;

    private void Awake()
    {
        dialogueUI = FindObjectOfType<DialogueUI2>();
        dialogueControls = FindObjectOfType<DialogueControls>();
        cam = Camera.main;
        merchantCupImage.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        //string[] dsakl = new string[1]; // enable these two strings to skip intro dialogue
        //enableTeleporter0(dsakl);

        dr.AddCommandHandler("enable_teleporter0", enableTeleporter0);

        dr.AddCommandHandler("startFirstCutscene", startFirstCutscene);

        dr.AddCommandHandler("endFirstCutscene", endFirstCutscene);

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

    // Update is called once per frame
    void Update()
    {
        
    }

    private void enableTeleporter0(string[] arr)
    {
        teleporter0.SetActive(true);
    }

    private void startFirstCutscene(string[] arr)
    {
        dialogueControls.SetOptions(3);
        Timeline_Merch.SetActive(true);
    }

    private void endFirstCutscene(string[] arr)
    {
        merch3.SetActive(false);
        merch2.SetActive(true);
        dialogueControls.SetOptions(2);
        Timeline_Merch.SetActive(false);

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
