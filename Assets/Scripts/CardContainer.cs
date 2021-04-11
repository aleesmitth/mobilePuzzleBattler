using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardContainer : MonoBehaviour {
    private Card card;

    public void SetCard(Card card) {
        this.card = card;
        card.SetContainer(this);
    }
}
