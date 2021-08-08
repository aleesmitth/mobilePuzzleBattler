using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorGrid2D : MonoBehaviour {
    public Vector3Value gridSize;
    public Vector3Value nodeSize;
    private int xNodes;
    private int yNodes;
    private Node[][] grid;
    private GameObject[][] gridGO;
    private Vector3[][] positionsGrid;

    private void Awake() {
        CreateGrid();
    }
    
    private void CreateGrid() {
        xNodes = Mathf.FloorToInt(gridSize.value.x / nodeSize.value.x);
        yNodes = Mathf.FloorToInt(gridSize.value.y / nodeSize.value.y);
        grid = new Node[xNodes][];
        positionsGrid = new Vector3[xNodes][];
        for (int i = 0; i < grid.Length; i++) {
            grid[i] = new Node[yNodes];
            positionsGrid[i] = new Vector3[yNodes];
            for (int j = 0; j < grid[i].Length; j++) {
                var nodePosition = new Vector3
                                   (i * nodeSize.value.x + nodeSize.value.x / 2,
                                       j * nodeSize.value.y + nodeSize.value.y / 2, 0) -
                                   gridSize.value / 2;
                grid[i][j] = new Node(nodePosition);
                positionsGrid[i][j] = nodePosition;
            }
        }
    }
    
    private void OnDrawGizmos() {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position, gridSize.value);
        if(grid == null) return;
        foreach (var row in grid) {
            foreach (var node in row) {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireCube(node.position, nodeSize.value);
            }
        }
    }

    public void ResetGrid() {
        if (gridGO == null) {
            gridGO = new GameObject[xNodes][];
            for (int i = 0; i < gridGO.Length; i++) {
                gridGO[i] = new GameObject[yNodes];
            }
        }
        else {
            for (int i = 0; i < gridGO.Length; i++) {
                for (int j = 0; j < gridGO[i].Length; j++) {
                    InventoryPool.instance.DestroyObject(gridGO[i][j]);
                }
            }
        }

        for (int i = 0; i < gridGO.Length; i++) {
            for (int j = 0; j < gridGO[i].Length; j++) {
                grid[i][j].ResetNodeType();
                gridGO[i][j] = InventoryPool.instance.Get();
                gridGO[i][j].GetComponent<MaterialController>().MakeVisible();
                gridGO[i][j].transform.position = grid[i][j].position;
            }
        }
    }
}