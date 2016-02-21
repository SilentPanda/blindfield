using UnityEngine;

public class IndestructibleBehaviour : MonoBehaviour {
    protected virtual void Awake() {
        DontDestroyOnLoad(gameObject);
    }
}
