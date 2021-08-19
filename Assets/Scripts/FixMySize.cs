using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixMySize : MonoBehaviour {
    public Vector3Value nodeSize;
    public SpriteRenderer sr;
    public FloatValue nodeScale;

    private void Start() {
        FixSize();
    }
    
    /*i make the sprite smaller, if i run this again i go back to the original size.*/
    public void FixSize() {
        var transform1 = transform;
        var scale = transform1.localScale;
        var sizeOfSprite = sr.bounds.extents * 2; 
        scale.x = nodeScale.value * nodeSize.value.x / sizeOfSprite.x;
        scale.y = nodeScale.value * nodeSize.value.y / sizeOfSprite.y;
        transform1.localScale = scale;
    }
}
