using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Hero {
    public string name;
    public HeroEvolutionNumber evolutionNumber;
    public ElementType elementType;
    public int level;
    public int experience;

    public string GetName() {
        return this.name;
    }
    public ElementType GetHeroType() {
        return this.elementType;
    }

    public HeroEvolutionNumber GetEvolution() {
        return this.evolutionNumber;
    }

    public void Attack(Dictionary<ElementType,float> elementsDamage) {
        foreach (var elementDamage in elementsDamage) {
            if (elementDamage.Key == elementType) {
                if (elementDamage.Value == 0)
                    continue;
                //do stuff
                Debug.Log("hero de nombre: " + name + " de tipo " + elementType + " hizo accion de " + elementDamage.Value);
                EventManager.OnAttackOneEnemy(elementDamage.Value);
            }
        }
    }
}

public enum HeroEvolutionNumber {
    ONE,
    TWO,
    THREE
}