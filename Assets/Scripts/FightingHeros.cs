using System;
using System.Collections.Generic;
using UnityEngine;

public class FightingHeros : MonoBehaviour {
    private LinkedList<Hero> activeHeros;
    public Vector3Value gridSize;
    public FloatValue porcentageSpaceBetweenCards;
    //4 cards fixed size of hand, to not make it dynamic because it's much more complicated.
    public FloatValue handSize;
    private GameObject[] herosGO;

    private void PositionHeroContainersInWorldMap() {//check if cards haven enough space to be placed
        if (porcentageSpaceBetweenCards.value >= 1 / handSize.value)
            porcentageSpaceBetweenCards.value = .1f; //default to .1 offset
        herosGO = new GameObject[(int)handSize.value];
        float cardSizeX = (gridSize.value.x - (porcentageSpaceBetweenCards.value * gridSize.value.x * (handSize.value - 1))) / handSize.value;
        int a = 0;
        for (int i = 0; i < 150000; i++) {
            a++;
        }
        print(a);
        for (int i = 0; i < handSize.value; i++) {
            if (HerosContainerPool.instance == default) print("AAAAAAAAAAAAAAAAAAAAA");
            herosGO[i] = HerosContainerPool.instance.Get();
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
        EventManager.onNodesDamageCalculated -= Attack;
        if (herosGO == default) return;
        for (int i = 0; i < handSize.value; i++) {
            HerosContainerPool.instance.DestroyObject(herosGO[i]);
        }
    }

    private void OnEnable() {
        EventManager.onNodesDamageCalculated += Attack;
    }

    public void Attack(Dictionary<ElementType,float> elementsDamage) {
        foreach (var hero in activeHeros) {
            hero.Attack(elementsDamage);
        }
    }

    private void Start() {
        if (GameContextData.PlayerHeros == default) return;
        this.activeHeros = new LinkedList<Hero>(GameContextData.PlayerHeros);
        int i = 0;
        PositionHeroContainersInWorldMap();
        foreach (var hero in activeHeros) {
            print("my physicall hero, number " + i);
            herosGO[i].GetComponent<HeroContainer>().SetHero(hero);
            i++;
        }
    }
}