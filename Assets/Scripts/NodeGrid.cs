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
    public float normalNodeHeight;
    public float selectedNodeHeight;
    private Node[][] grid;
    private Vector3[][] positionsGrid;
    private int xNodes;
    private int zNodes;
    private Node hoveredNode;
    private GameObject hoveredNodeGO;
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
                var node = new Node(new Vector3
                    (i * nodeSize.value.x + nodeSize.value.x/2, 0, (j + grid[i].Length) * nodeSize.value.z + nodeSize.value.z/2) - gridSize.value / 2);
                GameObject nodeGO = default;
                switch (node.nodeType) {
                    case NodeType.Cube: nodeGO = CubePool.instance.Get();
                        break;
                    case NodeType.Sphere: nodeGO = SpherePool.instance.Get();
                        break;
                    default:
                        break;
                }

                if (nodeGO is null) continue;
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
        if (hoveredNodeGO == null) return;
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
        positionsGrid = new Vector3[xNodes][];
        for (int i = 0; i < grid.Length; i++) {
            grid[i] = new Node[zNodes];
            positionsGrid[i] = new Vector3[zNodes];
            for (int j = 0; j < grid[i].Length; j++) {
                var nodePosition = new Vector3
                                   (i * nodeSize.value.x + nodeSize.value.x / 2, 0,
                                       j * nodeSize.value.z + nodeSize.value.z / 2) -
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
                if(node.isBeingHovered)
                    Gizmos.color = Color.yellow;
                Gizmos.DrawWireCube(node.position, nodeSize.value);
            }
        }
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
                grid[i][j].ResetNodeType();
                switch (grid[i][j].nodeType) {
                    case NodeType.Cube: gridGO[i][j] = CubePool.instance.Get();
                        break;
                    case NodeType.Sphere: gridGO[i][j] = SpherePool.instance.Get();
                        break;
                    default:
                        break;
                }
                gridGO[i][j].transform.position = grid[i][j].position;
                gridGO[i][j].GetComponent<FakeGravity>().SetDesiredPosition(grid[i][j].position);
            }
        }
    }
    public LinkedList<LinkedList<GameObject>> LookForMatrixHits() {
        LinkedList<LinkedList<GameObject>> listOfHits = new LinkedList<LinkedList<GameObject>>();
        int[] hitsPerColumn = new int[grid.Length];
        LookForVerticalHits(listOfHits, hitsPerColumn);
        LookForHorizontalHits(listOfHits, hitsPerColumn);

        for (int i = 0; i < hitsPerColumn.Length; i++) {
            print("In column: " + i + " there was " + hitsPerColumn[i] + " hits");
        }

        EventManager.OnHitsProcessed(hitsPerColumn);
        
        return listOfHits;
    }

    private void LookForHorizontalHits(LinkedList<LinkedList<GameObject>> listOfHits, int[] hitsPerColumn) {
        for (int i = 0; i < grid[0].Length; i++) {
            var currentType = grid[0][i].nodeType;
            int countInARow = 0;
            for (int j = 0; j < grid.Length; j++) {
                if (currentType == grid[j][i].nodeType) countInARow++;
                else {
                    if (countInARow >= 3) {
                        listOfHits.AddFirst(new LinkedList<GameObject>());
                        for (; countInARow > 0; countInARow--) {
                            bool alreadyInList = false;
                            foreach (var list in listOfHits.Where(list => list.Contains(gridGO[j - countInARow][i]))) {
                                alreadyInList = true;
                            }
                            
                            if (alreadyInList) continue;
                            hitsPerColumn[j - countInARow]++;
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
            listOfHits.AddFirst(new LinkedList<GameObject>());
            for (; countInARow > 0; countInARow--) {
                bool alreadyInList = false;
                foreach (var list in listOfHits.Where(list => list.Contains(gridGO[grid.Length - countInARow][i]))) {
                    alreadyInList = true;
                }
                            
                if (alreadyInList) continue;
                hitsPerColumn[grid.Length - countInARow]++;
                listOfHits.First.Value.AddLast(gridGO[grid.Length - countInARow][i]);
                //TODO placeholder, there's still no way to calculate points nor combos.
                grid[grid.Length - countInARow][i].hasBeenHit = true;
            }
        }
    }

    private void LookForVerticalHits(LinkedList<LinkedList<GameObject>> listOfHits, int[] hitsPerColumn) {
        for (int i = 0; i < grid.Length; i++) {
            var currentType = grid[i][0].nodeType;
            int countInARow = 0;
            for (int j = 0; j < grid[i].Length; j++) {
                if (currentType == grid[i][j].nodeType) countInARow++;
                else {
                    if (countInARow >= 3) {
                        listOfHits.AddFirst(new LinkedList<GameObject>());
                        for (; countInARow > 0; countInARow--) {
                            bool alreadyInList = false;
                            foreach (var list in listOfHits.Where(list => list.Contains(gridGO[i][j - countInARow]))) {
                                alreadyInList = true;
                            }
                            if (alreadyInList) continue;
                            hitsPerColumn[i]++;
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
            listOfHits.AddFirst(new LinkedList<GameObject>());
            for (; countInARow > 0; countInARow--) {
                bool alreadyInList = false;
                foreach (var list in listOfHits.Where(list => list.Contains(gridGO[i][gridGO[i].Length - countInARow]))) {
                    alreadyInList = true;
                }
                if (alreadyInList) continue;
                hitsPerColumn[i]++;
                listOfHits.First.Value.AddLast(gridGO[i][gridGO[i].Length - countInARow]);
                //TODO placeholder, there's still no way to calculate points nor combos.
                grid[i][gridGO[i].Length - countInARow].hasBeenHit = true;
            }
        }
    }
}
