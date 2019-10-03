using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OpponentActions : MonoBehaviour
{
    BattleHandler bh;
    PlayerActions player;

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
		bpTxt.text = "Bond Points: " + bh.opponent.currentBp;
		bpSlider.value = bh.opponent.currentBp;
	}

    public void ChooseRandomMove()
    {
        int rand = Random.Range(0, bh.opponent.moves.Length);
        bh.opponent.moves[rand].UseMoveOnPlayer(player);
        Debug.Log(bh.opponent._name + " used " + bh.opponent.moves[rand]._name + " on the player!");
        bh.player.BeginPlayerTurn();
    }
    public void BeQuirky()
    {
        Debug.Log("The opponent does something quirky but it has no effect");
        bh.player.BeginPlayerTurn();
    }
}
