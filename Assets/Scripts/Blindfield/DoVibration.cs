using UnityEngine;
using System.Collections;
using InControl;

public class DoVibration : MonoBehaviour
{
    public LightPlayer lp;
    public HeavyPlayer hp;

    Vector4 playerCenter;
        
	// Update is called once per frame
	void LateUpdate ()
    {
        InputDevice device = InputManager.ActiveDevice;
        InputManager.AttachDevice(device);

        device.Vibrate(hp.vibration, lp.vibration);

        playerCenter.x = (hp.transform.position.x + lp.transform.position.x) * .5f;
        playerCenter.y = (hp.transform.position.y + lp.transform.position.y) * .5f;

        playerCenter = Camera.main.WorldToScreenPoint(playerCenter);
        playerCenter.x /= Screen.width;
        playerCenter.y /= Screen.height;

        Shader.SetGlobalVector("_playerCenter", playerCenter);
    }
}
