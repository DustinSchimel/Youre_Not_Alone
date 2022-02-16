using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class DialogueRangeCheck : MonoBehaviour
{
    public Flowchart flowchart;
    public string inNPCRange;

    void OnTriggerEnter2D()
    {
        flowchart.SetBooleanVariable(inNPCRange, true); //"inWinstonRange"
    }

    void OnTriggerExit2D()
    {
        flowchart.SetBooleanVariable(inNPCRange, false);    //"inWinstonRange"
    }
}