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

    public Sprite[] types; // 0 rock kind, 1 tired scissors, 2 paper lively

    [Space]
    [Header("UI mess")]
    public TextMeshProUGUI bpTxt;
    public TextMeshProUGUI nameTxt;
    public Slider bpSlider;
    public Image type;

    bool madeMove;
    int rand;

    // Start is called before the first frame update
    void Start()
    {
        bh = FindObjectOfType<BattleHandler>();
        if (bh.opponent._type == GameController.Types.kindness)
        {
            type.sprite = types[0];
        }
        else if (bh.opponent._type == GameController.Types.tiredness)
        {
            type.sprite = types[1];
        }
        else if (bh.opponent._type == GameController.Types.liveliness)
        {
            type.sprite = types[2];
        }

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
            if (!bh.opponent.moves[rand].onSelf)
            {
                bh.dh.DisplayBattleText(bh.opponent._name + " used " + bh.opponent.moves[rand]._name + " on the player!");
            }
            else
            {
                bh.dh.DisplayBattleText(bh.opponent._name + " used " + bh.opponent.moves[rand]._name + "!");
            }
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
        bh.opponent.moves[rand].UseMoveOnPlayer(player);
        bh.dh.DisplayBattleText(bh.opponent.moves[rand].GetRandomDialogue());
        madeMove = true; 
    }
    public void BeQuirky()
    {
        bh.dh.DisplayBattleText("The opponent does something quirky but it means nothing to you. ");
        bh.player.BeginPlayerTurn();
    }
}
