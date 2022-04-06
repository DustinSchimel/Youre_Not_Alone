using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    private AudioSource source;
    private bool pauseVolumeSet;

    [Range(0, 1f)]
    public float volume = 0.7f;

    [Range(0.5f, 1.5f)]
    public float pitch = 1f;

    public bool loop = false;

    public void SetSource(AudioSource _source)
    {
        source = _source;
        source.clip = clip;
    }

    public void Play()
    {
        source.volume = volume;
        source.pitch = pitch;
        source.loop = loop;
        source.Play();
    }

    public void StopPlaying()
    {
        source.Stop();
    }

    public void PauseVolume()
    {
        if (!pauseVolumeSet)
        {
            pauseVolumeSet = true;
            source.pitch *= 0.5f;
            source.volume *= 0.5f;
        }

    }

    public void NormalVolume()
    {
        if (pauseVolumeSet)
        {
            pauseVolumeSet = false;
            source.pitch *= 2;
            source.volume *= 2;
        }
    }
}


public class AudioManager : MonoBehaviour
{
    private static Dictionary<Sound, float> soundTimerDictionary;
    public static AudioManager instance;

    [SerializeField]
    Sound[] sounds;

    private void Awake()
    {
        soundTimerDictionary = new Dictionary<Sound, float>();
        if (instance != null)
        {
            Debug.LogError("TOO MANY AUDIO MANAGERS");
        }
        else
        {
            instance = this;
        }

    }

    private void Start()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject _go = new GameObject("Sound_" + i + "_" + sounds[i].name);
            _go.transform.SetParent(this.transform);
            sounds[i].SetSource(_go.AddComponent<AudioSource>());
            if (sounds[i].name == "Walking (Grass)")
            {
                soundTimerDictionary[sounds[i]] = 0f;
            }
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (PauseMenu.gameIsPaused)
            {
                sounds[i].PauseVolume();
            } else if (!PauseMenu.gameIsPaused)
            {
                sounds[i].NormalVolume();
            }
            if(sounds[i].name == "Wind")
            {
                if (CaveBarrier.inCave)
                {
                    sounds[i].StopPlaying();
                }
            }
        }


    }

    public void PlaySound(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                if (sounds[i].name == "Walking (Grass)")
                {
                    if (CanPlaySound(sounds[i]))
                    {
                        sounds[i].Play();
                    }
                    return;

                }
                sounds[i].Play();
                return;
            }
        }
    }

    private bool CanPlaySound(Sound sound)
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
    //break;
}
