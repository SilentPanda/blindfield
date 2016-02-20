using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using InControl;

public class MixerControls : MonoBehaviour
{
    public AudioMixerGroup mixerGroup;
       
	// Update is called once per frame
	void Update ()
    {
        InputDevice device = InputManager.ActiveDevice;
        InputManager.AttachDevice(device);

        float avgX = (device.LeftStick.X + device.RightStick.X) * .5f;
        //remap
        avgX = (avgX + 1) * .5f;

        if ( !mixerGroup.audioMixer.SetFloat("EQ_Freq", Mathf.Lerp(100, 3000, avgX) ) )
        {
            Debug.LogWarning("NOT FOUND: EQ_Freq");
        }
    }
}
