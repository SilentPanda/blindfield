using InControl;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlindfieldRuleSet : MonoBehaviour {
    public float inputSpeed = 1;

    public GameObject heavyPlayer;
    public GameObject lightPlayer;

    public GameObject treasure;

	public GameObject onWin;

    public AudioClip victory;

    public GameObject deathText;

    private float winWait = 0.25f;
    private float winTime = 0.0f;
	private bool won = false;
    private bool dead = false;

    public void Dead()
    {
        dead = true;
        deathText.SetActive(true);
    }

    void Update() {
		if (!won && !dead) {
			UpdatePlayers ();
			CheckWinConditions ();
		} else {
			ResetOnButton ();
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
        treasureBounds.size = treasureBounds.size * treasure.transform.localScale.x;

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
		Conductor.Stop ();

        AudioSource.PlayClipAtPoint(victory, Camera.main.transform.position);
	}

	private void ResetOnButton()
    {
        InputDevice device = InputManager.ActiveDevice;
        InputManager.AttachDevice(device);

        if ( device.AnyButton.IsPressed )
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
