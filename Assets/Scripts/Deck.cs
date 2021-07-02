using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Deck {
    private LinkedList<Card> cards;

    public Deck() {
        cards = new LinkedList<Card>();
    }
    public void Draw(int amount, LinkedList<Card> cards) {
        if (this.cards.Count == 0) return;
        for (int i = 0; i < amount; i++) {
            cards.AddLast(this.cards.First.Value);
            this.cards.RemoveFirst();
        }
    }

    public void AddCard() {
        //hard coded random card type
        Array values = Enum.GetValues(typeof(NodeType));
        NodeType randomType = (NodeType)values.GetValue((int)(Random.value * values.Length));
        
        cards.AddLast(new Card("Card n° " + cards.Count, randomType));
        Debug.Log("Added cads, now the total is " + cards.Count + " cards.");
    }
}