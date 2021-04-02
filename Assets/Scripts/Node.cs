using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {
    public Vector3 position;
    public bool isBeingHovered;
    public NodeType nodeType;
    public bool hasBeenHit;

    public Node(Vector3 position) {
        nodeType = (Random.value > .5) ? NodeType.Cube : NodeType.Sphere;
        this.position = position;
        isBeingHovered = false;
        hasBeenHit = false;
    }

    public void ResetNodeType() {
        nodeType = (Random.value > .5) ? NodeType.Cube : NodeType.Sphere;
    }
}

public enum NodeType {
    Cube,
    Sphere
}
