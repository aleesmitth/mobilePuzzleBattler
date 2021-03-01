using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using Random = UnityEngine.Random;

public class NodeGrid : MonoBehaviour {
    public Vector3Value gridSize;
    public Vector3Value nodeSize;
    public float normalNodeHeight;
    public float selectedNodeHeight;
    private Node[][] grid;
    private int xNodes;
    private int zNodes;
    private Node selectedNode;
    private GameObject selectedNodeGO;
    private Camera mainCamera;
    private Vector3 mouseClickPosition;
    private GameObject[][] gridGO;

    private void Start() {
        mainCamera = Camera.main;
        CreateGrid();
    }

    private void Update() {
        if (grid == null) return;
        var mousePosition = Input.mousePosition;
        mousePosition.z = mainCamera.transform.position.y;
        mousePosition = mainCamera.ScreenToWorldPoint(mousePosition);
        if (Input.GetMouseButtonDown(0))
            mouseClickPosition = mousePosition;
        float xPercentage = (mousePosition.x + gridSize.value.x / 2) / gridSize.value.x;
        float zPercentage = (mousePosition.z + gridSize.value.z / 2) / gridSize.value.z;
        int xIndex = Mathf.FloorToInt(Mathf.Clamp(xNodes * xPercentage, 0, xNodes - 1));
        int zIndex = Mathf.FloorToInt(Mathf.Clamp(zNodes * zPercentage, 0, zNodes - 1));
        if(selectedNode != null)
            selectedNode.isSelected = false;
        selectedNode = grid[xIndex][zIndex];
        selectedNode.isSelected = true;
        
        if (gridGO == null) return;
        if (selectedNodeGO != null) {
            var normalPosition = selectedNodeGO.transform.position;
            normalPosition.y = normalNodeHeight;
            selectedNodeGO.transform.position = normalPosition;
        }
        selectedNodeGO = gridGO[xIndex][zIndex];
        var selectedPosition = selectedNodeGO.transform.position;
        selectedPosition.y = selectedNodeHeight;
        selectedNodeGO.transform.position = selectedPosition;
    }

    private void CreateGrid() {
        xNodes = Mathf.FloorToInt(gridSize.value.x / nodeSize.value.x);
        zNodes = Mathf.FloorToInt(gridSize.value.z / nodeSize.value.z);
        grid = new Node[xNodes][];
        for (int i = 0; i < grid.Length; i++) {
            grid[i] = new Node[zNodes];
            for (int j = 0; j < grid[i].Length; j++) {
                grid[i][j] = new Node(new Vector3
                    (i * nodeSize.value.x + nodeSize.value.x/2, 0, j * nodeSize.value.z + nodeSize.value.z/2) - gridSize.value / 2);
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
                if(node.isSelected)
                    Gizmos.color = Color.yellow;
                Gizmos.DrawWireCube(node.position, nodeSize.value - .1f * Vector3.one);
            }
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(mouseClickPosition, .1f);
    }

    public void ResetGrid() {
        if (gridGO == null) {
            gridGO = new GameObject[xNodes][];
            for (int i = 0; i < gridGO.Length; i++) {
                gridGO[i] = new GameObject[zNodes];
            }
        }
        else {
            for (int i = 0; i < gridGO.Length; i++) {
                for (int j = 0; j < gridGO[i].Length; j++) {
                    switch (grid[i][j].nodeType) {
                        case NodeType.Cube: CubePool.instance.DestroyObject(gridGO[i][j]);
                            break;
                        case NodeType.Sphere: SpherePool.instance.DestroyObject(gridGO[i][j]);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        for (int i = 0; i < gridGO.Length; i++) {
            for (int j = 0; j < gridGO[i].Length; j++) {
                grid[i][j].nodeType = (Random.value > .5) ? NodeType.Cube : NodeType.Sphere;
                switch (grid[i][j].nodeType) {
                    case NodeType.Cube: gridGO[i][j] = CubePool.instance.Get();
                        break;
                    case NodeType.Sphere: gridGO[i][j] = SpherePool.instance.Get();
                        break;
                    default:
                        break;
                }
                gridGO[i][j].transform.position = grid[i][j].position;
                gridGO[i][j].transform.localScale = new Vector3(nodeSize.value.x / 2, 1, nodeSize.value.z / 2);
            }
        }
    }
}
