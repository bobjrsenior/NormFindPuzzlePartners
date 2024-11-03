using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

[Serializable]
public class AudioObject
{
    public AudioClip audioClip;
    public float preWait;
    public float postWait;
    public float volume = 1.0f;
    public bool hasQuestion = false;
    public string option1;
    public string option2;
    public string option3;
    public int specialIndex1 = -1;
    public int specialIndex2 = -1;
    public int specialIndex3 = -1;
    public int specialLoadScene1 = -1;
    public int specialLoadScene2 = -1;
    public int specialLoadScene3 = -1;
    public bool stopBackgroundAudioBefore = false;
    public bool startBackgroundMusicBefore;
    public bool startBackgroundMusicAfter;
    public AudioClip backgroundAudioClip;
    public bool updateBackgroundVideo = false;
    public VideoClip videoClip;
    public bool loopVideo = true;
    public bool dontRepeatAfterSpecial = false;
    public bool startStaticAudio = false;
    public AudioClip staticAudioClip;
    public bool updateBackgroundImage = false;
    public Sprite backgroundSprite;
}
