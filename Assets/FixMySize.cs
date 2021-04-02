using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixMySize : MonoBehaviour {
    public MeshRenderer mr;
    public Vector3Value nodeSize;

    private void Start() {
        FixSize();
    }

    private void OnEnable() {
        FixSize();
    }

    private void FixSize() {
        var transform1 = transform;
        var scale = transform1.localScale;
        var bounds = mr.bounds;
        scale.x = nodeSize.value.x;
        scale.z = nodeSize.value.z;
        transform1.localScale = scale;
    }
}
