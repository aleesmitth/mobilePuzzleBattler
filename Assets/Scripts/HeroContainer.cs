using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class HeroContainer : MonoBehaviour {
    private Hero _hero;
    public TextMeshProUGUI cardTextDisplay;
    public Sprite[] sprites;

    public void SetHero(Hero hero) {
        this._hero = hero;
        var cardName = hero.GetName();
        var cardType = hero.GetCardType();
        print(" has the card name " + cardName + " and type "  + cardType);
        cardTextDisplay.text = hero.GetName();
        ChangeCardModel(cardType);
    }

    //TODO kinda hard coded, refactor later
    private void ChangeCardModel(NodeType cardType) {
        this.gameObject.GetComponentInChildren<SpriteRenderer>().sprite = cardType switch {
            NodeType.Cube => sprites[0],
            NodeType.Sphere => sprites[1],
            NodeType.Cylinder => sprites[2],
            NodeType.Capsule => sprites[3],
            _ => sprites[0]
        };
    }
}
