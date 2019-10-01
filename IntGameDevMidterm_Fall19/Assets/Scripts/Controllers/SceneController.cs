using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // to keep track of score and other things as you proceed from level to level
    public int currentScene;

    public  SceneController sC;

    public float timer;
    public bool timerIsOn;

    public string storedName;


    // Start is called before the first frame update
    void Start()
    {
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
    }

    public void LoadScene(string sName)
    {
        SceneManager.LoadScene(sName);
    }

    public void WaitThenLoad(string sName, float time)
    {
        timerIsOn = true;
        timer = time;
        storedName = sName;
    }
}
