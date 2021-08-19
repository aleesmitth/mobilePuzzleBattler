using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryInputListener : MonoBehaviour {
    public Camera mainCamera;
    public InventorGrid2D inventoryGrid;

    void Update() {

        if (Input.GetKeyDown(KeyCode.Space)) {
            inventoryGrid.ResetGrid();
        }
        
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene("Scenes/Map", LoadSceneMode.Single);
        }
        
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