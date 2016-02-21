using System;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class CirclesOverTargetsRuleSet : BaseRuleSet {
    public float inputSpeed = 0.12f;

    protected float overlapThreshold = 1.0f;

    private float winWait = 0.25f;
    private float winTime = 0.0f;

    public List<GameObject> circles = new List<GameObject>();
    public List<GameObject> targets = new List<GameObject>();

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

    protected virtual bool AllOverlap() {
        if (circles.Count < 1) {
            return false;
        }

        foreach (GameObject circle in circles) {
			bool circleOverlaps = false;

            foreach (GameObject target in targets) {
                float distance = Vector2.Distance(
                    circle.transform.position, target.transform.position
                );

                // TODO: Fix this to check that they're within the bounds instead.
                if (distance < overlapThreshold) {
					circleOverlaps = true;

                    break;
                }

				// var circleBounds = circle.GetComponent<SpriteRenderer>().sprite.bounds;
				// var targetBounds = target.GetComponent<SpriteRenderer>().sprite.bounds;
				// Debug.Log ("Circle:" + circleBounds);
				// Debug.Log ("Target" + targetBounds);
				// if (targetBounds.Contains (circleBounds.min)
				//    && targetBounds.Contains (circleBounds.max)) {
				// 	circleOverlaps = true;
				// }
            }

			if (!circleOverlaps) {
				return false;
			}
        }

        return true;
    }
}
