using System;
using System.Collections.Generic;
using UnityEngine;

public class CirclesOverTargetsRuleSet : BaseRuleSet {
    public int circleCount = 2;
    public int targetCount = 1;
	public float inputSpeed = 0.5f;

    private float overlapThreshold = 1.0f;
	private float winWait = 0.25f;
	private float winTime = 0.0f;
    // TODO: Pass these in as editor properties?
    private GameObject playerCirclePrefab;
    private GameObject targetPrefab;
    private List<GameObject> circles = new List<GameObject>();
    private List<GameObject> targets = new List<GameObject>();

    void Awake() {
        playerCirclePrefab = Resources.Load("PlayerCircle") as GameObject;
        targetPrefab = Resources.Load("Target") as GameObject;
    }

    void OnEnable() {
        for (int i = 0; i < circleCount; i++) {
            GameObject circle = Instantiate(playerCirclePrefab);
			circle.transform.position = new Vector3 (-8.0f+i*0.5f, i*2f, 0.0f);

            circle.name = String.Format("Player Circle {0}", i);

            circles.Add(circle);
        }

        for (int i = 0; i < targetCount; i++) {
            GameObject target = Instantiate(targetPrefab);

            target.name = String.Format("Target {0}", i);

            targets.Add(target);
        }
    }

    void OnDisable() {
        foreach (GameObject circle in circles) {
            Destroy(circle);
        }

        circles.Clear();

        foreach (GameObject target in targets) {
            Destroy(target);
        }

        targets.Clear();
    }

    void Update() {
        CheckOverlap();
		var controller = ControllerInput.GetController ();
		var movement = ControllerInput.TwoStickCombine (controller);
		foreach (var circle in circles) {
			circle.transform.position += movement;
		}
    }

    private void CheckOverlap() {
        if (!enabled) {
            return;
        }

        if (circles.Count < 1) {
            return;
        }        

		if (AllCirclesOverlapTarget ()) {
			winTime += Time.deltaTime;
		} else {
			winTime = 0.0f;
		}

		if (winTime >= winWait) {
			OnCompleted ();
		}
    }

	private bool AllCirclesOverlapTarget() {
		foreach (GameObject circle in circles) {
			foreach (GameObject target in targets) {
				float distance = Vector2.Distance(
					circle.transform.position, target.transform.position);
				// TODO: Fix this to check that they're within the bounds instead.
				if (!(distance < overlapThreshold)) {
					return false;
				}
			}
		}
		return true;
	}
}
