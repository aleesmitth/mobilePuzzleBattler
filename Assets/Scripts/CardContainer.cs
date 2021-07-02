using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class CardContainer : MonoBehaviour {
    private Card card;
    public TextMeshProUGUI cardTextDisplay;
    public Material[] materials;

    public void SetCard(Card card) {
        this.card = card;
        var cardName = card.GetName();
        var cardType = card.GetCardType();
        print(" has the card name " + cardName + " and type "  + cardType);
        cardTextDisplay.text = card.GetName();
        ChangeCardModel(cardType);
    }

    //TODO kinda hard coded, refactor later
    private void ChangeCardModel(NodeType cardType) {
        this.gameObject.GetComponentInChildren<MeshRenderer>().material = cardType switch {
            NodeType.Cube => materials[0],
            NodeType.Sphere => materials[1],
            NodeType.Cylinder => materials[2],
            NodeType.Capsule => materials[3],
            _ => materials[0]
        };
    }
}
