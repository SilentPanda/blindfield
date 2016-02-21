using System;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class CirclesOverTargetsRuleSet : BaseRuleSet {
    /*public int circleCount = 1;
    public int targetCount = 1;*/
    public float inputSpeed = 0.12f;

    private float overlapThreshold = 1.0f;
    private float winWait = 0.25f;
    private float winTime = 0.0f;

    // TODO: Pass these in as editor properties?
    /*public GameObject playerCirclePrefab;
    public GameObject targetPrefab;*/

    public List<GameObject> circles = new List<GameObject>();
    public List<GameObject> targets = new List<GameObject>();

    protected virtual void Awake() {
        /*if (!playerCirclePrefab)
            playerCirclePrefab = Resources.Load("PlayerCircle") as GameObject;
        if(!targetPrefab)
            targetPrefab = Resources.Load("Target") as GameObject;*/

    }

    protected virtual void OnEnable() {
        /*for (int i = 0; i < circleCount; i++) {
            GameObject circle = Instantiate(playerCirclePrefab);

            circle.name = String.Format("Player Circle {0}", i);
			if (i == 1) {
				circle.GetComponent<SpriteRenderer> ().color = Color.red;
			}

            circle.transform.position = new Vector3 (
                -8.0f, i * 2f, 0.0f
            );

            circles.Add(circle);
        }

        for (int i = 0; i < targetCount; i++) {
            GameObject target = Instantiate(targetPrefab);

            target.name = String.Format("Target {0}", i);

			target.transform.position = new Vector3 (
				0.0f, i * 2f, 0.0f
			);

            targets.Add(target);
        }*/
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
        CheckCompletionConditions();
    }

	void FixedUpdate() {
		var controller = ControllerInput.GetController();
		Control (controller);
		ClampCircles();
	}

	protected virtual void Control(InputDevice controller) {
		var movement = ControllerInput.TwoStickCombine(controller);
		foreach (var circle in circles) {
			circle.transform.position += movement * inputSpeed;
		}
		ControllerInput.ShakeOnDifferentInput(controller);
	}

    private void ClampCircles() {
        var lowerLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        var upperRight = Camera.main.ScreenToWorldPoint(
            new Vector3(Screen.width, Screen.height, 0)
        );

        Rect screen = new Rect(
            lowerLeft.x, lowerLeft.y,
            upperRight.x-lowerLeft.x, upperRight.y-lowerLeft.y
        );

        foreach (var circle in circles) {
            circle.transform.position = new Vector3(
                Mathf.Clamp(circle.transform.position.x, screen.x, screen.xMax),
                Mathf.Clamp(circle.transform.position.y, screen.y, screen.yMax)
            );
        }
    }

    private void CheckCompletionConditions() {
        if (!enabled) {
            return;
        }

        if (AllOverlap()) {
            winTime += Time.deltaTime;
        } else {
            winTime = 0.0f;
        }

        if (winTime >= winWait) {
            OnCompleted();
			Debug.Log ("Win!");
        }
    }

    private bool AllOverlap() {
        if (circles.Count < 1) {
            return false;
        }

        foreach (GameObject circle in circles) {
			bool circleOverlap = false;
            foreach (GameObject target in targets) {
                float distance = Vector2.Distance(
                    circle.transform.position, target.transform.position
                );

                // TODO: Fix this to check that they're within the bounds instead.
                if (distance < overlapThreshold) {
					circleOverlap = true;
                }
				/*var circleBounds = circle.GetComponent<SpriteRenderer>().sprite.bounds;
				var targetBounds = target.GetComponent<SpriteRenderer>().sprite.bounds;
				Debug.Log ("Circle:" + circleBounds);
				Debug.Log ("Target" + targetBounds);
				if (targetBounds.Contains (circleBounds.min)
				   && targetBounds.Contains (circleBounds.max)) {
					circleOverlap = true;
				}*/
            }

			if (!circleOverlap) {
				return false;
			}
        }
        return true;
    }
}
