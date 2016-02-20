using System;
using System.Collections.Generic;
using UnityEngine;

public class OverlappingCircles : MonoBehaviour {
    public delegate void CompletedEventHandler();
    public event CompletedEventHandler onCompleted;

    public int circleCount = 2;
    public int targetCount = 1;

    public float overlapThreshold = 100.0f;

    private List<GameObject> circles = new List<GameObject>();
    private List<GameObject> targets = new List<GameObject>();

    void Awake() {
        GameObject playerCirclePrefab = Resources.Load("PlayerCircle") as GameObject;
        GameObject targetPrefab = Resources.Load("Target") as GameObject;

        for (int i = 0; i < circleCount; i++) {
            GameObject circle = Instantiate(playerCirclePrefab);

            circle.name = String.Format("Player Circle {0}", i);

            circles.Add(circle);
        }

        for (int i = 0; i < targetCount; i++) {
            GameObject target = Instantiate(targetPrefab);

            target.name = String.Format("Target {0}", i);

            targets.Add(target);
        }
    }

    void Update() {
        CheckOverlap();
    }

    private void CheckOverlap() {
        if (circles.Count < 1) {
            return;
        }

        bool allCirclesOverlapTarget = true;

        foreach (GameObject circle in circles) {
            bool circleOverlapsTarget = false;

            foreach (GameObject target in targets) {
                float distance = Vector2.Distance(
                    circle.transform.position, target.transform.position
                );

                // TODO: Fix this to check that they're within the bounds instead.
                if (distance < overlapThreshold) {
                    circleOverlapsTarget = true;

                    break;
                }
            }

            if (!circleOverlapsTarget) {
                allCirclesOverlapTarget = false;

                break;
            }
        }

        if (allCirclesOverlapTarget && onCompleted != null) {
            onCompleted();
        }
    }
}
