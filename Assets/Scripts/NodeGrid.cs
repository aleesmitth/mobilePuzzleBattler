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
    private Node hoveredNode;
    private GameObject hoveredNodeGO;
    private Camera mainCamera;
    private Vector3 mouseClickPosition;
    private GameObject[][] gridGO; //the first [] are the col, second [] the rows. bottom left is 0,0
    private GameObject selectedNodeGO;
    private Node selectedNode;

    private void Start() {
        mainCamera = Camera.main;
        CreateGrid();
    }

    private void Update() {
        if (grid == null) return;
        var mousePosition = Input.mousePosition;
        mousePosition.z = mainCamera.transform.position.y;
        mousePosition = mainCamera.ScreenToWorldPoint(mousePosition);
        
        GetGridIndex(mousePosition, out var xIndex, out var zIndex);
        //resets previous selected node
        if(hoveredNode != null)
            hoveredNode.isBeingHovered = false;
        //change state of which node is hovered by mouse position.
        hoveredNode = grid[xIndex][zIndex];
        hoveredNode.isBeingHovered = true;
        
        if (Input.GetMouseButtonDown(0)) {
            selectedNode = grid[xIndex][zIndex];
            selectedNodeGO = gridGO[xIndex][zIndex];
        }
        
        if (Input.GetMouseButton(0)) {
            mouseClickPosition = mousePosition;
            NodeFollowMouse(true, mousePosition);
            //when entering another node grid tile, swap node tile position.
            //selectedNode keeps original position, selectedNodeGO is the only one moving while being selected.
            if (selectedNode.position != grid[xIndex][zIndex].position) {
                var selectedPositionBuffer = selectedNode.position;
                var aux = selectedNode.position;
                selectedNode.position = grid[xIndex][zIndex].position;
                grid[xIndex][zIndex].position = aux;
                gridGO[xIndex][zIndex].transform.position = aux;
                //swap game objects
                SwapGridNodes(xIndex, zIndex, selectedPositionBuffer);
            }
        }

        //sends selectedNodeGO to default tile position.
        if (Input.GetMouseButtonUp(0)) {
            //NodeFollowMouse(false);
            selectedNodeGO.transform.position = selectedNode.position;
            selectedNode = null;
            selectedNodeGO = null;
        }
        
        if (gridGO == null) return;
        //restores previous hovered node height
        if (hoveredNodeGO != null) {
            var normalPosition = hoveredNodeGO.transform.position;
            normalPosition.y = normalNodeHeight;
            hoveredNodeGO.transform.position = normalPosition;
        }
        //ups the height of hovered node.
        hoveredNodeGO = gridGO[xIndex][zIndex];
        var selectedPosition = hoveredNodeGO.transform.position;
        selectedPosition.y = selectedNodeHeight;
        hoveredNodeGO.transform.position = selectedPosition;
    }

    private void SwapGridNodes(int xIndex, int zIndex, Vector3 selectedNodePosition) {
        GetGridIndex(selectedNodePosition, out int selectedXIndex, out int selectedZIndex);
        var auxNode = grid[xIndex][zIndex];
        var auxNodeGO = gridGO[xIndex][zIndex];
        grid[xIndex][zIndex] = grid[selectedXIndex][selectedZIndex];
        gridGO[xIndex][zIndex] = gridGO[selectedXIndex][selectedZIndex];
        grid[selectedXIndex][selectedZIndex] = auxNode;
        gridGO[selectedXIndex][selectedZIndex] = auxNodeGO;
    }

    private void GetGridIndex(Vector3 nodePosition, out int xIndex, out int zIndex) {
        float xPercentage = (nodePosition.x + gridSize.value.x / 2) / gridSize.value.x;
        float zPercentage = (nodePosition.z + gridSize.value.z / 2) / gridSize.value.z;
        xIndex = Mathf.FloorToInt(Mathf.Clamp(xNodes * xPercentage, 0, xNodes - 1));
        zIndex = Mathf.FloorToInt(Mathf.Clamp(zNodes * zPercentage, 0, zNodes - 1));
    }

    private void NodeFollowMouse(bool b, Vector3 mousePosition = default) {
        if (b) selectedNodeGO.transform.position = mousePosition;
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
                if(node.isBeingHovered)
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
    public LinkedList<LinkedList<GameObject>> LookForMatrixHits() {
        LinkedList<LinkedList<GameObject>> listOfHits = new LinkedList<LinkedList<GameObject>>();
        LookForVerticalHits(listOfHits);
        LookForHorizontalHits(listOfHits);

        return listOfHits;
    }

    private void LookForHorizontalHits(LinkedList<LinkedList<GameObject>> listOfHits) {
        for (int i = 0; i < grid[0].Length; i++) {
            var currentType = grid[0][i].nodeType;
            int countInARow = 0;
            for (int j = 0; j < grid.Length; j++) {
                if (currentType == grid[j][i].nodeType) countInARow++;
                else {
                    if (countInARow >= 3) {
                        listOfHits.AddFirst(new LinkedList<GameObject>());
                        for (; countInARow > 0; countInARow--) {
                            listOfHits.First.Value.AddLast(gridGO[j - countInARow][i]);
                        }
                    }

                    countInARow = 1;
                    currentType = grid[j][i].nodeType;
                }
            }
            
            if (countInARow < 3) continue;
            listOfHits.AddFirst(new LinkedList<GameObject>());
            for (; countInARow > 0; countInARow--) {
                listOfHits.First.Value.AddLast(gridGO[grid.Length - countInARow][i]);
            }
        }
    }

    private void LookForVerticalHits(LinkedList<LinkedList<GameObject>> listOfHits) {
        for (int i = 0; i < grid.Length; i++) {
            var currentType = grid[i][0].nodeType;
            int countInARow = 0;
            for (int j = 0; j < grid[i].Length; j++) {
                if (currentType == grid[i][j].nodeType) countInARow++;
                else {
                    if (countInARow >= 3) {
                        listOfHits.AddFirst(new LinkedList<GameObject>());
                        for (; countInARow > 0; countInARow--) {
                            listOfHits.First.Value.AddLast(gridGO[i][j - countInARow]);
                        }
                    }

                    countInARow = 1;
                    currentType = grid[i][j].nodeType;
                }
            }

            if (countInARow < 3) continue;
            listOfHits.AddFirst(new LinkedList<GameObject>());
            for (; countInARow > 0; countInARow--) {
                listOfHits.First.Value.AddLast(gridGO[i][gridGO[i].Length - countInARow]);
            }
        }
    }
}
