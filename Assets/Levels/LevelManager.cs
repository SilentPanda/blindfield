using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    private int sceneIndex = 0;

    void Awake() {
        DontDestroyOnLoad(gameObject);
    }

    void Start() {
        LoadNextScene();
    }

    private void LoadNextScene() {
        sceneIndex++;

        Application.LoadLevel(sceneIndex);
    }
}
