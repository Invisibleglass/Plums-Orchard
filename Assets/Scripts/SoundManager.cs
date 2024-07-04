using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    private List<AudioSource> currentAudioSources = new List<AudioSource>();

    public AudioMixerGroup sFXMixerGroup;
    public AudioMixerGroup musicMixerGroup;

    // Start is called before the first frame update
    void Start()
    {
        //currentAudioSources.Add(gameObject.GetComponent<AudioSource>());
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

    public void StopAllSFX()
    {
        for (int i = 0; i < currentAudioSources.Count; i++)
        {
            Destroy(currentAudioSources[i]);
        }
    }
}
