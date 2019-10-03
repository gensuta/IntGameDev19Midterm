using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractBehavior : MonoBehaviour
{
    GameController gc;
    public Character character;
    public string _name;
    // Start is called before the first frame update
    void Start()
    {
        gc = FindObjectOfType<GameController>();
        _name = character._name;

        if(gc.CheckIfSame(_name))
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
