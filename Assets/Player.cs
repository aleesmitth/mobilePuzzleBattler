using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    private Hero[] heros;
    public Hero[] GetHeros() {
        return this.heros;
    }

    public void SetStartingHeros(Hero[] startingHeros) {
        this.heros = startingHeros;
        print("i got the starting heros (:");
    }
}
