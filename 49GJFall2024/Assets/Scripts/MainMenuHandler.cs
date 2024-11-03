using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    public AudioSource audioSource;

    void Start()
    {
        StaticAudioHandler.instance.StopAudio();
        if (StaticAudioHandler.instance.gameBeat)
        {
            audioSource.Play();
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void RecapPuzzlePalace()
    {
        SceneManager.LoadScene(9);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
