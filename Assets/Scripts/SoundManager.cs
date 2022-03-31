using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    public enum Sound
    {
        PlayerMoveGrass,
        PlayerMoveStone,
        PlayerJump,
        PlayerDash,
        PlayerDie,
        BackgroundMusic,
        AmbientNoise,
    }

    private static Dictionary<Sound, float> soundTimerDictionary;

    public static void Initialize()
    {
        soundTimerDictionary = new Dictionary<Sound, float>();
        soundTimerDictionary[Sound.PlayerMoveGrass] = 0f;
    }
    public static void PlaySound(Sound sound)
    {
        if (CanPlaySound(sound))
        {
            GameObject soundGameObject = new GameObject("Sound");
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();

            if (sound == Sound.BackgroundMusic || sound == Sound.AmbientNoise)
            {
                audioSource.loop = true;
            }

            if(sound == Sound.AmbientNoise && CaveBarrier.inCave == true)
            {
                audioSource.loop = false;
            }

            audioSource.volume = 0.1f;

            if (PauseMenu.gameIsPaused)
            {
                audioSource.pitch *= 0.5f;
            }

            if(sound == Sound.AmbientNoise && CaveBarrier.inCave == true )
            {

            }
            else
            {
                audioSource.PlayOneShot(GetAudioClip(sound));
            }
                
        }
    }

    private static bool CanPlaySound (Sound sound)
    {
        switch (sound)
        {
            default:
                return true;
            case Sound.PlayerMoveGrass:
                if (soundTimerDictionary.ContainsKey(sound))
                {
                    float lastTimePlayed = soundTimerDictionary[sound];
                    float playerMoveTimerMax = 0.543f;
                    if (lastTimePlayed + playerMoveTimerMax < Time.time)
                    {
                        soundTimerDictionary[sound] = Time.time;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return true;
                }
                //break;
        }
    }

    private static AudioClip GetAudioClip(Sound sound)
    {
        foreach(GameAssets.SoundAudioClip soundAudioClip in GameAssets.i.soundAudioClipArray)
        {   
            if (soundAudioClip.sound == sound)
            {
                return soundAudioClip.audioClip;
            }
        }

        Debug.LogError("Sound " + sound + " not found!");
        return null;
    }
}
