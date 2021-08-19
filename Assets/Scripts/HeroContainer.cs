using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
[System.Serializable]
public struct InspectorElementsAndSprites {
    public ElementType elementType;
    [Header("Evolution number has to be unique")]
    [Space]
    public InspectorEvolutionsAndSprites[] evolutionsAndSprites;
}

[System.Serializable]
public struct InspectorEvolutionsAndSprites {
    public HeroEvolutionNumber evolutionNumber;
    public Sprite sprite;
}
public class HeroContainer : MonoBehaviour {
    private Hero _hero;
    [Header("Element type has to be unique")]
    [Space]
    public InspectorElementsAndSprites[] inspectorElementsAndSprites;

    public void SetHero(Hero hero) {
        this._hero = hero;
        var heroName = hero.GetName();
        var heroType = hero.GetHeroType();
        var heroEvolution = hero.GetEvolution();
        print(" has the hero name " + heroName + " and type "  + heroType + " and evolution " + heroEvolution);
        ChangeCardModel(heroType, heroEvolution);
    }

    //TODO kinda hard coded, refactor later
    private void ChangeCardModel(ElementType cardType, HeroEvolutionNumber evolutionNumber) {
        foreach (var element in inspectorElementsAndSprites) {
            if (element.elementType != cardType) continue;
            foreach (var evolution in element.evolutionsAndSprites) {
                if (evolution.evolutionNumber != evolutionNumber) continue;
                this.gameObject.GetComponentInChildren<SpriteRenderer>().sprite = evolution.sprite;
            }
        }
    }
}
