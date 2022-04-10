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

    private float zoomSpeed = 3f;
    private float zoomInMax = 40f;
    public float zoomOutMax = 90f;

    private Camera cam;

    public GameObject teleporter0;

    public Image merchantCupImage;
    public bool hasMerchantCup = false;

    private void Awake()
    {
        dialogueUI = FindObjectOfType<DialogueUI2>();
        //dialogueControls = FindObjectOfType<DialogueControls>();
        cam = Camera.main;
        merchantCupImage.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        dr.AddCommandHandler("enable_teleporter0", enableTeleporter0);

        dr.AddCommandHandler("startFirstCutscene", startFirstCutscene);
        dr.AddCommandHandler("endFirstCutscene", endFirstCutscene);

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

    // Update is called once per frame
    void Update()
    {

    }

    private void enableTeleporter0(string[] arr)
    {
        teleporter0.SetActive(true);
        merch.SetActive(false);
        merch0.SetActive(true);
    }

    private void startFirstCutscene(string[] arr)
    {
        Timeline_Merch.SetActive(true);
    }

    private void endFirstCutscene(string[] arr)
    {
        merch3.SetActive(false);
        merch2.SetActive(true);
        Timeline_Merch.SetActive(false);
    }

    public void setOptionCountTo3(string[] arr)
    {
        dialogueControls.SetOptions(3);
    }

    public void setOptionCountTo2(string[] arr)
    {
        dialogueControls.SetOptions(2);
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
