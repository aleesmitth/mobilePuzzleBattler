using System.Collections.Generic;
using UnityEngine;

public class FightingHeros : MonoBehaviour {
    private LinkedList<Hero> activeHeros;
    public Vector3Value gridSize;
    public FloatValue porcentageSpaceBetweenCards;
    //4 cards fixed size of hand, to not make it dynamic because it's much more complicated.
    public FloatValue handSize;
    private GameObject[] herosGO;

    private void PositionCardsInWorldMap() {//check if cards haven enough space to be placed
        if (porcentageSpaceBetweenCards.value >= 1 / handSize.value)
            porcentageSpaceBetweenCards.value = .1f; //default to .1 offset
        herosGO = new GameObject[(int)handSize.value];
        float cardSizeX = (gridSize.value.x - (porcentageSpaceBetweenCards.value * gridSize.value.x * (handSize.value - 1))) / handSize.value;
        for (int i = 0; i < handSize.value; i++) {
            herosGO[i] = CardsPool.instance.Get();
            var position = herosGO[i].transform.position;
            var sr = herosGO[i].GetComponent<SpriteRenderer>();
            var sizeOfSprite = sr.bounds.extents * 2;
            
            var scale = herosGO[i].transform.localScale;
            //i scale it to be a square
            Debug.Log("card size: " + cardSizeX + " size x: " + sizeOfSprite.x + " size y: " + sizeOfSprite.y
                      + " cardsize/size" + cardSizeX / sizeOfSprite.x + "," + cardSizeX / sizeOfSprite.y);
            scale.x = cardSizeX / sizeOfSprite.x;
            scale.y = cardSizeX / sizeOfSprite.y;
            herosGO[i].transform.localScale = scale;
            
            position.x = (- gridSize.value.x + cardSizeX) / 2 + i * cardSizeX + i * porcentageSpaceBetweenCards.value * gridSize.value.x;
            position.y = (gridSize.value.y + sizeOfSprite.y * scale.y) / 2;
            position.z = 0;
            herosGO[i].transform.position = position;
        }
    }

    private void OnDisable() {
        if (herosGO == default) return;
        for (int i = 0; i < handSize.value; i++) {
            CardsPool.instance.DestroyObject(herosGO[i]);
        }
    }

    public void Attack(Dictionary<ElementType,float> elementsDamage) {
        foreach (var hero in activeHeros) {
            hero.Attack(elementsDamage);
        }
    }

    public void LoadHeros(Hero[] heros) {
        if (heros == default) return;
        this.activeHeros = new LinkedList<Hero>(heros);
        int i = 0;
        PositionCardsInWorldMap();
        foreach (var hero in activeHeros) {
            print("my physicall hero, number " + i);
            herosGO[i].GetComponent<HeroContainer>().SetHero(hero);
            i++;
        }
    }
}