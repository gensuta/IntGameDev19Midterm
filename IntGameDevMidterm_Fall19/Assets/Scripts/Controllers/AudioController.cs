using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    SceneController sc;

    public AudioSource aud;
    public AudioSource[] battleAuds;
    public AudioSource sfxAud; // handles sfx

    [Space]
    bool fadingIn;
    bool fadingOut;

    public AudioClip[] voiceClips; // 0 tired, 1 kind, 2 lively
    public AudioClip[] battleNoises; // 0 bondUP, 1 bondDown, 2pChange, 3win, 4lose

    public AudioClip hallwayMusic;

    // Start is called before the first frame update
    void Start()
    {
        aud = GetComponent<AudioSource>();
        sc = FindObjectOfType<SceneController>();
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
        else if (fadingOut)
        {
            if(aud.volume > 0)
            {
                aud.volume -= 0.5f * Time.deltaTime;
            }
            else
            {
                FadeIn();
                fadingOut = false;

            }
        }
        else
        {
            aud.volume = 1f;
        }
    }

    public void SwitchSong(AudioClip song, bool isLooping = true)
    {
        foreach (AudioSource _aud in battleAuds)
        {
            _aud.Stop();
        }

        FadeOut();
        fadingIn = false;
        aud.clip = song;
        aud.loop = isLooping;
    }

    public void BeginBattleMusic()
    {
        aud.Stop();
        fadingOut = false;
        foreach (AudioSource _aud in battleAuds)
        {
            _aud.volume = 0;
            _aud.Play();
        }

        if(sc.GetSceneName() == "Battle")
        {
            FadeIn();
            PlayerActions player = FindObjectOfType<PlayerActions>();
            aud = battleAuds[player.currentPersona];
        }
    }

    public void FadeIn(AudioClip song)
    {
        //aud.Stop();
        aud.volume = 0f;
        aud.clip = song;
        //aud.Play();
        fadingIn = true;
    }


    public void FadeIn()
    {
        if (sc.GetSceneName() == "Battle")
        {
            PlayerActions player = FindObjectOfType<PlayerActions>();
            aud = battleAuds[player.currentPersona];
        }

       // aud.Stop();
        aud.volume = 0f;
        //aud.Play();
        fadingIn = true;
    }

    public void FadeOut()
    {
        fadingOut = true;
    }

    public void PlaySFX(AudioClip clip, float vol = 0.5f)
    {
        sfxAud.pitch = 1;
        sfxAud.volume = vol;
        sfxAud.clip = clip;
        sfxAud.Play();
    }

    public void PlayVoiceClip(AudioClip clip)
    {
        sfxAud.pitch = Random.Range(3, -3);
        sfxAud.clip = clip;
        sfxAud.Play();
    }

}
