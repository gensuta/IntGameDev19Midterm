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
    }

	void UpdateUI()
	{
        storedBp = Mathf.Lerp(storedBp, bh.opponent.currentBp, lerpSpeed * Time.deltaTime);
        bpTxt.text = "Bond Points: " + bh.opponent.currentBp;
		bpSlider.value = storedBp;
	}

    public void ChooseRandomMove()
    {
        int rand = Random.Range(0, bh.opponent.moves.Length);
        bh.opponent.moves[rand].UseMoveOnPlayer(player);
        bh.dh.DisplayBattleText(bh.opponent._name + " used " + bh.opponent.moves[rand]._name + " on the player!");
        bh.player.BeginPlayerTurn();
    }
    public void BeQuirky()
    {
        bh.dh.DisplayBattleText("The opponent does something quirky but it means nothing to you. ");
        bh.player.BeginPlayerTurn();
    }
}
