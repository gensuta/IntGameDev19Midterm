using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public int currentScene;
    public float timer;
    public bool timerIsOn;

    public string storedName;

    TransitionHandler th;
    AudioController ac;

    // Start is called before the first frame update
    void Start()
    {
        ac = FindObjectOfType<AudioController>();
        th = FindObjectOfType<TransitionHandler>();
        storedName = "";
        timer = 0.5f;
        timerIsOn = true;
    }


    // Update is called once per frame
    void Update()
    {
        currentScene = SceneManager.GetActiveScene().buildIndex;

        if (currentScene == 0)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !timerIsOn) // keeps people from starting the game too fast
            {
                ForwardAScene();
            }
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                BackAScene();
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                ForwardAScene();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                ResetScene();
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                LoadScene("Hallway"); // go to "beginning" scene
            }
        }


        if (timerIsOn)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                if (storedName != "")
                {
                    LoadScene(storedName);
                }
                timerIsOn = false;
            }
        }
    }

    public string GetSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }

    public void BackAScene() //when you wanna go back one scene
    {
        if (currentScene != 0)
        {
            currentScene -= 1;
            LoadScene(currentScene);
        }
    }
    public void ForwardAScene() //when you wanna go forward one scene
    {
        if (currentScene != SceneManager.sceneCountInBuildSettings - 1)
        {
            currentScene += 1;
            LoadScene(currentScene);
        }
    }
    public void ResetScene()
    {
        LoadScene(currentScene);
    }

    public void LoadScene(int num)
    {
        SceneManager.LoadScene(num);
        ac.GetSFXSource();
    }

    public void LoadScene(string sName)
    {
        SceneManager.LoadScene(sName);
        ac.GetSFXSource();

    }

    public void WaitThenLoad(string sName, float time, int transition = 0)
    {
        th.PlayAnim(transition);
        timerIsOn = true; 
        timer = time;
        storedName = sName;
    }
    public void WaitThenTransitionAndLoad(string sName, float time, int transition = 0)
    {
        th.PlayDelayedAnim(transition,time - 0.5f);
        timerIsOn = true;
        timer = time;
        storedName = sName;
    }
}
