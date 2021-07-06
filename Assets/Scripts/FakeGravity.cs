using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeGravity : MonoBehaviour {
    public FloatValue nodeFallVelocity;
    public Rigidbody2D rb;
    private Vector3 desiredPosition;
    private bool gravityOn = false;
    private void Start() {
        EventManager.onNewNodesSpawned += GravityOn;
        gravityOn = false;
    }

    private void OnEnable() {
        EventManager.onNewNodesSpawned += GravityOn;
        gravityOn = false;
    }

    private void OnDisable() {
        EventManager.onNewNodesSpawned -= GravityOn;
        gravityOn = false;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.D))
            GravityOn();
        if (Input.GetKeyDown(KeyCode.F))
            GravityOff();
        if (gravityOn && transform.position.y <= desiredPosition.y)
            GravityOff();
    }
    private void FixedUpdate() {
        if (!gravityOn) return;
        rb.velocity = Vector2.down * nodeFallVelocity.value;
        /*
        var transformBuffer = transform;
        var currentPosition = transformBuffer.position;
        currentPosition.y -= nodeFallVelocity.value;
        transformBuffer.position = currentPosition;*/
    }

    public void SetDesiredPosition (Vector3 desiredPosition) {
        this.desiredPosition = desiredPosition;
    }

    private void GravityOn() {
        if (transform.position == desiredPosition) return;
        gravityOn = true;
    }

    private void GravityOff() {
        gravityOn = false;
        transform.position = desiredPosition;
        rb.velocity = Vector2.zero;
    }
}
