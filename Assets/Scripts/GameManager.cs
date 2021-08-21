using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if(instance!=this)
            Destroy(gameObject);
    }

    private void OnEnable() {
        EventManager.onCollisionWithEnemy += LoadFightScene;
    }

    private void LoadFightScene() {
        //placeholder to load scene
        SceneManager.LoadScene("Scenes/Fight", LoadSceneMode.Single);
    }
}
