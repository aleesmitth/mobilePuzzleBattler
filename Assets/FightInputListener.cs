using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FightInputListener : MonoBehaviour {
    public NodeGrid grid;
    public Sprite hitNodeSprite;
    
    //TODO theres a bug if i press S -> A -> S the game breaks, it's because A is mostly for debugging.
    //TODO i'm not sure why it happens but for now it's not worth looking into, A will be removed, it's just for show
    private void Update() {
        
        if (Input.GetKeyDown(KeyCode.Space)) {
            grid.ResetGrid();
        }
        
        if (Input.GetKeyDown(KeyCode.S)) {
            var hits = grid.LookForMatrixHits();
            grid.DestroyHitNodes(hits);
        }
        
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene("Scenes/Map", LoadSceneMode.Single);
        }
        
        //TODO this is just for debug
        if (Input.GetKeyDown(KeyCode.A)) {
            var hits = grid.GetMatrixHits();
            foreach (var listOfHits in hits) {
                foreach (var hit in listOfHits) {
                    hit.GetComponent<SpriteRenderer>().sprite = hitNodeSprite;
                }
            }
        }
    }
}
