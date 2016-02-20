using UnityEngine;
using System.Collections;
using InControl;

public class ControllerInput : MonoBehaviour {
	public float speed = 0.1f;

	private InputDevice controller; 
	void Start() {

	}

	void Update () {
		controller = InputManager.ActiveDevice;
		InputManager.AttachDevice (controller);
		Vector3 p1 = new Vector3(controller.LeftStickX.Value, controller.LeftStickY.Value);
		Vector3 p2 = new Vector3(controller.RightStickX.Value, controller.RightStickY.Value);

		float spd = Mathf.Min (p1.magnitude, p2.magnitude);

		controller.Vibrate (p2.magnitude, p1.magnitude);

		transform.position = transform.position + (p1+p2) * spd * speed;
	}
}
