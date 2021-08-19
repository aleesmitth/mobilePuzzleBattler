using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapInputListener : MonoBehaviour {
    private void Update() {
        if (Input.GetKeyDown(KeyCode.I)) {
            SceneManager.LoadScene("Scenes/Inventory", LoadSceneMode.Single);
        }
    }
}
