using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : IndestructibleBehaviour {
    public int levelIndex = 1;
    public int maxLevelIndex = 9;

    private BaseRuleSet currentRuleSet;

    void Start() {
        LoadLevel();
    }

    public void LoadLevel() {
        if (levelIndex > maxLevelIndex) {
            Debug.Log("The End!");

            return;
        }

        Debug.Log(String.Format("Loading level {0}", levelIndex));

        SceneManager.LoadScene(levelIndex);

        levelIndex++;
    }
}
