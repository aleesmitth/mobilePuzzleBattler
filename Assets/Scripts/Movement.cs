using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Movement {
    private List<Tilemap> groundTilemaps;
    private List<Tilemap> collisionTilemaps;
    private readonly float VALID_DIRECTION_SIZE = 0.2f;
    public Movement(Tilemap groundTilemap, Tilemap collisionTilemap) {
        this.groundTilemaps = new List<Tilemap> {groundTilemap};
        this.collisionTilemaps = new List<Tilemap> {collisionTilemap};
    }
    
    public Movement(Tilemap[] groundTilemaps, Tilemap collisionTilemap) {
        this.groundTilemaps = new List<Tilemap>(groundTilemaps);
        this.collisionTilemaps = new List<Tilemap> {collisionTilemap};
    }
    
    public Movement(Tilemap[] groundTilemaps, Tilemap[] collisionTilemaps) {
        this.groundTilemaps = new List<Tilemap>(groundTilemaps);
        this.collisionTilemaps = new List<Tilemap>(collisionTilemaps);
    }
    public Vector3 Move(Vector3 originPosition, Vector3 destination, float speed) {
        Vector2 direction = (destination - originPosition).normalized;
        if (CanMove(originPosition, direction) && (originPosition != destination)) {
            originPosition = Vector2.MoveTowards
            (originPosition
                , destination
                , speed * Time.deltaTime);
        }

        return originPosition;
    }
    
    private bool CanMove(Vector3 position, Vector2 direction) {
        Vector3Int gridPosition;
        foreach (var groundTilemap in groundTilemaps) {
            gridPosition = groundTilemap.WorldToCell(position + ((Vector3)direction * VALID_DIRECTION_SIZE));

            if (!groundTilemap.HasTile(gridPosition)) continue;
            
            if (collisionTilemaps.Any(collisionTilemap => collisionTilemap.HasTile(gridPosition))) {
                return false;
            }

            return true;
        }

        return false;
    }
}