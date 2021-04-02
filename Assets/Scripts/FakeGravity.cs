using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeGravity : MonoBehaviour {
    public Rigidbody rb;
    public Collider collider;
    public FloatValue nodeFallVelocity;
    private Vector3 desiredPosition;
    private bool gravityOn = false;
    private void Start() {
        EventManager.onNewNodesSpawned += GravityOn;
        gravityOn = false;
        rb.velocity = Vector3.zero;
        collider.enabled = false;
    }

    private void OnEnable() {
        EventManager.onNewNodesSpawned += GravityOn;
        gravityOn = false;
        rb.velocity = Vector3.zero;
        collider.enabled = false;
    }

    private void OnDisable() {
        EventManager.onNewNodesSpawned -= GravityOn;
        gravityOn = false;
        rb.velocity = Vector3.zero;
        collider.enabled = false;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.S))
            GravityOn();
        if (Input.GetKeyDown(KeyCode.D))
            GravityOff();
        if (gravityOn && transform.position.z <= desiredPosition.z)
            GravityOff();
    }
    private void FixedUpdate() {
        if (gravityOn) {
            rb.velocity = Vector3.back * nodeFallVelocity.value;
        }
    }

    public void SetDesiredPosition (Vector3 desiredPosition) {
        this.desiredPosition = desiredPosition;
    }

    private void GravityOn() {
        if (transform.position == desiredPosition) return;
        collider.enabled = true;
        gravityOn = true;
    }

    private void GravityOff() {
        gravityOn = false;
        rb.velocity = Vector3.zero;
        collider.enabled = false;
        transform.position = desiredPosition;
    }
}
