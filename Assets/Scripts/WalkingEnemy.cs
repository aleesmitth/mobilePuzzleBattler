using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class WalkingEnemy : MonoBehaviour {
    public float speed;
    public float chanceToWalk;
    public float minSecondsSleep;
    public float maxSecondsSleep;
    public int minTilesToWalk;
    public int maxTilesToWalk;
    private Tilemap groundTilemap;
    //private List<Vector2Int> occupiedTilePositions;
    private Dictionary<int, Vector2Int> mapOfGridPositions;
    // 0 - 1 - 2
    // 3 - x - 4
    // 5 - 6 - 7
    private Movement movement;
    private Vector3 moveTowards;
    private bool startMoving;

    public void SetEnemyData(/*List<Vector2Int> occupiedTiles, */Tilemap groundTilemap, Tilemap collisionTilemap) {
        //this.occupiedTilePositions = occupiedTiles;
        this.groundTilemap = groundTilemap;

        mapOfGridPositions = new Dictionary<int, Vector2Int> {
            {0, new Vector2Int(-1, -1)},
            {1, new Vector2Int(0, -1)},
            {2, new Vector2Int(1, -1)},
            {3, new Vector2Int(-1, 0)},
            {4, new Vector2Int(1, 0)},
            {5, new Vector2Int(-1, 1)},
            {6, new Vector2Int(0, 1)},
            {7, new Vector2Int(1, 1)}
        };
        movement = new Movement(groundTilemap, collisionTilemap);
        StartCoroutine(Move());
    }

    private void Update() {
        if (!startMoving) return;
        transform.position = movement.Move(transform.position, moveTowards, speed);
    }

    private IEnumerator Move() {
        while (enabled) {
            yield return new WaitForSeconds(Random.Range(minSecondsSleep, maxSecondsSleep));
            
            if (!(Random.value < chanceToWalk)) continue;
            
            int tilesToMove = Random.Range(minTilesToWalk, maxTilesToWalk);


            var worldPosition =
                groundTilemap.layoutGrid.CellToWorld((Vector3Int) mapOfGridPositions[Random.Range(0, 7)] *
                                                     tilesToMove);
            worldPosition.x += .5f; //offset because of celltoworld error

            //UpdateOccupiedPositions();
            
            this.moveTowards = worldPosition + transform.position;
            this.startMoving = true;
        }

        yield return null;
    }

    /*private void UpdateOccupiedPositions() {
        Vector2Int cellCurrentPosition = (Vector2Int)groundTilemap.WorldToCell(transform.position);
        if (occupiedTilePositions.Contains(cellCurrentPosition)) occupiedTilePositions.Remove(cellCurrentPosition);
        occupiedTilePositions.Add((Vector2Int)groundTilemap.WorldToCell(moveTowards));
    }*/
}
