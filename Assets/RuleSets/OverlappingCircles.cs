using System;
using System.Collections.Generic;
using UnityEngine;

public class OverlappingCircles : BaseRuleSet {
    public int circleCount = 2;
    public int targetCount = 1;

    public float overlapThreshold = 100.0f;

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
    }

    private void CheckOverlap() {
        if (!enabled) {
            return;
        }

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

        if (allCirclesOverlapTarget) {
            OnCompleted();
        }
    }
}
