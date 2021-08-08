using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using Random = UnityEngine.Random;

public class NodeGrid : MonoBehaviour {
    public Vector3Value gridSize;
    public Vector3Value nodeSize;
    public int newNodesOffset;
    private Node[][] grid;
    private Vector3[][] positionsGrid;
    private int xNodes;
    private int yNodes;
    private Node hoveredNode;
    private Camera mainCamera;
    private GameObject[][] gridGO; //the first [] are the col, second [] the rows. bottom left is 0,0
    private GameObject selectedNodeGO;
    private Node selectedNode;

    private void OnEnable() {
        mainCamera = Camera.main;
        CreateGrid();
        EventManager.onHitsProcessed += SpawnMissingNodes;
    }

    private void OnDisable() {
        EventManager.onHitsProcessed -= SpawnMissingNodes;
    }

    private void SpawnMissingNodes(int[] hitsPerColumn) {

        SinkRemainingNodes();
        
        for (int i = 0; i < hitsPerColumn.Length; i++) {
            for (int j = 0; j < hitsPerColumn[i]; j++) {
                var node = new Node(new Vector3(i * nodeSize.value.x + nodeSize.value.x/2,
                    newNodesOffset + (j + grid[i].Length) * nodeSize.value.y + nodeSize.value.y/2 + j,
                    0) - gridSize.value / 2);
                GameObject nodeGO = default;
                nodeGO = PoolManager.Get(node.nodeType);
                nodeGO.GetComponent<MaterialController>().MakeInvisible();
                nodeGO.transform.position = node.position;
                //nodeGO.transform.localScale = new Vector3(nodeSize.value.x / 2, 1, nodeSize.value.z / 2);
                node.position = positionsGrid[i][grid[i].Length - hitsPerColumn[i] + j];
                grid[i][grid[i].Length - hitsPerColumn[i] + j] = node;
                gridGO[i][grid[i].Length - hitsPerColumn[i] + j] = nodeGO;
                nodeGO.GetComponent<FakeGravity>().SetDesiredPosition(node.position);
            }
        }
        //EventManager.OnNewNodesSpawned();
    }
    
    /// <summary>
    /// sink nodes to bottom of their column if there's space. 
    /// </summary>
    private void SinkRemainingNodes() {
        for (int i = 0; i < grid.Length; i++) {
            int newPosition = 0;
            for (int j = 0; j < grid[i].Length; j++) {
                if (grid[i][j].hasBeenHit) continue;
                grid[i][newPosition] = grid[i][j];
                grid[i][newPosition].position = positionsGrid[i][newPosition];
                gridGO[i][newPosition] = gridGO[i][j];
                gridGO[i][newPosition].GetComponent<FakeGravity>().SetDesiredPosition(positionsGrid[i][newPosition]);
                newPosition++;
            }
        }
    }

    public void DestroyHitNodes(LinkedList<LinkedList<GameObject>> hits) {
        //TODO placeholder, wasting memory should pool destroyed nodes.
        foreach (var listOfHits in hits) {
            foreach (var hit in listOfHits) {
                Destroy(hit);
            }
        }
    }

    private void Update() {
        if (grid == null || gridGO == null) return;
        var mousePosition = Input.mousePosition;
        mousePosition.z = - mainCamera.transform.position.z;
        mousePosition = mainCamera.ScreenToWorldPoint(mousePosition);
        
        GetGridIndex(mousePosition, out var xIndex, out var yIndex);
        //resets previous selected node
        if(hoveredNode != null)
            hoveredNode.isBeingHovered = false;
        //change state of which node is hovered by mouse position.
        hoveredNode = grid[xIndex][yIndex];
        hoveredNode.isBeingHovered = true;
        
        if (Input.GetMouseButtonDown(0)) {
            selectedNode = grid[xIndex][yIndex];
            selectedNodeGO = gridGO[xIndex][yIndex];
        }
        
        if (Input.GetMouseButton(0)) {
            NodeFollowMouse(true, mousePosition);
            //when entering another node grid tile, swap node tile position.
            //selectedNode keeps original position, selectedNodeGO is the only one moving while being selected.
            if (selectedNode.position != grid[xIndex][yIndex].position) {
                var selectedPositionBuffer = selectedNode.position;
                var aux = selectedNode.position;
                selectedNode.position = grid[xIndex][yIndex].position;
                grid[xIndex][yIndex].position = aux;
                gridGO[xIndex][yIndex].transform.position = aux;
                //swap game objects
                SwapGridNodes(xIndex, yIndex, selectedPositionBuffer);
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
    }

    private void SwapGridNodes(int xIndex, int yIndex, Vector3 selectedNodePosition) {
        GetGridIndex(selectedNodePosition, out int selectedXIndex, out int selectedZIndex);
        var auxNode = grid[xIndex][yIndex];
        var auxNodeGO = gridGO[xIndex][yIndex];
        grid[xIndex][yIndex] = grid[selectedXIndex][selectedZIndex];
        gridGO[xIndex][yIndex] = gridGO[selectedXIndex][selectedZIndex];
        grid[selectedXIndex][selectedZIndex] = auxNode;
        gridGO[selectedXIndex][selectedZIndex] = auxNodeGO;
    }

    private void GetGridIndex(Vector3 position, out int xIndex, out int yIndex) {
        float xPercentage = (position.x + gridSize.value.x / 2) / gridSize.value.x;
        float yPercentage = (position.y + gridSize.value.y / 2) / gridSize.value.y;
        xIndex = Mathf.FloorToInt(Mathf.Clamp(xNodes * xPercentage, 0, xNodes - 1));
        yIndex = Mathf.FloorToInt(Mathf.Clamp(yNodes * yPercentage, 0, yNodes - 1));
    }

    private void NodeFollowMouse(bool b, Vector3 mousePosition = default) {
        if (b) selectedNodeGO.transform.position = mousePosition;
    }

    private void CreateGrid() {
        //checking how many nodes fit in each axis
        xNodes = Mathf.FloorToInt(gridSize.value.x / nodeSize.value.x);
        yNodes = Mathf.FloorToInt(gridSize.value.y / nodeSize.value.y);
        //creating matrices to store nodes and positions, storing positiosn so i don't have to access the transform
        grid = new Node[xNodes][];
        positionsGrid = new Vector3[xNodes][];
        for (int i = 0; i < grid.Length; i++) {
            grid[i] = new Node[yNodes];
            positionsGrid[i] = new Vector3[yNodes];
            //positioning nodes next to each other
            for (int j = 0; j < grid[i].Length; j++) {
                var nodePosition = new Vector3(
                                       i * nodeSize.value.x + nodeSize.value.x / 2,
                                       j * nodeSize.value.y + nodeSize.value.y / 2, 0) - gridSize.value / 2;
                //storeing new node, nodePosition is not needed for creating the node, may delete later.
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
                if(node.isBeingHovered)
                    Gizmos.color = Color.yellow;
                Gizmos.DrawWireCube(node.position, nodeSize.value);
            }
        }
    }

    public void ResetGrid() {
        //first time populating the grid
        if (gridGO == null) {
            gridGO = new GameObject[xNodes][];
            for (int i = 0; i < gridGO.Length; i++) {
                gridGO[i] = new GameObject[yNodes];
            }
        }
        //destroying the populated grid
        else {
            for (int i = 0; i < gridGO.Length; i++) {
                for (int j = 0; j < gridGO[i].Length; j++) {
                    PoolManager.Destroy(grid[i][j].nodeType, gridGO[i][j]);
                }
            }
        }
        //instatiating prefabs and moving them to right position
        for (int i = 0; i < gridGO.Length; i++) {
            for (int j = 0; j < gridGO[i].Length; j++) {
                grid[i][j].ResetNodeType();
                gridGO[i][j] = PoolManager.Get(grid[i][j].nodeType);
                gridGO[i][j].GetComponent<MaterialController>().MakeVisible();
                gridGO[i][j].transform.position = grid[i][j].position;
            }
        }
    }
    
    public LinkedList<LinkedList<GameObject>> LookForMatrixHits() {
        LinkedList<LinkedList<GameObject>> listOfHits = new LinkedList<LinkedList<GameObject>>();
        int[] hitsPerColumn = new int[grid.Length];

        LinkedList<LinkedList<NodeType>> listsOfElementsHit = new LinkedList<LinkedList<NodeType>>();
        var listsOfColumnElementsHit = LookForVerticalHits(listOfHits, hitsPerColumn);
        var listsOfRowElementsHit = LookForHorizontalHits(listOfHits, hitsPerColumn);

        foreach (var list in listsOfColumnElementsHit) {
            listsOfElementsHit.AddLast(list);
        }
        
        foreach (var list in listsOfRowElementsHit) {
            listsOfElementsHit.AddLast(list);
        }
        
        EventManager.OnNodesDestroyed(listsOfElementsHit);

        EventManager.OnHitsProcessed(hitsPerColumn);
        
        return listOfHits;
    }

    private LinkedList<LinkedList<NodeType>> LookForHorizontalHits(LinkedList<LinkedList<GameObject>> listOfHits, int[] hitsPerColumn) {
        var listsOfElementsHit = new LinkedList<LinkedList<NodeType>>();
        for (int i = 0; i < grid[0].Length; i++) {
            var currentType = grid[0][i].nodeType;
            int countInARow = 0;
            for (int j = 0; j < grid.Length; j++) {
                if (currentType == grid[j][i].nodeType) countInARow++;
                else {
                    if (countInARow >= 3) {
                        listsOfElementsHit.AddFirst(new LinkedList<NodeType>());
                        listOfHits.AddFirst(new LinkedList<GameObject>());
                        for (; countInARow > 0; countInARow--) {
                            bool alreadyInList = false;
                            foreach (var list in listOfHits.Where(list => list.Contains(gridGO[j - countInARow][i]))) {
                                alreadyInList = true;
                            }
                            
                            if (alreadyInList) continue;
                            hitsPerColumn[j - countInARow]++;
                            listsOfElementsHit.First.Value.AddLast(currentType);
                            listOfHits.First.Value.AddLast(gridGO[j - countInARow][i]);
                            //TODO placeholder, there's still no way to calculate points nor combos.
                            grid[j - countInARow][i].hasBeenHit = true;
                        }
                    }

                    countInARow = 1;
                    currentType = grid[j][i].nodeType;
                }
            }
            
            if (countInARow < 3) continue;
            listsOfElementsHit.AddFirst(new LinkedList<NodeType>());
            listOfHits.AddFirst(new LinkedList<GameObject>());
            for (; countInARow > 0; countInARow--) {
                bool alreadyInList = false;
                foreach (var list in listOfHits.Where(list => list.Contains(gridGO[grid.Length - countInARow][i]))) {
                    alreadyInList = true;
                }
                            
                if (alreadyInList) continue;
                hitsPerColumn[grid.Length - countInARow]++;
                listsOfElementsHit.First.Value.AddLast(currentType);
                listOfHits.First.Value.AddLast(gridGO[grid.Length - countInARow][i]);
                //TODO placeholder, there's still no way to calculate points nor combos.
                grid[grid.Length - countInARow][i].hasBeenHit = true;
            }
        }

        return listsOfElementsHit;
    }

    private LinkedList<LinkedList<NodeType>> LookForVerticalHits(LinkedList<LinkedList<GameObject>> listOfHits, int[] hitsPerColumn) {
        var listsOfElementsHit = new LinkedList<LinkedList<NodeType>>();
        for (int i = 0; i < grid.Length; i++) {
            var currentType = grid[i][0].nodeType;
            int countInARow = 0;
            for (int j = 0; j < grid[i].Length; j++) {
                if (currentType == grid[i][j].nodeType) countInARow++;
                else {
                    if (countInARow >= 3) {
                        listsOfElementsHit.AddFirst(new LinkedList<NodeType>());
                        listOfHits.AddFirst(new LinkedList<GameObject>());
                        for (; countInARow > 0; countInARow--) {
                            bool alreadyInList = false;
                            foreach (var list in listOfHits.Where(list => list.Contains(gridGO[i][j - countInARow]))) {
                                alreadyInList = true;
                            }
                            if (alreadyInList) continue;
                            hitsPerColumn[i]++;
                            listsOfElementsHit.First.Value.AddLast(currentType);
                            listOfHits.First.Value.AddLast(gridGO[i][j - countInARow]);
                            //TODO placeholder, there's still no way to calculate points nor combos.
                            grid[i][j - countInARow].hasBeenHit = true;
                        }
                    }

                    countInARow = 1;
                    currentType = grid[i][j].nodeType;
                }
            }

            if (countInARow < 3) continue;
            listsOfElementsHit.AddFirst(new LinkedList<NodeType>());
            listOfHits.AddFirst(new LinkedList<GameObject>());
            for (; countInARow > 0; countInARow--) {
                bool alreadyInList = false;
                foreach (var list in listOfHits.Where(list => list.Contains(gridGO[i][gridGO[i].Length - countInARow]))) {
                    alreadyInList = true;
                }
                if (alreadyInList) continue;
                hitsPerColumn[i]++;
                listsOfElementsHit.First.Value.AddLast(currentType);
                listOfHits.First.Value.AddLast(gridGO[i][gridGO[i].Length - countInARow]);
                //TODO placeholder, there's still no way to calculate points nor combos.
                grid[i][gridGO[i].Length - countInARow].hasBeenHit = true;
            }
        }

        return listsOfElementsHit;
    }

    public LinkedList<LinkedList<GameObject>> GetMatrixHits() {
        LinkedList<LinkedList<GameObject>> listOfHits = new LinkedList<LinkedList<GameObject>>();
        int[] hitsPerColumn = new int[grid.Length];
        LookForVerticalHits(listOfHits, hitsPerColumn);
        LookForHorizontalHits(listOfHits, hitsPerColumn);

        for (int i = 0; i < hitsPerColumn.Length; i++) {
            print("In column: " + i + " there was " + hitsPerColumn[i] + " hits");
        }
        
        return listOfHits;
    }
}