using System;
using UnityEngine;

[Serializable]
public class AudioObject
{
    public AudioClip audioClip;
    public float preWait;
    public float postWait;
    public float volume = 1.0f;
}
