using System.Collections.Generic;
using UnityEngine;

public class ColorShifter : MonoBehaviour {
    public float blendSpeed;
    public List<Color> colors;

    private float blendAmount = 0;
    private int colorIndex = 0;

    void Start()
    {
        for (int i = 0; i < colors.Count; ++i)
        {
            Shader.SetGlobalColor("_BGColor" + i, colors[i]);
        }
    }

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

    void LateUpdate()
    {
        if (Mine.danger > 0)
        {
            Shader.SetGlobalFloat("_DangerT", ( Mathf.Sin( ( Time.time - Mine.dangerTime ) * 10 ) + 1 ) * .5f );
        }
        else
        {
            Shader.SetGlobalFloat("_DangerT", 0);
        }
    }
}
