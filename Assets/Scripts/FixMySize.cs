using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixMySize : MonoBehaviour {
    public Vector3Value nodeSize;
    public SpriteRenderer sr;
    public FloatValue nodeMargin;

    private void Start() {
        FixSize();
    }
    
    /*i make the sprite smaller, if i run this again i go back to the original size.*/
    private void FixSize() {
        var transform1 = transform;
        var scale = transform1.localScale;
        var sizeOfSprite = sr.bounds.extents * 2; 
        scale.x = nodeMargin.value * nodeSize.value.x / sizeOfSprite.x;
        scale.y = nodeMargin.value * nodeSize.value.y / sizeOfSprite.y;
        transform1.localScale = scale;
    }
}
