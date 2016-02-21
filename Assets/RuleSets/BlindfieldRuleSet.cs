using InControl;
using UnityEngine;

public class BlindfieldRuleSet : MonoBehaviour {
    public float inputSpeed = 1;

    public GameObject heavyPlayer;
    public GameObject lightPlayer;

    public GameObject treasure;

    private float winWait = 0.25f;
    private float winTime = 0.0f;

    void Update() {
        UpdatePlayers();
        CheckWinConditions();
    }

    private void UpdatePlayers() {
        InputDevice controller = ControllerInput.GetController();

        float speed = inputSpeed * Time.deltaTime;

        heavyPlayer.transform.position += (Vector3)controller.LeftStick.Value * speed;
        lightPlayer.transform.position += (Vector3)controller.RightStick.Value * speed;
    }

    private void CheckWinConditions() {
        if (ShouldWin()) {
            winTime += Time.deltaTime;
        } else {
            winTime = 0.0f;
        }

        if (winTime >= winWait) {
            Debug.Log("Win!");
        }
    }

    private bool ShouldWin() {
        Bounds treasureBounds = treasure.GetComponent<SpriteRenderer>().sprite.bounds;

        Vector2 heavyPlayerToTreasure = (Vector2)(
            heavyPlayer.transform.position - treasure.transform.position
        );
        Vector2 lightPlayerToTreasure = (Vector2)(
            lightPlayer.transform.position - treasure.transform.position
        );

        if (
            treasureBounds.Contains(heavyPlayerToTreasure) &&
            treasureBounds.Contains(lightPlayerToTreasure)
        ) {
            return true;
        }

        return false;
    }
}
