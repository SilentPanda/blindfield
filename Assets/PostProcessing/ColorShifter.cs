using System.Collections.Generic;
using UnityEngine;

public class ColorShifter : MonoBehaviour {
    public float blendSpeed;
    public List<Color> colors;

    private float blendAmount = 0;
    private int colorIndex = 0;

    void FixedUpdate() {
        blendAmount += blendSpeed;

        if (blendAmount > 1) {
            blendAmount = 0;

            colorIndex++;
            colorIndex %= colors.Count;
        }

        int nextColorIndex = (colorIndex + 1) % colors.Count;

        Renderer renderer = GetComponent<Renderer>();

        renderer.material.SetColor("_sourceColor", colors[colorIndex]);
        renderer.material.SetColor("_targetColor", colors[nextColorIndex]);

        renderer.material.SetFloat("_blendAmount", blendAmount);
    }
}
