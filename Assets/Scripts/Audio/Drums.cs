using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class Drums : MonoBehaviour
{
    public enum Type
    {
        KICK = 0,
        SNARE,
        HIHAT
    }

    static float[] TYPE_TIMES = new float[3]
    {
        .25f,
        .35f,
        .1f
    };

    public AnimationCurve kickFrequency;
    public AnimationCurve kickGain;

    public string notes = "";

    System.Random random = new System.Random();

    public Wave waveSettings;

    List<Wave> waves = new List<Wave>();
    List<float> startTimes = new List<float>();
    List<Type> types = new List<Type>();

    float time;

    public void DoKick()
    {
        Wave w = new Wave();
        w.CopyFrom(waveSettings);
        
        //TODO: What is kick?
        w.frequency = 80;
        Type t = Type.KICK;

        waves.Add(w);
        types.Add(t);
        startTimes.Add(Time.time);
    }

    public void DoSnare()
    {
        Wave w = new Wave();
        w.CopyFrom(waveSettings);
        
        Type t = Type.SNARE;

        waves.Add(w);
        types.Add(t);
        startTimes.Add(Time.time);
    }

    public void DoHats()
    {
        Wave w = new Wave();
        w.CopyFrom(waveSettings);
        
        Type t = Type.HIHAT;

        waves.Add(w);
        types.Add(t);
        startTimes.Add(Time.time);
    }

    IEnumerator Start()
    {
        //basic base/snare
        while ( Application.isPlaying )
        {
            DoKick();
            DoHats();
            yield return new WaitForSeconds(.25f);
            DoHats();
            yield return new WaitForSeconds(.25f);
            DoSnare();
            DoHats();
            yield return new WaitForSeconds(.25f);
            DoHats();
            yield return new WaitForSeconds(.25f);
        }
    }

    void Update()
    {
        time = Time.time;
    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        for (int w = 0; w < waves.Count; ++w)
        {
            if ( time - startTimes[w] > TYPE_TIMES[(int)types[w]] )
            {
                startTimes.RemoveAt(w);
                types.RemoveAt(w);
                waves.RemoveAt(w);
                w--;

                continue;
            }

            Wave wave = waves[w];
            float t = (time - startTimes[w]) / TYPE_TIMES[(int)types[w]];

            switch (types[w])
            {
                case Type.KICK:

                    //two sine waves at different frequencies
                    AddSine(ref data, channels, true, 20 * kickFrequency.Evaluate(t), wave.gain * kickGain.Evaluate(t) );
                    AddSine(ref data, channels, true, 50 * kickFrequency.Evaluate(t), wave.gain * kickGain.Evaluate(t) * .5f );

                    //maybe add some noise?
                    AddWhiteNoise(ref data, channels, 100, wave.gain * kickGain.Evaluate(t) * .1f);

                    break;
                case Type.SNARE:

                    AddWhiteNoise(ref data, channels, 100, wave.gain * kickGain.Evaluate(t));

                    break;
                case Type.HIHAT:

                    AddWhiteNoise(ref data, channels, 100, wave.gain * kickGain.Evaluate(t) * .5f);

                    break;
            }
            /*
            wave.increment = ( wave.frequency * 2 * Mathf.PI ) / GlobalSoundVariables.SAMPLING_FREQUENCY;
            for (var i = 0; i < data.Length; i = i + channels)
            {
                wave.phase = wave.phase + wave.increment;

                // this is where we copy audio data to make them “available” to Unity
                float targetGain;

                float t = (time - startTimes[w]) / TYPE_TIMES[(int)types[w]];

                
                        //full gain, decrease exponentially, somewhat higher frequency at the start (cosine)

                        float kG = kickGain.Evaluate(t);
                        float kF = kickFrequency.Evaluate(t) * wave.frequency;

                        targetGain = (float)( wave.gain * kG );
                        data[i] = targetGain * Mathf.Sin(wave.phase);
                        break;
                    case Type.SNARE:
                        targetGain = (float)(wave.gain * random.NextDouble());
                        targetGain = (targetGain < 0) ? -targetGain : targetGain;
                        data[i] = targetGain;
                        break;
                    case Type.HIHAT:
                        targetGain = (wave.gain + Mathf.Cos(wave.gainPhaseSpeed * time) * wave.gainPhaseRange);
                        data[i] = (float)(random.NextDouble() * Mathf.Cos(wave.phase));
                        break;
                }

                // if we have stereo, we copy the mono data to each channel
                if (channels == 2) data[i + 1] = data[i];
                if (wave.phase > 2 * Mathf.PI) wave.phase = 0;
            }
            */
        }
    }

    void AddSine( ref float[] data, int channels, bool square, float frequency, float gain )
    {
        float phase = 0;
        float increment = frequency * 2 * Mathf.PI / GlobalSoundVariables.SAMPLING_FREQUENCY;
        for (var i = 0; i < data.Length; i = i + channels)
        {
            phase = phase + increment;
            
            // this is where we copy audio data to make them “available” to Unity
            float targetGain = (float)(gain) * Mathf.Sin(phase);
            if (square) targetGain = (targetGain < 0) ? -targetGain : targetGain;

            data[i] += (float)(targetGain) * Mathf.Sin(phase);

            // if we have stereo, we copy the mono data to each channel
            if (channels == 2) data[i + 1] = data[i];
            if (phase > 2 * Mathf.PI) phase = 0;
        }
    }

    void AddWhiteNoise( ref float[] data, int channels, float frequency, float gain )
    {
        float phase = 0;
        float increment = frequency * 2 * Mathf.PI / GlobalSoundVariables.SAMPLING_FREQUENCY;
        for (var i = 0; i < data.Length; i = i + channels)
        {
            phase = phase + increment;
            
            // this is where we copy audio data to make them “available” to Unity
            float targetGain = (float)(gain * random.NextDouble());
            targetGain = (targetGain < 0) ? -targetGain : targetGain;
            data[i] += targetGain;

            // if we have stereo, we copy the mono data to each channel
            if (channels == 2) data[i + 1] = data[i];
            if (phase > 2 * Mathf.PI) phase = 0;
        }
    }
}