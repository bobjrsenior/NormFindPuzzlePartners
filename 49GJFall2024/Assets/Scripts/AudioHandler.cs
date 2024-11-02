using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    public enum AudioState {
        LOAD_NEXT,
        PRE_WAIT,
        PLAYING_AUDIO,
        POST_WAIT,
        DONE
    };
    
    public AudioSource audioSource;
    public List<AudioObject> audioObjects;
    public int audioIndex = -1;
    public AudioState audioState = AudioState.LOAD_NEXT;
    public float timer = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        //audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (audioState)
        {
            case AudioState.LOAD_NEXT:
                audioIndex += 1;
                if (audioIndex == audioObjects.Count)
                {
                    audioState = AudioState.DONE;
                }
                else
                {
                    audioSource.clip = audioObjects[audioIndex].audioClip;
                    audioSource.volume = audioObjects[audioIndex].volume;
                    timer = audioObjects[audioIndex].preWait;
                    audioState = AudioState.PRE_WAIT;
                }
            break;
            case AudioState.PRE_WAIT:
                timer -= Time.deltaTime;
                if (timer <= 0.0f)
                {
                    audioState = AudioState.PLAYING_AUDIO;
                    audioSource.Play();
                }
            break;
            case AudioState.PLAYING_AUDIO:
                if (!audioSource.isPlaying)
                {
                    audioState = AudioState.POST_WAIT;
                    timer = audioObjects[audioIndex].postWait;
                }
            break;
            case AudioState.POST_WAIT:
            timer -= Time.deltaTime;
                if (timer <= 0.0f)
                {
                    audioState = AudioState.LOAD_NEXT;
                }
            break;
            case AudioState.DONE:
                // Load next scene
                Debug.Log("DONE");
            break;
        }
    }
}
