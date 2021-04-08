using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public NodeGrid grid;
    public Material hitNodeMat;
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

        if (Input.GetKeyDown(KeyCode.A)) {
            var hits = grid.GetMatrixHits();
            foreach (var listOfHits in hits) {
                foreach (var hit in listOfHits) {
                    hit.GetComponent<MeshRenderer>().material = hitNodeMat;
                }
            }
        }
        
        if (Input.GetKeyDown(KeyCode.S)) {
            var hits = grid.LookForMatrixHits();
            grid.DestroyHitNodes(hits);
        }
    }
    
    //here i should process hit info, as damage, target, combo etc.
    //double check but pretty sure that list of hits, has 1 list per line that hit, with no duplicated nodes if
    //it hits both vertically and horizontally
    //so i could do like
    //Combo points = 1 + numberOfLists * 0.1f (this would give me 10% bonus increase per line hit.
    //elementDamage = NodesOfElement * Combo points
}
