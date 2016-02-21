using UnityEngine;
using System.Collections;
using InControl;

public class GroupedCirclesOverTargets : CirclesOverTargetsRuleSet {
    protected override void Control(InputDevice controller) {
        int i;

        for (i = 0; i < circles.Count / 2; i++) {
            circles[i].transform.position += (Vector3)controller.LeftStick.Value * inputSpeed;
        }

        for (i = i; i < circles.Count; i++) {
            circles[i].transform.position += (Vector3)controller.RightStick.Value * inputSpeed;
        }

        // FIXME: We need also to lock them together...
    }
}
