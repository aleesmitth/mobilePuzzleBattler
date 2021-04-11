using System;
using System.Collections;
using System.ComponentModel.Design;
using UnityEngine;

public class Player : MonoBehaviour {
    private Deck deck;
    public HandOfCards hand;

    public void Draw(int amount) {
        hand.Draw(amount, deck);
    }
}