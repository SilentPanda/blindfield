using UnityEngine;

public class LevelManager : IndestructibleBehaviour {
    private int sceneIndex = 0;

    void Start() {
        LoadNextScene();
    }

    private void LoadNextScene() {
        sceneIndex++;

        Application.LoadLevel(sceneIndex);
    }
}
