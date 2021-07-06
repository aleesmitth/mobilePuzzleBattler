using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowNodesCollider : MonoBehaviour {

    /*private void OnTriggerStay2D(Collider2D other) {
        if (!other.gameObject.CompareTag("Node")) return;
        other.gameObject.GetComponent<MaterialController>().MakeVisible();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!other.gameObject.CompareTag("Node")) return;
        other.gameObject.GetComponent<MaterialController>().MakeVisible();
    }*/

    private void OnTriggerExit2D(Collider2D other) {
        if (!other.gameObject.CompareTag("Node")) return;
        other.gameObject.GetComponent<MaterialController>().MakeVisible();
    }
}
