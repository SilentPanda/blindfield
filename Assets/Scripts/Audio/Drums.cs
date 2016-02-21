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

    const float BPM_Conversion = .05f;

    public AnimationCurve kickFrequency;
    public AnimationCurve kickGain;

    public List<string[]> variations;
    public string[] activeMelody;
    int currentNote = 0;

    System.Random random = new System.Random();

    public Wave waveSettings;

    List<Wave> waves = new List<Wave>();
    List<float> startTimes = new List<float>();
    List<Type> types = new List<Type>();

    float time;

    public void SetNotes( List<string> notes )
    {
        variations = new List<string[]>();

        //parse into variations
        for (int i = 0; i < notes.Count; ++i)
        {
            variations.Add(notes[i].Split(';'));
        }

        enabled = true;
    }

    public void DoNote()
    {
        if (activeMelody == null || currentNote >= activeMelody.Length) SelectVariation();

        string note = activeMelody[currentNote];
        string[] stuff = note.Split(',');
        for( int i = 0; i < stuff.Length; ++i )
        {
            switch( stuff[i] )
            {
                case "b":
                    DoKick();
                    break;
                case "h":
                    DoHats();
                    break;
                case "s":
                    DoSnare();
                    break;
            }
        }

        if (++currentNote >= activeMelody.Length) SelectVariation();
    }

    void SelectVariation()
    {
        if (variations == null)
        {
            Debug.LogError("No variations for synth!");
            return;
        }
        currentNote = 0;
        activeMelody = variations[Random.Range(0, variations.Count)];
    }

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

    void Update()
    {
        time = Time.time;
    }

    float BPMConvert( float BPM )
    {
        BPM = BPM - 300;
        if (BPM < 0) return 1f;
        else
        {
            return Mathf.Clamp( 1f - (BPM / 1000), .1f, 1f ); 
        }
    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        for (int w = 0; w < waves.Count; ++w)
        {
            if ( time - startTimes[w] > ( TYPE_TIMES[(int)types[w]] / BPMConvert( Conductor._BPM ) ) )
            {
                startTimes.RemoveAt(w);
                types.RemoveAt(w);
                waves.RemoveAt(w);
                w--;

                continue;
            }

            Wave wave = waves[w];
            float t = (time - startTimes[w]) / (TYPE_TIMES[(int)types[w]] / BPMConvert(Conductor._BPM));

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