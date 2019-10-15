using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OpponentActions : MonoBehaviour
{
    BattleHandler bh;
    PlayerActions player;

    float storedBp;
    public float lerpSpeed = 0.25f; // for slider

    [Space]
    [Header("UI mess")]
    public TextMeshProUGUI bpTxt;
    public TextMeshProUGUI nameTxt;
    public Slider bpSlider;

    bool madeMove;
    int rand;

    // Start is called before the first frame update
    void Start()
    {
        bh = FindObjectOfType<BattleHandler>();
        player = FindObjectOfType<PlayerActions>();


        storedBp = bh.opponent.currentBp;
        bpSlider.maxValue = 20;
        nameTxt.text = bh.opponent._name;

    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();

        if(madeMove && !bh.dh.isActive)
        {
            bh.dh.DisplayBattleText(bh.opponent._name + " used " + bh.opponent.moves[rand]._name + " on the player!");
            bh.player.BeginPlayerTurn();
            madeMove = false;

        }
    }

	void UpdateUI()
	{
        storedBp = Mathf.Lerp(storedBp, bh.opponent.currentBp, lerpSpeed * Time.deltaTime);
        bpTxt.text = "Bond Points: " + bh.opponent.currentBp;
		bpSlider.value = storedBp;
        ClampStuff();
	}

    void ClampStuff()
    {
        if (bh.opponent.currentBp > 20)
        {
            bh.opponent.currentBp = 20;
        }
        if (bh.opponent.currentBp < 0)
        {
            bh.opponent.currentBp = 0;
        }

    }

    public void ChooseRandomMove()
    {
        rand = Random.Range(0, bh.opponent.moves.Length);
        bh.dh.DisplayBattleText(bh.opponent.moves[rand].GetRandomDialogue());
        madeMove = true; 
    }
    public void BeQuirky()
    {
        bh.dh.DisplayBattleText("The opponent does something quirky but it means nothing to you. ");
        bh.player.BeginPlayerTurn();
    }
}
