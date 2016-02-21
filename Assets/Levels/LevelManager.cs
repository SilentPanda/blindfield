using UnityEngine;

public class LevelManager : IndestructibleBehaviour {
    public int levelIndex = 1;

    void Start() {
        LoadLevel();
    }

    void OnLevelWasLoaded(int loadedLevelIndex) {
        levelIndex++;
    }

    private void LoadLevel() {
        Application.LoadLevel(levelIndex);
    }
}
