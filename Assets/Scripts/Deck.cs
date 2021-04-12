using System.Collections.Generic;
using UnityEngine;

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
        cards.AddLast(new Card("Card n° " + cards.Count));
        Debug.Log("Added" + cards.Count + " cards.");
    }
}