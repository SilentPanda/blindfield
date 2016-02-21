using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : IndestructibleBehaviour {
    public int levelIndex = 1;

    private BaseRuleSet currentRuleSet;

    void Start() {
        LoadLevel();
    }

    public void LoadLevel() {
        if (levelIndex > SceneManager.sceneCount) {
            Debug.Log("The End!");

            return;
        }

        Debug.Log(String.Format("Loading level {0}", levelIndex));

        SceneManager.LoadScene(levelIndex);

        levelIndex++;
    }
}
