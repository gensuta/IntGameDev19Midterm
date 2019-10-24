using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DialogueHandler : MonoBehaviour
{
    public bool isBattleMode;
    [Space]
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
    public bool isShowingDefeat;

    AudioController ac;
    SceneController sc;
    float timer;
    bool isCountingDown;

    public int whichVoice;

    // Use this for initialization
    void Start()
    {
        ac = FindObjectOfType<AudioController>();
        sc = FindObjectOfType<SceneController>();
    }

    // Update is called once per frame
    void Update()
    {

        if(isCountingDown)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                DisableTextBox();
                isCountingDown = false;
            }
        }

        if (!isBattleMode)
        {
            if (isActive && Input.GetKeyDown(KeyCode.Space))
            {
                if (already)
                {
                    if (!isTyping) // if no text is typing go to next line OR disable textbox
                    {
                        currentLine += 1;
                        if (currentLine >= myLines.Count)
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
        else
        {
            //if (isActive && Input.GetKeyDown(KeyCode.Space))
            //{
            //    if (isTyping && !cancelTyping)
            //    {
            //        cancelTyping = true;
            //        already = false;
            //        timer = 0.2f;
            //    }
            //}
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
            if (!isBattleMode)
            {
                ac.PlayVoiceClip(ac.voiceClips[whichVoice]);
            }
            letter += 1;
            _text.maxVisibleCharacters = letter;
            yield return new WaitForSeconds(textSpeed);
        }

        _text.maxVisibleCharacters = _text.text.Length;
        isTyping = false;
        cancelTyping = false;
    }

    public void DisplayBattleText(string _text, float amt = 3.5f)
    {
        textHolder.SetActive(true);
        isActive = true;
        BeginWaitingTime(amt);
        StartCoroutine(Textscroll(_text));
    }

    void BeginWaitingTime(float amt)
    {
        timer = amt;
        isCountingDown = true;
    }

    public void EnableTextBox()
    {
        StartCoroutine(Textscroll(myLines[currentLine]));
        textHolder.SetActive(true);
        isActive = true;
    }

    public void DisableTextBox()
    {
        already = false;
        isActive = false;
        textHolder.SetActive(false);
        _text.text = "";

        if (!isBattleMode && !isShowingDefeat)
        {
            ac.FadeOut();
            sc.WaitThenLoad("Battle", 1f, 1);
            //to ensure the label is off during the transition
            label.SetActive(false);
            FindObjectOfType<PlayerMovement>().GetComponent<BoxCollider>().enabled = false;
        }
        else
        {
            isShowingDefeat = false;
        }

    }
    public bool DoesContain(string theThing)
    {
        return myLines[currentLine].Contains(theThing);
    }

    public void CheckCharacter() // checks who to show
    {

    }
}
