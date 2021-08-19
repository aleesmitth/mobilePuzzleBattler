using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingChest : MonoBehaviour {
    public Hero[] startingHeros;
    public Player player;
    private void OnMouseDown() {
        player.SetStartingHeros(startingHeros);
    }
}
