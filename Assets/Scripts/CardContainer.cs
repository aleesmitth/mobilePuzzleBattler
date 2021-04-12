using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardContainer : MonoBehaviour {
    private Card card;
    public TextMeshProUGUI cardText;

    public void SetCard(Card card) {
        this.card = card;
        print(" has the card name " + card.GetName());
        cardText.text = card.GetName();
        card.SetContainer(this);
    }
}
