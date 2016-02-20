using UnityEngine;

public class BaseRuleSet : MonoBehaviour {
    public delegate void CompletedEventHandler();
    public event CompletedEventHandler onCompleted;

    protected void OnCompleted() {
        if (onCompleted != null) {
            onCompleted();
        }
    }
}
