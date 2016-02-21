using UnityEngine;
using System.Collections;

public class GameStart : MonoBehaviour {
	public GameObject lightPlayer;
	public GameObject heavyPlayer;
	public GameObject treasure;
	public GameObject minePrefab;

	public float minTreasureDist = 5.0f;
	public float minMineDist = 1.0f;
	public int numMines = 4;

	void Start() {
		// spawn players
		lightPlayer.transform.position = RandomPoint(2.0f);
		heavyPlayer.transform.position = lightPlayer.transform.position + new Vector3 (1.0f, 1.0f);

		// spawn treasure
		Vector3 treasurePos = RandomPoint();
		while ((treasurePos - lightPlayer.transform.position).magnitude < minTreasureDist) {
			treasurePos = RandomPoint ();
		}
		treasure.transform.position = treasurePos;

		// spawn mines
		for (int i = 0; i < numMines; i++) {
			Vector3 minePos = RandomPoint ();
			float player1Dist = (lightPlayer.transform.position - minePos).magnitude;
			float player2Dist = (heavyPlayer.transform.position - minePos).magnitude;
			float treasureDist = (treasure.transform.position - minePos).magnitude;

			while (player1Dist < minMineDist || player2Dist < minMineDist || treasureDist < minMineDist) {
				minePos = RandomPoint ();
				player1Dist = (lightPlayer.transform.position - minePos).magnitude;
				player2Dist = (heavyPlayer.transform.position - minePos).magnitude;
				treasureDist = (treasure.transform.position - minePos).magnitude;
			}

			var mine = GameObject.Instantiate (minePrefab);
			mine.transform.position = minePos;
		}

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
