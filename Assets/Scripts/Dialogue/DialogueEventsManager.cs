using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class DialogueEventsManager : MonoBehaviour
{
    public DialogueRunner dr;

    public bool cupRetrieved;

    public GameObject teleporter0;

    // Start is called before the first frame update
    void Start()
    {
        //string[] dsakl = new string[1]; // enable these two strings to skip intro dialogue
        //enableTeleporter0(dsakl);

        dr.AddCommandHandler("enable_teleporter0", enableTeleporter0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void enableTeleporter0(string[] arr)
    {
        teleporter0.SetActive(true);
    }
}
