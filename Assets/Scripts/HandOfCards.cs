using System.Collections.Generic;
using UnityEngine;

public class HandOfCards : MonoBehaviour {
    private LinkedList<Card> cards;
    public Vector3Value gridSize;
    public FloatValue porcentageSpaceBetweenCards;
    //4 cards fixed size of hand, to not make it dynamic because it's much more complicated.
    public FloatValue handSize;
    private GameObject[] cardsGO;

    private void OnEnable() {
        //check if cards haven enough space to be placed
        if (porcentageSpaceBetweenCards.value >= 1 / handSize.value)
            porcentageSpaceBetweenCards.value = .1f; //default to .1 offset
        cardsGO = new GameObject[(int)handSize.value];
        float cardSizeX = (gridSize.value.x - (porcentageSpaceBetweenCards.value * gridSize.value.x * (handSize.value - 1))) / handSize.value;
        for (int i = 0; i < handSize.value; i++) {
            cardsGO[i] = CardsPool.instance.Get();
            var position = cardsGO[i].transform.position;
            var mr = cardsGO[i].GetComponent<MeshRenderer>();
            var bounds = mr.bounds;
            
            var scale = cardsGO[i].transform.localScale;
            //i scale it to be a square
            scale.x = cardSizeX / bounds.size.x;
            scale.z = cardSizeX / bounds.size.z;
            cardsGO[i].transform.localScale = scale;
            
            position.x = (- gridSize.value.x + cardSizeX) / 2 + i * cardSizeX + i * porcentageSpaceBetweenCards.value * gridSize.value.x;
            position.z = (gridSize.value.z + bounds.size.z * scale.z) / 2;
            position.y = 0;
            cardsGO[i].transform.position = position;
        }
    }

    private void OnDisable() {
        for (int i = 0; i < handSize.value; i++) {
            CardsPool.instance.DestroyObject(cardsGO[i]);
        }
    }

    public void Draw(int amount, Deck deck) {
        this.cards = new LinkedList<Card>();
        deck.Draw(amount, cards);
        int i = 0;
        foreach (var card in cards) {
            print("my physicall card, number " + i);
            cardsGO[i].GetComponent<CardContainer>().SetCard(card);
            i++;
        }
    }

    public void PlayCards(Dictionary<NodeType,float> elementsDamage) {
        foreach (var card in cards) {
            card.PlayCard(elementsDamage);
        }
    }
}