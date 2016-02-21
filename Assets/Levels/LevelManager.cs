using UnityEngine;

public class LevelManager : IndestructibleBehaviour {
    public int sceneIndex = 1;

    void Start() {
        LoadScene();
    }

    private void LoadScene() {
        Application.LoadLevel(sceneIndex);

        sceneIndex++;
    }
}
