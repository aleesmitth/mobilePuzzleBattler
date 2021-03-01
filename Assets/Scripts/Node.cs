using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {
    public Vector3 position;
    public bool isSelected;
    public NodeType nodeType;

    public Node(Vector3 position) {
        this.position = position;
        isSelected = false;
    }
}

public enum NodeType {
    Cube,
    Sphere
}
