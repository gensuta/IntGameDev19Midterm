using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    SceneController sc;
    TransitionHandler th;
    AudioController ac;

    public static GameController gc;
    // Start is called before the first frame update
    void Start()
    {
        if (gc == null)
        {
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

}
