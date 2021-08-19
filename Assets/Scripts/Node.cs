using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {
    public Vector3 position;
    public bool isBeingHovered;
    public ElementType elementType;
    public bool hasBeenHit;

    public Node(Vector3 position) {
        elementType = GetRandomType();
        this.position = position;
        isBeingHovered = false;
        hasBeenHit = false;
    }

    public void ResetNodeType() {
        elementType = GetRandomType();
        //nodeType = (Random.value > .5) ? NodeType.Cube : NodeType.Sphere;
    }

    private ElementType GetRandomType() {
        var randomValue = Random.value;
        if (randomValue <= .25) return ElementType.FIRE;
        if (randomValue > .25 && randomValue <= .5) return ElementType.WATER;
        if (randomValue > .5 && randomValue <= .75) return ElementType.EARTH;
        return ElementType.AIR;
    }
}

public enum ElementType {
    FIRE,
    WATER,
    EARTH,
    AIR
}
