using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Wave
{
    public string note = "";
    public float frequency;
    public float gain;
    public float gainPhaseSpeed = 0;
    public float gainPhaseRange = 0;
    public bool square;
    public float phase;
    public float increment;

    public void CopyFrom( Wave otherWave )
    {
        if (otherWave == null) return;

        this.note = otherWave.note;
        this.frequency = otherWave.frequency;
        this.gain = otherWave.gain;
        this.gainPhaseSpeed = otherWave.gainPhaseSpeed;
        this.gainPhaseRange = otherWave.gainPhaseRange;
        this.square = otherWave.square;
        //this.phase = otherWave.phase;
        //this.increment = otherWave.increment;
    }
}

[System.Serializable]
public enum WaveCombineMode
{
    Chord = 0,
    Toggle
}

[RequireComponent(typeof(AudioSource))]
public class SineWave : MonoBehaviour
{
    const int sampling_frequency = 48000;

    public List<Wave> waves;
    public WaveCombineMode mode;
    public float toggleTime = .05f;
    int toggleIndex = 0;

    float time;
    float toggleTimer;

    void Update()
    {
        foreach( Wave w in waves )
        {
            if (!string.IsNullOrEmpty(w.note))
            {
                w.frequency = NoteConverter.getFreq(w.note);
            }
        }
    }

    void FixedUpdate()
    {
        time += Time.fixedDeltaTime;
        toggleTimer += Time.fixedDeltaTime;
        if ( toggleTimer > toggleTime )
        {
            toggleIndex = ++toggleIndex % waves.Count;
            toggleTimer = 0;
        }
    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        switch( mode )
        {
            case WaveCombineMode.Chord:
                {
                    for (int w = 0; w < waves.Count; ++w)
                    {
                        Wave wave = waves[w];
                        wave.increment = wave.frequency * 2 * Mathf.PI / sampling_frequency;
                        for (var i = 0; i < data.Length; i = i + channels)
                        {
                            wave.phase = wave.phase + wave.increment;
                            // this is where we copy audio data to make them “available” to Unity
                            float targetGain = (wave.gain + Mathf.Sin(wave.gainPhaseSpeed * time) * wave.gainPhaseRange);
                            data[i] += (float)(targetGain) * Mathf.Sin(wave.phase);
                            if (wave.square) data[i] = (data[i] > 0) ? targetGain : -targetGain;
                            // if we have stereo, we copy the mono data to each channel
                            if (channels == 2) data[i + 1] = data[i];
                            if (wave.phase > 2 * Mathf.PI) wave.phase = 0;
                        }
                    }
                }
                break;
            case WaveCombineMode.Toggle:
                {
                    Wave wave = waves[toggleIndex];
                    wave.increment = wave.frequency * 2 * Mathf.PI / sampling_frequency;
                    for (var i = 0; i < data.Length; i = i + channels)
                    {
                        wave.phase = wave.phase + wave.increment;
                        // this is where we copy audio data to make them “available” to Unity
                        float targetGain = (wave.gain + Mathf.Sin(wave.gainPhaseSpeed * time) * wave.gainPhaseRange);
                        data[i] = (float)(targetGain) * Mathf.Sin(wave.phase);
                        if (wave.square) data[i] = (data[i] > 0) ? targetGain : -targetGain;
                        // if we have stereo, we copy the mono data to each channel
                        if (channels == 2) data[i + 1] = data[i];
                        if (wave.phase > 2 * Mathf.PI) wave.phase = 0;
                    }
                }
                break;
        }

        // update increment in case frequency has changed
        
    }
}
