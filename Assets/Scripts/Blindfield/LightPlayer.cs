using UnityEngine;
using System.Collections;
using InControl;

public class LightPlayer : MonoBehaviour
{
    public Transform treasure;
    public float maxRange = 2;

    public float vibration = 0;

	// Update is called once per frame
	void Update ()
    {
        InputDevice device = InputManager.ActiveDevice;
        InputManager.AttachDevice(device);

        Vector3 dir = treasure.position - transform.position;
        //calc dist to treasure
        float d = dir.magnitude;
        //if within range
        if (d < maxRange)
        {
            //vibrate when pointing in right direction + latent vibration for distance
            vibration = (maxRange - d) / maxRange;
            vibration = Mathf.Clamp(vibration, 0, 1);
        }
	}
}
