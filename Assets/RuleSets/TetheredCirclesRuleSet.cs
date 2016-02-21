using UnityEngine;
using System.Collections;
using InControl;

public class TetheredCirclesRuleSet : CirclesOverTargetsRuleSet {
	public float maxDist = 5f;
	private GameObject tetherPrefab;
	private GameObject tether;

	protected override void Awake() {
		base.Awake ();
		tetherPrefab = Resources.Load ("Tether") as GameObject;
	}

	protected override void OnEnable() {
		base.OnEnable ();
		tether = GameObject.Instantiate (tetherPrefab);
	}

	protected override void Control(InputDevice controller) {
		circles [0].transform.position += (Vector3)controller.LeftStick.Value * inputSpeed;
		circles [1].transform.position += (Vector3)controller.RightStick.Value * inputSpeed;
		//ControllerInput.ShakeOnDifferentInput (controller);

		TetherCircles ();

		var renderer = tether.GetComponent<LineRenderer> ();
		renderer.SetPosition (0, circles [0].transform.position);
		renderer.SetPosition (1, circles [1].transform.position);
	}

	private void TetherCircles() {
		var c0 = circles [0].transform.position;
		var c1 = circles [1].transform.position;
		Vector3 midpoint = new Vector3 ((c0.x + c1.x) / 2, (c0.y + c1.y) / 2);
		var dist = (c0-c1).magnitude;

		if (dist > maxDist) {
			var relPos = -(midpoint - c0);
			circles [0].transform.position = (midpoint + relPos.normalized * (maxDist / 2));

			relPos = -(midpoint - c1);
			circles [1].transform.position = (midpoint + relPos.normalized * (maxDist / 2));
		}
	}
}
