using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour 
{
	// Update is called once per frame
	void Update () 
	{
		transform.Translate( -Vector3.up * Time.deltaTime, Space.World );

		if (transform.position.y < -5)
			Destroy (gameObject);
	}
}
