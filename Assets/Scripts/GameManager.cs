using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public NodeGrid grid;
    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if(instance!=this)
            Destroy(gameObject);
        
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            grid.ResetGrid();
        }
    }
}
