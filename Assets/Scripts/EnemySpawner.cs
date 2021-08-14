using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour {
    public Tilemap groundTilemap;
    public Tilemap collisionTilemap;
    public int maxEnemies;
    //private List<Vector2Int> occupiedTilePositions;
    private int activeEnemies;

    private void Awake() {
        //occupiedTilePositions = new List<Vector2Int>();
    }

    private void OnEnable() {
        activeEnemies = 0;
    }

    private void Update() {
        if (activeEnemies >= maxEnemies) return;
        activeEnemies++;
        SummonEnemy();
    }

    private void SummonEnemy() {
        //falta el rol para ver si spawneo o no, y hacer algun sleep
        GameObject enemyInstace = EnemyPool.instance.Get();
        enemyInstace.transform.SetParent(transform, false);

        var randomGridPosition = GetRandomGridPosition();
        var worldPosition = groundTilemap.layoutGrid.CellToWorld((Vector3Int) randomGridPosition);
        worldPosition.x += .5f; //offset because of celltoworld error
        enemyInstace.transform.position = worldPosition;
        enemyInstace.GetComponent<WalkingEnemy>().SetEnemyData(/*occupiedTilePositions, */groundTilemap, collisionTilemap);
    }

    private Vector2Int GetRandomGridPosition() {
        Vector2Int randomGridPosition = Vector2Int.zero;
        do {
            foreach (Vector3Int position in groundTilemap.cellBounds.allPositionsWithin) {
                if (!groundTilemap.HasTile(position)) continue;
                if (!(Random.value < .01)) continue; //chance of spawning enemy has to be low because always iterating
                //the tiles the same way
                randomGridPosition = (Vector2Int)position;
                break;
            }
        } while (!ValidPosition(randomGridPosition));
        //occupiedTilePositions.Add(randomGridPosition);
        return randomGridPosition;
    }

    private bool ValidPosition(Vector2Int position) {
        return groundTilemap.HasTile((Vector3Int) position)/* && !occupiedTilePositions.Contains(position)*/;
    }
}
