using UnityEngine;
using System.Collections;

public class GameStart : MonoBehaviour {
	public GameObject lightPlayer;
	public GameObject heavyPlayer;
	public GameObject treasure;
	public GameObject minePrefab;

	void Start() {
		// spawn players
		lightPlayer.transform.position = RandomPoint(2.0f);
		heavyPlayer.transform.position = lightPlayer.transform.position + new Vector3 (1.0f, 1.0f);


	}

	Vector3 RandomPoint(float clearance = 1.0f) {
		// Get play area world coordinates
		var lowerLeft = Camera.main.ScreenToWorldPoint (new Vector3 (0, 0, 0));
		var upperRight = Camera.main.ScreenToWorldPoint (
			                 new Vector3 (Screen.width, Screen.height, 0)
		                 );
		return new Vector3 (Random.Range (lowerLeft.x+clearance, upperRight.x-clearance),
			Random.Range (lowerLeft.y+clearance, upperRight.y-clearance));
	}
}
