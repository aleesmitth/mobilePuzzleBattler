using System.Collections.Generic;
using UnityEngine;

public class Hero {
    private string name;
    private NodeType type;

    public Hero(string name, NodeType type) {
        this.name = name;
        this.type = type;
        Debug.Log("card with name: " + name + " type is: " + type);
    }

    public string GetName() {
        return this.name;
    }
    public NodeType GetCardType() {
        return this.type;
    }

    public void Attack(Dictionary<NodeType,float> elementsDamage) {
        foreach (var elementDamage in elementsDamage) {
            if (elementDamage.Key == type) {
                if (elementDamage.Value == 0)
                    continue;
                //do stuff
                Debug.Log("carta de nombre: " + name + " de tipo " + type + " hizo accion de " + elementDamage.Value);
                bool attackEveryone = (Random.value < .5); 
                Debug.Log("mandando ataque a todos los enemigos? " + attackEveryone);
                if (attackEveryone)
                    EventManager.OnAttackAllEnemies(elementDamage.Value);
                else {
                    EventManager.OnAttackOneEnemy(elementDamage.Value);
                }
            }
        }
    }
}