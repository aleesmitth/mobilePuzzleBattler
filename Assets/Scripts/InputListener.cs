using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class InputListener : MonoBehaviour {
    public Camera mainCamera;

    void Update() {
        if (!Input.GetMouseButtonDown(0)) return;

        //Vector3 worldPosition = MousePositionToWorld();
        
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 150, Color.red, 2f);
        
        int layerMask = LayerMask.GetMask("Interactable");
        var interactableHit = Physics2D.GetRayIntersection(ray, Mathf.Infinity, layerMask);
        
        if (!interactableHit) return;
        interactableHit.transform.GetComponent<IInteraction>().Interact();

    }

    /*private Vector3 MousePositionToWorld() {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = -mainCamera.transform.position.z;
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
        return worldPosition;
    }*/
}