using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SimonSaysHandler : MonoBehaviour
{
    public static SimonSaysHandler instance;

    public enum GameState {
        GAME_SETUP,
        INIT,
        PLAYER_TURN,
        GENERATE_PATTERN,
        SHOW_PATTERN,
        DONE
    }

    public List<GameObject> colorsObj;
    public List<SimonButtonScript> colors;

    public GameState curGameState = GameState.INIT;

    public int numPatterns = 0;
    public List<int> order; 

    public float initialCooldown = 2.0f;
    public float patternCooldown = 2.5f;
    public float cooldownTimer;
    public int curPatternIndex;
    public Color saveColor;

    public AudioSource failAudioSource;
    public AudioClip failAudioClip;
    public int endAtPattern = 7;
    public float doneDelay = 1.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
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

    void Start ()
    {
        order = new List<int>();
        foreach(GameObject obj in colorsObj)
        {
            var image = obj.GetComponent<UnityEngine.UI.Image>();
            //colors.Add(image);
        }
        numPatterns = 1;
    }

    // Update is called once per frame
    void Update()
    {
        switch(curGameState)
        {
            case GameState.GAME_SETUP:
                initialCooldown -= Time.deltaTime;
                if (initialCooldown <= 0.0f)
                {
                    curGameState = GameState.INIT;
                }
            break;
            case GameState.INIT:
                cooldownTimer -= Time.deltaTime;
                if (cooldownTimer <= 0.0f)
                {
                    curGameState = GameState.GENERATE_PATTERN;
                }
            break;
            case GameState.GENERATE_PATTERN:
                //order.Clear();
                //for (int i = 0; i < numPatterns; i++)
                //{
                order.Add(UnityEngine.Random.Range(0, 4));
                //}
                curGameState = GameState.SHOW_PATTERN;
                curPatternIndex = -1;
                cooldownTimer = 0.0f;
                break;
            case GameState.SHOW_PATTERN:
                if(cooldownTimer <= 0.0f)
                {
                    if (curPatternIndex >= 0)
                    {
                        //var image = colors[order[curPatternIndex]];
                        //image.color = saveColor;
                    }
                    curPatternIndex += 1;
                    if (curPatternIndex == numPatterns)
                    {
                        curGameState = GameState.PLAYER_TURN;
                        curPatternIndex = 0;
                    }
                    else
                    {
                        var image = colors[order[curPatternIndex]];
                        image.Press();
                        //saveColor = image.color;
                        //image.color = Color.white;
                        cooldownTimer = patternCooldown;
                    }
                }
                else{
                    cooldownTimer -= Time.deltaTime;
                }
            break;
            case GameState.DONE:
                doneDelay -= Time.deltaTime;
                if (doneDelay <= 0.0f)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                }
            break;
        }
    }

    public void Clicked(int index)
    {
        if (curGameState != GameState.PLAYER_TURN)
        {
            return;
        }
        
        if (order[curPatternIndex] != index)
        {
            failAudioSource.clip = failAudioClip;
            order.Clear();
            numPatterns = 1;
            failAudioSource.Play();
            curPatternIndex = 0;
            StartInit();
        }
        else
        {
            colors[index].Press();
            curPatternIndex += 1;
            if (curPatternIndex == order.Count)
            {
                curPatternIndex = 0;
                numPatterns += 1;
                if (numPatterns == endAtPattern)
                {
                    curGameState = GameState.DONE;
                }
                else
                {
                    StartInit();
                }
            }
        }
    }

    public void StartInit()
    {
        cooldownTimer = patternCooldown;
        curGameState = GameState.INIT;
    }

    public void ClickedBlue()
    {
        Clicked(0);
    }

    public void ClickedRed()
    {
        Clicked(1);
    }

    public void ClickedGreen()
    {
        Clicked(2);
    }

    public void ClickedYellow()
    {
        Clicked(3);
    }
}
