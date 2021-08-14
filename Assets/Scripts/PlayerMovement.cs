using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour {
    public Tilemap[] groundTilemap;
    public Tilemap collisionTilemap;
    public Camera mainCamera;
    public float speed;
    private Vector3 moveTowards;
    private Movement movement;

    private void OnEnable() {
        movement = new Movement(groundTilemap, collisionTilemap);
    }

    void Update() {
        
        Move();
        
        if (!Input.GetMouseButton(0)) return;
        UpdateDestination();
    }

    private void UpdateDestination() {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = -mainCamera.transform.position.z;
        moveTowards = mainCamera.ScreenToWorldPoint(mousePosition);
    }

    private void Move() {
        transform.position = movement.Move(transform.position, moveTowards, speed);
    }
}