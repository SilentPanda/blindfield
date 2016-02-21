using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class WavePlayer : MonoBehaviour
{
    protected Wave activeWave = new Wave();
    protected float time;

    protected virtual void FixedUpdate()
    {
        time = Time.time;
    }

    protected virtual void OnAudioFilterRead( float[] data, int channels )
    {
        if ( activeWave.gain == 0 )
        {
            //play silence
        }
        else
        {
            //play activeWave
            activeWave.increment = activeWave.frequency * 2 * Mathf.PI / GlobalSoundVariables.SAMPLING_FREQUENCY;
            for (var i = 0; i < data.Length; i = i + channels)
            {
                activeWave.phase = activeWave.phase + activeWave.increment;
                // this is where we copy audio data to make them “available” to Unity
                float targetGain = (activeWave.gain + Mathf.Sin(activeWave.gainPhaseSpeed * time) * activeWave.gainPhaseRange);
                data[i] = (float)(targetGain) * Mathf.Sin(activeWave.phase);
                if (activeWave.square) data[i] = (data[i] > 0) ? targetGain : -targetGain;
                // if we have stereo, we copy the mono data to each channel
                if (channels == 2) data[i + 1] = data[i];
                if (activeWave.phase > 2 * Mathf.PI) activeWave.phase = 0;
            }
        }
    }
}
