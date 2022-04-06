using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.UI;

public class DialogueEventsManager : MonoBehaviour
{
    public DialogueRunner dr;

    public GameObject teleporter0;

    public Image merchantCupImage;
    public bool hasMerchantCup = false;

    private void Awake()
    {
        merchantCupImage.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        //string[] dsakl = new string[1]; // enable these two strings to skip intro dialogue
        //enableTeleporter0(dsakl);

        dr.AddCommandHandler("enable_teleporter0", enableTeleporter0);

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
