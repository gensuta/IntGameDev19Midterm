using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractBehavior : MonoBehaviour
{

    public Character character;
    public string _name;
    // Start is called before the first frame update
    void Start()
    {
        _name = character.name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
