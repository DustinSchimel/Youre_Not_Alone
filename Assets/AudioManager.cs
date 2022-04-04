using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    private AudioSource source;

    [Range(0, 1f)]
    public float volume = 0.7f;

    [Range(0.5f, 1.5f)]
    public float pitch = 1f;

    public bool loop = false;

    public void SetSource (AudioSource _source)
    {
        source = _source;
        source.clip = clip;
    }

    public void Play()
    {
        source.volume = volume;
        source.pitch = pitch;
        source.loop = loop;

        if (source.name == "Walking (Grass)")
            if (!source.isPlaying)
            {
                source.Play();
            }
            else
            {
                source.Stop();
               
            }
        else
            Debug.Log("1");
        source.PlayOneShot(source.clip);
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
        else {
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
            if(sounds[i].name == "Walking (Grass)")
            {               
                soundTimerDictionary[sounds[i]] = 0f;
            }
        }
    }

    public void PlaySound(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if(sounds[i].name == _name)
            {
                if(_name == "Walking (Grass)")
                {
                    //if(CanPlaySound(sounds[i]))
                    //{
                    //    sounds[i].Play();
                    //}
                }
                sounds[i].Play();
                return;
            }
        }

        //no sound with name
        Debug.LogWarning("NO SOUND WITH THAT NAME FOUND");
    }

    private bool CanPlaySound(Sound sound)
    {
        switch (sound.name)
        {
            default:
                return true;
            case "Walking (Grass)":
                if (soundTimerDictionary.ContainsKey(sound))
                {
                    float lastTimePlayed = soundTimerDictionary[sound];
                    float playerMoveTimerMax = 1f;
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

}
