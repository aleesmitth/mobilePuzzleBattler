using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour {
    private Deck deck;
    public HandOfCards hand;

    private void Start() {
        deck = new Deck();
        deck.AddStartingHeros();
        hand.UpdateActiveHeros(deck);
    }

    public void Attack(Dictionary<NodeType,float> elementsDamage) {
        hand.Attack(elementsDamage);
    }
}