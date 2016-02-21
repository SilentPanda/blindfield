using UnityEngine;
using System.Collections;
using InControl;

public class HeavyPlayer : MonoBehaviour
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
        if (d > maxRange)
        {
            //vibrate when pointing in right direction + latent vibration for distance
            vibration = ( d - maxRange ) / maxRange;

            Vector2 controlDir = new Vector2(device.LeftStick.X, device.LeftStick.Y);
            float dot = Mathf.Clamp(Vector2.Dot(controlDir, dir.normalized), 0, 1);

            vibration = Mathf.Clamp(vibration * dot, 0, 1);
        }
	}
}
