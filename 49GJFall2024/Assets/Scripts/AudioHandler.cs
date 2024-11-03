using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class AudioHandler : MonoBehaviour
{
    public static AudioHandler instance;
    public enum AudioState {
        LOAD_NEXT,
        LOAD_SPECIAL,
        PRE_WAIT,
        PLAYING_AUDIO,
        POST_WAIT,
        WAIT_FOR_USER_RESPONSE,
        DONE
    };
    
    public AudioSource audioSource;
    public AudioSource backgroundAudioSource;
    public VideoPlayer videoPlayer;
    public List<AudioObject> audioObjects;

    public List<AudioObject> specialAudioObjects;
    public AudioObject curAudioObject;
    public int audioIndex = -1;
    public AudioState audioState = AudioState.LOAD_NEXT;
    public float timer = 0.0f;
    public bool playingSpecial;
    public int specialIndex;
    public Button button1;
    public TextMeshProUGUI option1;
    public Button button2;
    public TextMeshProUGUI option2;
    public Button button3;
    public TextMeshProUGUI option3;
    public float finalDelay = 0.0f;
    public Image backgroundImage;
    public bool lastScene = false;
    public bool isRecap = false;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //audioSource = GetComponent<AudioSource>();
        button1.gameObject.SetActive(false);
        button2.gameObject.SetActive(false);
        button3.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        switch (audioState)
        {
            case AudioState.LOAD_SPECIAL:
                curAudioObject = specialAudioObjects[specialIndex];
                if (!curAudioObject.dontRepeatAfterSpecial)
                    audioIndex -= 1; // Go back after playing
                
                audioSource.clip = curAudioObject.audioClip;
                audioSource.volume = curAudioObject.volume;
                timer = curAudioObject.preWait;
                if(curAudioObject.stopBackgroundAudioBefore)
                {
                    backgroundAudioSource.Stop();
                }
                if(curAudioObject.startBackgroundMusicBefore)
                {
                    backgroundAudioSource.clip = curAudioObject.backgroundAudioClip;
                    backgroundAudioSource.Play();
                }
                if(curAudioObject.updateBackgroundVideo)
                {
                    videoPlayer.clip = curAudioObject.videoClip;
                    videoPlayer.isLooping = curAudioObject.loopVideo;
                    videoPlayer.Play();
                }
                if(curAudioObject.startStaticAudio)
                {
                    StaticAudioHandler.instance.PlayClip(curAudioObject.staticAudioClip, true);
                }
                if(curAudioObject.updateBackgroundImage)
                {
                    backgroundImage.sprite = curAudioObject.backgroundSprite;
                }
                audioState = AudioState.PRE_WAIT;
            break;
            case AudioState.LOAD_NEXT:
                audioIndex += 1;
                if (audioIndex == audioObjects.Count)
                {
                    if (finalDelay > 0.0f)
                    {
                        videoPlayer.Stop();
                    }
                    if(lastScene)
                    {
                        StaticAudioHandler.instance.StopAudio();
                    }
                    audioState = AudioState.DONE;
                }
                else
                {
                    curAudioObject = audioObjects[audioIndex];
                    audioSource.clip = curAudioObject.audioClip;
                    audioSource.volume = curAudioObject.volume;
                    timer = curAudioObject.preWait;
                    if(curAudioObject.stopBackgroundAudioBefore)
                    {
                        backgroundAudioSource.Stop();
                    }
                    if(curAudioObject.startBackgroundMusicBefore && (backgroundAudioSource.clip == null || backgroundAudioSource.clip.name != curAudioObject.backgroundAudioClip.name))
                    {
                        backgroundAudioSource.clip = curAudioObject.backgroundAudioClip;
                        backgroundAudioSource.Play();
                    }
                    if(curAudioObject.updateBackgroundVideo)
                    {
                        videoPlayer.clip = curAudioObject.videoClip;
                        videoPlayer.isLooping = curAudioObject.loopVideo;
                        videoPlayer.Play();
                    }
                    if(curAudioObject.startStaticAudio)
                    {
                        StaticAudioHandler.instance.PlayClip(curAudioObject.staticAudioClip, true);
                    }
                    if(curAudioObject.updateBackgroundImage)
                    {
                        backgroundImage.sprite = curAudioObject.backgroundSprite;
                    }
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
                    if(curAudioObject.startBackgroundMusicAfter)
                    {
                        backgroundAudioSource.clip = curAudioObject.backgroundAudioClip;
                        backgroundAudioSource.Play();
                    }
                    audioState = AudioState.POST_WAIT;
                    timer = curAudioObject.postWait;
                }
            break;
            case AudioState.POST_WAIT:
            timer -= Time.deltaTime;
                if (timer <= 0.0f)
                {
                    if (curAudioObject.hasQuestion)
                    {
                        //specialIndex = curAudioObject.specialIndex;
                        //playingSpecial = true;
                        SetupQuestion();
                    }
                    else if (playingSpecial)
                    {
                        playingSpecial = false;
                        audioState = AudioState.LOAD_NEXT;
                    }
                    else
                    {
                        audioState = AudioState.LOAD_NEXT;
                    }
                }
            break;
            case AudioState.DONE:
                // Load next scene
                finalDelay -= Time.deltaTime;
                
                if (finalDelay <= 0.0f)
                {
                    if (!lastScene && !isRecap)
                    {
                        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                    }
                    else if(isRecap)
                    {
                        SceneManager.LoadScene(0);
                    }
                    else
                    {
                        StaticAudioHandler.instance.gameBeat = true;
                        SceneManager.LoadScene(0);
                    }
                }
            break;
        }
    }

    public void SetupQuestion()
    {
        if (curAudioObject.option1.Length > 0)
        {
            option1.text = curAudioObject.option1;
            button1.gameObject.SetActive(true);
        }
        if (curAudioObject.option2.Length > 0)
        {
            option2.text = curAudioObject.option2;
            button2.gameObject.SetActive(true);
        }
        
        if (curAudioObject.option3.Length > 0)
        {
            option3.text = curAudioObject.option3;
            button3.gameObject.SetActive(true);
        }
        audioState = AudioState.WAIT_FOR_USER_RESPONSE;
    }

    public void HideQuestions()
    {
        option1.text = "";
        option2.text = "";
        option3.text = "";
        button1.gameObject.SetActive(false);
        button2.gameObject.SetActive(false);
        button3.gameObject.SetActive(false);
    }

    public void Response1()
    {
        HideQuestions();
        if (curAudioObject.specialIndex1 >= 0)
        {
            specialIndex = curAudioObject.specialIndex1;
            playingSpecial = true;
            audioState = AudioState.LOAD_SPECIAL;
        }
        else if(curAudioObject.specialLoadScene1 >= 0)
        {
            SceneManager.LoadScene(curAudioObject.specialLoadScene1);
        }
        else
        {
            playingSpecial = false;
            audioState = AudioState.LOAD_NEXT;
        }
    }

    public void Response2()
    {
        HideQuestions();
        if (curAudioObject.specialIndex2 >= 0)
        {
            specialIndex = curAudioObject.specialIndex2;
            playingSpecial = true;
            audioState = AudioState.LOAD_SPECIAL;
        }
        else if(curAudioObject.specialLoadScene2 >= 0)
        {
            SceneManager.LoadScene(curAudioObject.specialLoadScene2);
        }
        else
        {
            playingSpecial = false;
            audioState = AudioState.LOAD_NEXT;
        }
    }

    public void Response3()
    {
        HideQuestions();
        if (curAudioObject.specialIndex3 >= 0)
        {
            specialIndex = curAudioObject.specialIndex3;
            playingSpecial = true;
            audioState = AudioState.LOAD_SPECIAL;
        }
        else if(curAudioObject.specialLoadScene3 >= 0)
        {
            SceneManager.LoadScene(curAudioObject.specialLoadScene3);
        }
        else
        {
            playingSpecial = false;
            audioState = AudioState.LOAD_NEXT;
        }
    }
}
