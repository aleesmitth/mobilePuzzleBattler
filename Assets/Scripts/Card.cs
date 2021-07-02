using System.Collections.Generic;
using UnityEngine;

public class Card {
    private string name;
    private NodeType type;

    public Card(string name, NodeType type) {
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

    public void PlayCard(Dictionary<NodeType,float> elementsDamage) {
        foreach (var elementDamage in elementsDamage) {
            if (elementDamage.Key == type) {
                //do stuff
                Debug.Log("carta de nombre: " + name + " de tipo " + type + " hizo accion de " + elementDamage.Value);
            }
        }
    }
}