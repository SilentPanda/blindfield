using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour 
{
	public Transform topLeft, topRight, leftPlayer, rightPlayer;
	public Object enemyPrefab;
	public Renderer laser;
	public ParticleSystem[] particles;

	public float minBPM, maxBPM, minDist, maxDist;

	int beat;

	Conductor conductor;

	IEnumerator Start()
	{
		Conductor.onBeat += Beat;
		laser.enabled = false;

		conductor = FindObjectOfType<Conductor> ();

		while (Application.isPlaying) 
		{
            float t = Random.value;
            float x = Mathf.Lerp(topLeft.position.x, topRight.position.x, t);
            float y = Mathf.Lerp(topLeft.position.y, topRight.position.y, t);

			Vector3 position = new Vector3(x, y, 0);
            GameObject enemy = (GameObject)Instantiate (enemyPrefab, position, Quaternion.identity);

            yield return new WaitForSeconds(2.5f);
        }
	}

	void Beat()
	{
		beat++;

		if (beat % 4 == 3)
		{
			DoLaser ();
		}
	}

	void DoLaser()
	{
		laser.enabled = true;
		for (int i = 0; i < particles.Length; i++) 
		{
			particles [i].Play ();
		}

		RaycastHit2D[] hit = Physics2D.LinecastAll(leftPlayer.position, rightPlayer.position, 1 << 8 ); //enemies
		for (int i = 0; i < hit.Length; ++i)
		{
			Destroy (hit [i].collider.gameObject);
			//screenshake
			CameraShake.ShakeFor(.25f);
		}

		Invoke ("LaserOff", .1f);
	}

	void LaserOff()
	{
		laser.enabled = false;
	}

	void Update()
	{
		if (conductor) 
		{
			float dist = Vector3.Distance (leftPlayer.position, rightPlayer.position);
			dist = Mathf.Clamp (dist, minDist, maxDist);
			float t = (dist - minDist) / (maxDist - minDist);
			conductor.BPM = Mathf.Lerp (minBPM, maxBPM, t);
		}
	}
}
