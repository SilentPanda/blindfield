using UnityEngine;
using System.Collections;
using InControl;

public class BreakApartRuleSet : BaseRuleSet {
	public GameObject playerCircle1;
	public GameObject playerCircle2;

	public float breakApartSpeed = 0.01f;
	public float breakApartDist = 1.0f;
	public float inputSpeed = 0.2f;

	private float breakApartTimeElapsed = 0.0f;

	void Update() {
		var controller = ControllerInput.GetController ();
		BreakApart (controller);
	}

	void FixedUpdate() {
		var controller = ControllerInput.GetController ();
		Control (controller);
		PullTogether ();
		ClampCircle ();
	}

	private void Control(InputDevice controller) {
		playerCircle1.transform.position += ControllerInput.TwoStickCombine (controller) * inputSpeed;
		playerCircle2.transform.position += ControllerInput.TwoStickCombine (controller) * inputSpeed;

		playerCircle1.transform.position += (Vector3)controller.LeftStick.Value * breakApartSpeed;
		playerCircle2.transform.position += (Vector3)controller.RightStick.Value * breakApartSpeed;

		ControllerInput.ShakeOnDifferentInput (controller);
	}

	private void BreakApart(InputDevice controller) {
		var dist = (playerCircle1.transform.position - playerCircle2.transform.position).magnitude;
		if (dist > breakApartDist) {
			OnCompleted ();
		}
	}

	private void PullTogether() {
		var c0 = playerCircle1.transform.position;
		var c1 = playerCircle2.transform.position;
		Vector3 midpoint = new Vector3 ((c0.x + c1.x) / 2, (c0.y + c1.y) / 2);
		var dist = (c0-c1).magnitude;

		if (dist > 0) {
			var relVec = (midpoint - c0);
			playerCircle1.transform.position += relVec.normalized * breakApartSpeed / 2;
			relVec = (midpoint - c1);
			playerCircle2.transform.position += relVec.normalized * breakApartSpeed / 2;
		}
	}

	private void ClampCircle() {
		var lowerLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
		var upperRight = Camera.main.ScreenToWorldPoint(
			new Vector3(Screen.width, Screen.height, 0)
		);

		Rect screen = new Rect(
			lowerLeft.x, lowerLeft.y,
			upperRight.x-lowerLeft.x, upperRight.y-lowerLeft.y
		);
			
		playerCircle1.transform.position = new Vector3(
			Mathf.Clamp(playerCircle1.transform.position.x, screen.x, screen.xMax),
			Mathf.Clamp(playerCircle1.transform.position.y, screen.y, screen.yMax)
		);
		playerCircle2.transform.position = new Vector3(
			Mathf.Clamp(playerCircle2.transform.position.x, screen.x, screen.xMax),
			Mathf.Clamp(playerCircle2.transform.position.y, screen.y, screen.yMax)
		);

	}
}
