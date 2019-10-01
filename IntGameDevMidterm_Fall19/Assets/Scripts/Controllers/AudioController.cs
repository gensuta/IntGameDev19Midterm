using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{

    public AudioSource aud;
    public AudioSource sfxAud; // handles sfx
    [Space]
    public bool fadingIn;

    // Start is called before the first frame update
    void Start()
    {
        aud = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (fadingIn)
        {
            if (aud.volume < 1f)
            {
                aud.volume += 0.3f * Time.deltaTime;
            }
            else
            {
                fadingIn = false;
            }
        }
        else
        {
            aud.volume = 1f;
        }
    }

    public void SwitchSong(AudioClip song, bool isLooping = true)
    {
        fadingIn = false;
        aud.clip = song;
        aud.loop = isLooping;

        aud.Play();
    }
    public void FadeIn(AudioClip song)
    {
        aud.Stop();
        aud.time += 30f;
        aud.volume = 0f;
        aud.clip = song;
        aud.Play();
        fadingIn = true;
    }

    public void PlaySFX(AudioClip clip, float vol = 1f)
    {
        sfxAud.clip = clip;
        sfxAud.Play();
    }

}
