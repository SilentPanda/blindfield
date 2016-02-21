using InControl;
using UnityEngine;

public class BlindfieldRuleSet : MonoBehaviour {
    public float inputSpeed = 1;

    public GameObject heavyPlayer;
    public GameObject lightPlayer;

    public GameObject treasure;

	public GameObject onWin;

    private float winWait = 0.25f;
    private float winTime = 0.0f;
	private bool won = false;

    void Update() {
		if (!won) {
			UpdatePlayers ();
			CheckWinConditions ();
		} else {
			WinDisplay ();
		}
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
			Win ();
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

	private void Win() {
		won = true;
		treasure.GetComponent<SpriteRenderer> ().enabled = true;
		GameObject.Instantiate (onWin);
	}

	private void WinDisplay() {
	}
}
