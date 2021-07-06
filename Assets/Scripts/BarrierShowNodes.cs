using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierShowNodes : MonoBehaviour {
    public Vector3Value gridSize;
    public GameObject prefab;
    public SpriteRenderer sr;
    
    private void OnEnable() {
        SpawnLowerBarrier();
    }

    private void Start() {
        //SpawnLowerBarrier();
    }

    private void SpawnLowerBarrier() {
        GameObject barrier = Instantiate(prefab);
        var position = transform.position;
        var barrierPosition = position;
        var size = sr.bounds.extents * 2;
        barrierPosition.y = (gridSize.value.y + size.y) / 2;
        barrierPosition.z = 0;
        barrier.transform.position = barrierPosition;

        var scale = barrier.transform.localScale;
        scale.x = gridSize.value.x / size.x; 
        barrier.transform.localScale = scale;
    }
}
