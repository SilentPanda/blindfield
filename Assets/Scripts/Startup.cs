using UnityEngine;
using System.Collections;
using InControl;
using UnityEngine.SceneManagement;

public class Startup : MonoBehaviour
{
	// Update is called once per frame
	void Update ()
    {
        InputDevice device = InputManager.ActiveDevice;
        InputManager.AttachDevice(device);

        if ( device.AnyButton.IsPressed )
        {
            SceneManager.LoadScene(1);
        }
    }
}
