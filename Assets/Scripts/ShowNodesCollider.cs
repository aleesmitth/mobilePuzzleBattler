using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowNodesCollider : MonoBehaviour {
    private void OnTriggerEnter(Collider other) {
        if (!other.gameObject.CompareTag("Node")) return;
        other.gameObject.GetComponent<MaterialController>().MakeVisible();
    }
}
