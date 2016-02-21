using UnityEngine;

public class TetheredCirclesNoTargetRuleSet : TetheredCirclesRuleSet {
    protected override bool AllOverlap() {
        if (circles.Count < 2) {
            return false;
        }

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
                return false;
            }
        }

        return true;
    }
}
