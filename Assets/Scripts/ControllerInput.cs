using UnityEngine;
using System.Collections;

public class ControllerInput : MonoBehaviour {
	public float speed = 0.1f;

	void Update () {
		Vector3 p1 = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		Vector3 p2 = new Vector3(Input.GetAxis("Horizontal2"), Input.GetAxis("Vertical2"));
		float spd = Mathf.Min (p1.magnitude, p2.magnitude);

		transform.position = transform.position + (p1+p2) * spd * speed;
	}
}
