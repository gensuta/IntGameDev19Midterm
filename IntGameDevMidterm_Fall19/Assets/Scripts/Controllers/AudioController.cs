using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{

    public AudioSource aud;

    [Space]
    [Header("Bools")]
    public bool didStart;
    public bool fadingIn;
    public bool hasLastBeatPassed;

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
}
