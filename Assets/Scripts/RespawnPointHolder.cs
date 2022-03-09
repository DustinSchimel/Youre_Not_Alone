using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPointHolder : MonoBehaviour
{
    public Vector2 respawnPoint;

    public Vector2 getPoint()
    {
        return respawnPoint;
    }
}