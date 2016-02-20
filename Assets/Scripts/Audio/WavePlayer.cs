using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class WavePlayer : MonoBehaviour
{
    const int sampling_frequency = 48000;
    protected Wave activeWave;
    protected float time;

    protected virtual void FixedUpdate()
    {
        time = Time.time;
    }

    void OnAudioFilterRead( float[] data, int channels )
    {
        if (activeWave == null )
        {
            //play silence
        }
        else
        {
            //play activeWave
            activeWave.increment = activeWave.frequency * 2 * Mathf.PI / sampling_frequency;
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
