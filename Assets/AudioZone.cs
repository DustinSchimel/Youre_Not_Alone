using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioZone : MonoBehaviour
{
    public AudioManager manager;
    public static bool CaveMusicIsPlaying = false;
    public static bool FriendMusicIsPlaying = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "CaveMusic")
        {
            if (!CaveMusicIsPlaying)
            {
                manager.playCaveMusic();
                CaveMusicIsPlaying = true;
            }
           
        }

        if (collision.tag == "FriendMusic")
        {
            if (!FriendMusicIsPlaying)
            {
                manager.playFriendMusic();
                FriendMusicIsPlaying = true;
            }

        }
    }
}
