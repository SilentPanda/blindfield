using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {
	public GameObject sphere;
	public GameObject target;
	public Text winText;

	private float inWin = 0f;
	private float winDist = 0.3f;

	void Start() {
		sphere.transform.position = new Vector3 (Random.Range (-3, 3), Random.Range (-2, 2), 0);
	}

	void Update() {
		Vector3 relative = (sphere.transform.position - target.transform.position);
		if (relative.magnitude < winDist) {
			inWin += Time.deltaTime;
		} else {
			inWin = 0;
		}

		if (relative.magnitude < winDist && inWin > 0.25) {
			winText.enabled = true;
		}

		sphere.transform.position = new Vector3 (sphere.transform.position.x, sphere.transform.position.y, 0);
	}
}
