﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DialogueHandler : MonoBehaviour
{
    //appears when in area of object
    [Header("Label Related")]
    public GameObject label;
    public TextMeshProUGUI labelText;

    [Space]
    [Header("Textbox Objects")]
    public GameObject textHolder;
    public TextMeshProUGUI _text;
    public TextMeshProUGUI nameTxt;
    public Image img; // shows whos taalking

    
    public List<string> myLines;
    public int currentLine;
    public float textSpeed;

    [Space]
    [Header("Dialogue Runner Bools")]
    bool isTyping = false;
    bool cancelTyping = false;
    public bool isActive;// are things showing?
    public bool already; // did you press space already?

    // Use this for initialization
    void Start()
    {
        if (isActive)
        {
            EnableTextBox();
        }
        else
        {
            DisableTextBox();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive && Input.GetKeyDown(KeyCode.Space))
        {
            if (already)
            {
                if (!isTyping) // if no text is typing go to next line OR disable textbox
                {
                    currentLine += 1;
                    if (currentLine >= myLines.Count - 1)
                    {
                        DisableTextBox();
                    }
                    else
                    {
                        StartCoroutine(Textscroll(myLines[currentLine]));
                    }
                }
                else if (isTyping && !cancelTyping) // cancel typing!
                {
                    cancelTyping = true;
                    already = false;
                }
            }
            else
            {
                already = true; // you pressed space already
            }
        }
    }

    private IEnumerator Textscroll(string lineoftext)
    {
        int letter = 0;
        _text.maxVisibleCharacters = 0;
  
        _text.text = lineoftext;

        isTyping = true;
        cancelTyping = false;

        while (isTyping && !cancelTyping && (letter < _text.text.Length - 1))
        {
            letter += 1;
            _text.maxVisibleCharacters = letter;
            yield return new WaitForSeconds(textSpeed);
        }

        _text.text = lineoftext;
        isTyping = false;
        cancelTyping = false;
    }

    public void EnableTextBox()
    {
        StartCoroutine(Textscroll(myLines[currentLine]));
        textHolder.SetActive(true);
        isActive = true;
    }

    public void DisableTextBox()
    {
        isActive = false;
        textHolder.SetActive(false);
        _text.text = "";
    }
    public bool DoesContain(string theThing)
    {
        return myLines[currentLine].Contains(theThing);
    }

    public void CheckCharacter() // checks who to show
    {

    }
}
