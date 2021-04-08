using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {
    public Vector3 position;
    public bool isBeingHovered;
    public NodeType nodeType;
    public bool hasBeenHit;

    public Node(Vector3 position) {
        nodeType = GetRandomType();
        this.position = position;
        isBeingHovered = false;
        hasBeenHit = false;
    }

    public void ResetNodeType() {
        nodeType = GetRandomType();
        //nodeType = (Random.value > .5) ? NodeType.Cube : NodeType.Sphere;
    }

    private NodeType GetRandomType() {
        var randomValue = Random.value;
        if (randomValue <= .25) return NodeType.Cube;
        if (randomValue > .25 && randomValue <= .5) return NodeType.Sphere;
        if (randomValue > .5 && randomValue <= .75) return NodeType.Cylinder;
        return NodeType.Capsule;
    }
}

public enum NodeType {
    Cube,
    Sphere,
    Cylinder,
    Capsule
}
