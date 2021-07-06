using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Deck {
    private LinkedList<Hero> heros;

    public Deck() {
        heros = new LinkedList<Hero>();
    }
    public void UpdateActiveHeroes(int amount, LinkedList<Hero> heros) {
        //TODO placeholder, i'm adding 4 heros on enable and then removing them and setting them as active.
        if (this.heros.Count == 0) return;
        for (int i = 0; i < amount; i++) {
            heros.AddLast(this.heros.First.Value);
            this.heros.RemoveFirst();
        }
    }

    public void AddStartingHeros() {
        for (int i = 0; i < 4; i++) {
            //hard coded random card type
            Array values = Enum.GetValues(typeof(NodeType));
            NodeType randomType = (NodeType) values.GetValue((int) (Random.value * values.Length));

            heros.AddLast(new Hero("Hero n° " + heros.Count, randomType));
            Debug.Log("Added hero, now the total is " + heros.Count + " heros.");
        }
    }
}