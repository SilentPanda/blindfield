using UnityEngine;
using System.Collections;
using InControl;

public class ControllerInput {
	public float speed = 0.1f;

	private InputDevice controller;

	public static InputDevice GetController() {
		var controller = InputManager.ActiveDevice;
		InputManager.AttachDevice (controller);
		return controller;
	}

	public static Vector3 TwoStickCombine(InputDevice controller) {
		Vector3 p1 = new Vector3(controller.LeftStickX.Value, controller.LeftStickY.Value);
		Vector3 p2 = new Vector3(controller.RightStickX.Value, controller.RightStickY.Value);
		float spd = Mathf.Min (p1.magnitude, p2.magnitude);
		return (p1 + p2) * spd;
	}

	public static void ShakeOtherSide (InputDevice controller) {
		Vector3 p1 = new Vector3(controller.LeftStickX.Value, controller.LeftStickY.Value);
		Vector3 p2 = new Vector3(controller.RightStickX.Value, controller.RightStickY.Value);

		controller.Vibrate (p2.magnitude, p1.magnitude);
	}

	public static void ShakeOnDifferentInput(InputDevice controller) {
		Vector3 p1 = new Vector3(controller.LeftStickX.Value, controller.LeftStickY.Value);
		Vector3 p2 = new Vector3(controller.RightStickX.Value, controller.RightStickY.Value);

		float dist = (p1 - p2).magnitude;
		if (dist > 0.85 && p1.magnitude > 0.9 && p2.magnitude > 0.9) {
			controller.Vibrate ((p1-p2).magnitude);
		} else {
			controller.Vibrate (0.0f);
		}
	}
}
