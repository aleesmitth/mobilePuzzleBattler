using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowerMapBarrier : MonoBehaviour {
    public Vector3Value gridSize;
    public GameObject prefab;
    public MeshRenderer mr;
    
    private void OnEnable() {
        SpawnLowerBarrier();
    }

    private void Start() {
        SpawnLowerBarrier();
    }

    private void SpawnLowerBarrier() {
        GameObject barrier = Instantiate(prefab);
        var position = transform.position;
        var barrierPosition = position;
        var bounds = mr.bounds;
        barrierPosition.z -= (gridSize.value.z + bounds.size.z) / 2;
        barrier.transform.position = barrierPosition;

        var scale = barrier.transform.localScale;
        scale.x = gridSize.value.x / bounds.size.x;
        barrier.transform.localScale = scale;
    }
}
