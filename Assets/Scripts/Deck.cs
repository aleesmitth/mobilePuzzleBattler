using System.Collections.Generic;

public class Deck {
    private LinkedList<Card> cards;
    public void Draw(int amount, LinkedList<Card> cards) {
        for (int i = 0; i < amount; i++) {
            cards.AddFirst(this.cards.First.Value);
            this.cards.RemoveFirst();
        }
    }
}