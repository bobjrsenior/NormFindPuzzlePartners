using UnityEngine;

public class AL1SpecialProcessing : MonoBehaviour
{
    public float fadeTime = 1.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (AudioHandler.instance.audioState == AudioHandler.AudioState.DONE)
        {
            
        }    
    }
}
