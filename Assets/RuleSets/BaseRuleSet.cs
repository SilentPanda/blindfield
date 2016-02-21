using UnityEngine;

public class BaseRuleSet : MonoBehaviour {
    private bool completed = false;

    protected void OnCompleted() {
        if (completed) {
            return;
        }

        completed = true;

        LevelManager[] levelManager = GameObject.FindObjectsOfType(
            typeof(LevelManager)
        ) as LevelManager[];

        if (levelManager.Length > 0) {
            levelManager[0].LoadLevel();
        }
    }
}
