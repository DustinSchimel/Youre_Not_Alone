using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class CinemachineSwitcher : MonoBehaviour
{
    public CinemachineVirtualCamera vcam1; // Zoomed out
    public CinemachineVirtualCamera vcam2; // Zoomed in
    private int currentCamera;

    void Start()
    {
        
    }

    public void SwitchPriority()
    {
        if (currentCamera == 0) // Zoomed out
        {
            vcam1.Priority = 0;
            vcam2.Priority = 1;

            currentCamera = 1;
        }
        else // Zoomed in
        {
            vcam1.Priority = 1;
            vcam2.Priority = 0;

            currentCamera = 0;
        }
    }
}
