using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    private List<AudioSource> currentAudioSources = new List<AudioSource>();
    private AudioSource currentSong;
    private List<AudioSource> tickingSounds;

    public AudioMixerGroup sFXMixerGroup;
    public AudioMixerGroup musicMixerGroup;
    public AudioClip songLoop;

    // Start is called before the first frame update
    void Start()
    {
        currentSong = GetComponent<AudioSource>();
        float introLength = currentSong.clip.length;

        //currentAudioSources.Add(gameObject.GetComponent<AudioSource>());
    }

    private void IntroEnded()
    {
        currentSong.clip = songLoop;
        currentSong.Play();
        currentSong.loop = true;
    }

    public void PlayOneShot(AudioClip clip)
    {
        foreach (AudioSource source in currentAudioSources)
        {
            if (source.isPlaying)
            {
                break;
            }
            source.PlayOneShot(clip);
            return;
        }

        AudioSource temp = gameObject.AddComponent<AudioSource>();
        temp.outputAudioMixerGroup = sFXMixerGroup;
        currentAudioSources.Add(temp);
        temp.PlayOneShot(clip);
        StartCoroutine(WaitForOneShot(clip, temp));
    }

    IEnumerator WaitForOneShot(AudioClip clip, AudioSource audioSource)
    {
        float lengthOfAudio = clip.length;
        yield return new WaitForSeconds(lengthOfAudio);
        currentAudioSources.Remove(audioSource);
        Destroy(audioSource);
    }

    private void Update()
    {
        if (!currentSong.isPlaying)
        {
            IntroEnded();
        }
    }
}
