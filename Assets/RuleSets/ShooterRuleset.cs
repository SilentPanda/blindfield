using UnityEngine;
using System.Collections;
using InControl;

public class ShooterRuleset : CirclesOverTargetsRuleSet
{
    public float targetTime = 20f;
    float timer;

    void LateUpdate()
    {
        timer += Time.deltaTime;
        if ( timer > targetTime )
        {
            OnCompleted();
        }
    }
}
