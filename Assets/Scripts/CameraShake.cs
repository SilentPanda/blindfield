using UnityEngine;
using System.Collections;

#if UNITY_XBOXONE
using Gamepad;
#endif

#if UNITY_PS4
using UnityEngine.PS4;
#endif

public class CameraShake : MonoBehaviour 
{
	//only makes sense to call on classes that update after the CameraShake class
	public static Vector3 ShakePosition
	{
		get
		{
			if ( instance == null )
			{
				return Vector3.zero;
			}

			if ( instance.shakeTime > 0 )
			{
				Vector3 ret = instance.T.position;
				ret.x += instance.xAdjustment;
				ret.y += instance.yAdjustment;
				return ret;
			}
			else
			{
				return instance.T.position;
			}
		}
	}

    public static Vector3 ShakeOffset
    {
        get
        {
            if (instance == null || instance.shakeTime <= 0 )
            {
                return Vector3.zero;
            }
            
            Vector3 ret = Vector3.zero;
            ret.x += instance.xAdjustment;
            ret.y += instance.yAdjustment;
            return ret;
        }
    }

    public float scale = 1;

	const float shakeFreqX = 10;
	const float shakeFreqY = 8;
	const float shakeFreqY2 = 20;
	const float shakeSizeX = .05f;
	const float shakeSizeY = .1f;
	const float shakeSizeY2 = .035f;

	const float shakeRateX = 10;
	const float shakeRateY = 100;

	const float MAX_SHAKE_TIME = 1f;

	static CameraShake instance;
	
	float shakeTime = 0;
	Transform T;
	Vector3 originalPosition;
	bool shook = false;

	int freezeFrames = 0;

	bool keepRunning = true;

	//shake values
	float t;
	float xAdjustment;
	float yAdjustment;

    public static void SetScale( float scale )
    {
        if (instance == null) return;

        instance.scale = scale;
    }

    public static void ResetScale()
    {
        if (instance == null) return;

        instance.scale = 1;
    }

	public static void ShakeFor( float time )
	{
		if ( instance == null ) return;

		instance.Shake( time );
	}

    public static void Stop()
    {
        if (instance == null) return;

        instance.StopShake();
    }

	public static void FreezeFrame( int frames )
	{
		if ( instance == null ) return;

		Time.timeScale = 0;
		instance.freezeFrames += frames;
	}

	//public because Animator
	public void Shake( float time )
	{
		if ( time < MAX_SHAKE_TIME )
		{
			shakeTime = Mathf.Clamp( instance.shakeTime + time, 0, MAX_SHAKE_TIME );
		}
		else
		{
			shakeTime = time;
		}
	}


	public void StartShake()
	{
		StartCoroutine(KeepShaking());
	}


	public void StopShake()
	{
		keepRunning = false;
		shakeTime = 0;
	}


	void Awake()
	{
		instance = this;
		T = transform;
	}


	void OnDestroy()
	{
		keepRunning = false;
	}


	IEnumerator KeepShaking()
	{
		keepRunning = true;

		while( keepRunning && Application.isPlaying )
		{
			ShakeFor( .01f );
			yield return 0;
		}

		yield return 0;
	}

	void Update()
	{
        UpdateControllers();

        if ( shakeTime > 0 )
		{
			shakeTime -= Time.deltaTime;
			t = Time.time;
			xAdjustment = Mathf.Sin( t*shakeRateX )*shakeSizeX;
			yAdjustment = Mathf.Sin( t*shakeRateY )*shakeSizeY + Mathf.Cos( t*shakeFreqY2 )*shakeSizeY2;
			originalPosition = T.position;
		}
        if (freezeFrames > 0)
        {
            if (--freezeFrames <= 0)
            {
                Time.timeScale = 1;
            }
        }
	}

	void OnPreRender () 
	{
		if ( shakeTime > 0 )
		{
			shook = true;
			originalPosition = T.position;

			Vector3 newPosition = originalPosition;
			newPosition.x += xAdjustment * scale;
			newPosition.y += yAdjustment * scale;
			T.position = newPosition;


        }
		else
		{
#if UNITY_XBOXONE && !UNITY_EDITOR
            GamepadPlugin.SetGamepadVibration(XboxOneInput.GetControllerId(0), 0, 0, 0, 0);
#endif
		}
	}

	void OnPostRender()
	{
		if ( shook )
		{
			T.position = originalPosition;
		}

		shook = false;
	}

    void UpdateControllers()
    {
        if ( shakeTime > 0 )
        {
            DoVibration();
        }
        else
        {
            StopVibration();
        }
    }

    void DoVibration()
    {
#if UNITY_XBOXONE && !UNITY_EDITOR
			GamepadPlugin.SetGamepadVibration(XboxOneInput.GetControllerId(0), .5f, .5f, 0, 0 );
#endif

#if UNITY_PS4 && !UNITY_EDITOR
        int val = 0;
        if (int.TryParse(LoginHandler.instance.UserID, out val))
        {
            PS4Input.PadSetVibration(val, (int)Mathf.Clamp(127 * scale, 0, 255), (int)Mathf.Clamp(127 * scale, 0, 255));
        }
#endif
    }

    void StopVibration()
    {
#if UNITY_XBOXONE && !UNITY_EDITOR
			GamepadPlugin.SetGamepadVibration(XboxOneInput.GetControllerId(0), 0f, 0f, 0, 0 );
#endif

#if UNITY_PS4 && !UNITY_EDITOR
        int val = 0;
        if (int.TryParse(LoginHandler.instance.UserID, out val))
        {
            PS4Input.PadSetVibration(val, 0, 0);
        }
#endif
    }
}