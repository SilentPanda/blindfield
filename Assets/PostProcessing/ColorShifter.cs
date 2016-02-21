using UnityEngine;

public class ColorShifter : MonoBehaviour {
    public float blendSpeed;
    public Color sourceColor;
    public Color targetColor;

    private float blendAmount = 0;
    private int blendDirection = 1;

    void FixedUpdate() {
        blendAmount += blendDirection * blendSpeed;

        if (blendAmount > 1) {
            blendDirection = -1;
            blendAmount = 1;
        }

        if (blendAmount < 0) {
            blendDirection = 1;
            blendAmount = 0;
        }

        Renderer renderer = GetComponent<Renderer>();

        renderer.material.SetColor("_sourceColor", sourceColor);
        renderer.material.SetColor("_targetColor", targetColor);
        renderer.material.SetFloat("_blendAmount", blendAmount);
    }
}
