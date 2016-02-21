using UnityEngine;
using System.Collections;
using InControl;

public class DoVibration : MonoBehaviour
{
    public LightPlayer lp;
    public HeavyPlayer hp;
        
	// Update is called once per frame
	void LateUpdate ()
    {
        InputDevice device = InputManager.ActiveDevice;
        InputManager.AttachDevice(device);

        device.Vibrate(hp.vibration, lp.vibration);
    }
}
