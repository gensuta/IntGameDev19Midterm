using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    SceneController sc;
    TransitionHandler th;
    AudioController ac;

    public static GameController gc;
	public bool isFirstBattle;

    public Character storedChar;

    public int whichOpponent;
    public Character[] opponents;

    // Start is called before the first frame update
    void Awake()
    {
        if (gc == null)
        {
            foreach(Character opponent in opponents)
            {
                opponent.isDefeated = false;
            }
            gc = this;
            DontDestroyOnLoad(gc);
        }
        else if (gc != this)
        {
            Destroy(gameObject);
        }

        ac = FindObjectOfType<AudioController>();
        sc = FindObjectOfType<SceneController>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public enum Types
    {
        kindness, // kindness beats tiredness
        tiredness, // tiredness beats liveliness
        liveliness // liveliness beats kindness
    }

    public bool CheckIfSame(string _name)
    {
        for (int i = 0; i < gc.opponents.Length; i++)
        {
            if (gc.opponents[i]._name == _name && gc.opponents[i].isDefeated)
            {
                return true;
            }
        }
        return false;
    }

    public int GetTwin(string _name)
    {
        for (int i = 0; i < gc.opponents.Length; i++)
        {
            if (gc.opponents[i]._name == _name)
            {
                return i;
            }
        }
        return 0;
    }
}
