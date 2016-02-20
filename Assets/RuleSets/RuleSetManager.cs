using System.Collections.Generic;
using UnityEngine;

public class RuleSetManager : MonoBehaviour {
    void Start() {
        DisableAllRuleSets();
        StartNewRandomRuleSet();
    }

    public void StartNewRandomRuleSet() {
        DisableAllRuleSets();

        BaseRuleSet[] ruleSets = gameObject.GetComponents<BaseRuleSet>();

        int index = Random.Range(0, ruleSets.Length);

        BaseRuleSet ruleSet = ruleSets[index];

        ruleSet.enabled = true;

        ruleSet.onCompleted += StartNewRandomRuleSet;
    }

    private void DisableAllRuleSets() {
        BaseRuleSet[] ruleSets = gameObject.GetComponents<BaseRuleSet>();

        foreach (BaseRuleSet ruleSet in ruleSets) {
            ruleSet.onCompleted -= StartNewRandomRuleSet;

            ruleSet.enabled = false;
        }
    }
}
