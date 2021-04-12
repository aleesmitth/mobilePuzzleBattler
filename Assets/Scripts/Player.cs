using System;
using System.Collections;
using System.ComponentModel.Design;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour {
    private Deck deck;
    public HandOfCards hand;

    private void OnEnable() {
        deck = new Deck();
    }

    public void Draw(int amount) {
        hand.Draw(amount, deck);
    }

    public void AddCardsToDeck(TMP_InputField amount) {
        for (int i = 0; i < int.Parse(amount.text); i++) {
            deck.AddCard();
        }
    }
}