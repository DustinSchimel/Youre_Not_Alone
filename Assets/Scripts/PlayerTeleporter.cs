using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTeleporter : MonoBehaviour
{
    private GameObject currentTeleporter;

    // Update is called once per frame


    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.CompareTag("Teleporter"))
        {
                transform.position = collision.gameObject.GetComponent<Teleporter>().GetDestination().position;      
        }
    }
}
