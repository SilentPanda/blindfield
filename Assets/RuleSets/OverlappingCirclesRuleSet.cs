using System;
using System.Collections.Generic;
using UnityEngine;

public class OverlappingCirclesRuleSet : BaseRuleSet {
    public int circleCount = 2;

    public float overlapThreshold = 100.0f;

    // TODO: Pass these in as editor properties?
    private GameObject playerCirclePrefab;

    private List<GameObject> circles = new List<GameObject>();

    void Awake() {
        playerCirclePrefab = Resources.Load("PlayerCircle") as GameObject;
    }

    void OnEnable() {
        for (int i = 0; i < circleCount; i++) {
            GameObject circle = Instantiate(playerCirclePrefab);

            circle.name = String.Format("Player Circle {0}", i);

            circles.Add(circle);
        }
    }

    void OnDisable() {
        foreach (GameObject circle in circles) {
            Destroy(circle);
        }

        circles.Clear();
    }

    void Update() {
        CheckOverlap();
    }

    private void CheckOverlap() {
        if (!enabled) {
            return;
        }

        if (circles.Count < 2) {
            return;
        }

        bool allCirclesOverlap = true;

        for (int i = 0; i < circles.Count; i++) {
            bool circleOverlaps = false;

            for (int j = 0; j < circles.Count; j++) {
                float distance = Vector2.Distance(
                    circles[i].transform.position,
                    circles[j].transform.position
                );

                // TODO: Fix this to check that they overlap e.g. 50%.
                if (distance < overlapThreshold) {
                    circleOverlaps = true;

                    break;
                }
            }

            if (!circleOverlaps) {
                allCirclesOverlap = false;

                break;
            }
        }

        if (allCirclesOverlap) {
            OnCompleted();
        }
    }
}
