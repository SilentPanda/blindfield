using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using InControl;

public class MixerControls : MonoBehaviour
{
    public AudioMixerGroup mixerGroup;
    Conductor conductor;

    public float minBPM, maxBPM;
    public float minDist, maxDist;

    public Transform heavyPlayer, lightPlayer;
    public Transform treasure;

    void Start()
    {
        conductor = FindObjectOfType<Conductor>();
    }

    // Update is called once per frame
    void Update()
    {
        InputDevice device = InputManager.ActiveDevice;
        InputManager.AttachDevice(device);

        if (conductor)
        {
            float d1 = Vector3.Distance(heavyPlayer.position, treasure.position);
            float d2 = Vector3.Distance(lightPlayer.position, treasure.position);

            float dist = Mathf.Min(d1, d2);

            dist = Mathf.Clamp(dist, minDist, maxDist);
            float t = (dist - minDist) / (maxDist - minDist);
            conductor.BPM = Mathf.Lerp(minBPM, maxBPM, 1 - Mathf.Pow( t, 8 ));
        }
    }
}