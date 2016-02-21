using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Synth : WavePlayer
{
    public Wave waveSettings;
    public List<string[]> variations;
    public string[] activeMelody;
    int currentNote = 0;

    Wave one, two;

    public void SetNotes(List<string> data)
    {
        variations = new List<string[]>();

        //parse into variations
        for (int i = 0; i < data.Count; ++i)
        {
            variations.Add(data[i].Split(';'));
        }

        //check for doubled notes
        for (int i = 0; i < variations.Count; ++i)
        {
            for (int x = 0; x < variations[i].Length; ++x)
            {
                //double sign = copy previous, should never go wrong
                if (variations[i][x] == "-")
                {
                    variations[i][x] = variations[i][x - 1];
                }
            }
        }

        one = new Wave();
        two = new Wave();

        enabled = true;
    }
    
    public void DoNote()
    {
        if (activeMelody == null || currentNote >= activeMelody.Length) SelectVariation();

        string note = activeMelody[currentNote];
        if (string.IsNullOrEmpty(note) || note == "x")
        {
            activeWave.gain = 0;
        }
        else
        {
            float f = NoteConverter.getFreq(note);
            //string firstNote = Conductor.GetRelativeNoteInKey(Conductor.activeKey, note, 2 * 7);
            //string secondNote = Conductor.GetRelativeNoteInKey(Conductor.activeKey, note, 2 * 7 + 2);
            //Debug.Log(string.Format("Overtones: {0} , {1} from base note {2}", firstNote, secondNote, note));
            //if (activeWave.frequency == f)
            //{
            //    currentNote++;
            //    activeWave.note = note;
            //    return; //keep playing
            //}
            //else
            //{
                activeWave.CopyFrom(waveSettings);
                activeWave.frequency = f;
                activeWave.note = note;
            //}
        }

        if (++currentNote >= activeMelody.Length) SelectVariation();
    }

    protected override void OnAudioFilterRead(float[] data, int channels)
    {
        if (activeWave.gain == 0) return;

        if (!string.IsNullOrEmpty(activeWave.note))
        {
            activeWave.frequency = NoteConverter.getFreq(activeWave.note);
        }

        activeWave.increment = activeWave.frequency * 2 * Mathf.PI / GlobalSoundVariables.SAMPLING_FREQUENCY;
        for (var i = 0; i < data.Length; i = i + channels)
        {
            activeWave.phase = activeWave.phase + activeWave.increment;
            // this is where we copy audio data to make them “available” to Unity
            float targetGain = (activeWave.gain + Mathf.Sin(activeWave.gainPhaseSpeed * time) * activeWave.gainPhaseRange);
            data[i] += (float)(targetGain) * Mathf.Sin(activeWave.phase);
            if (activeWave.square) data[i] = (data[i] > 0) ? targetGain : -targetGain;
            // if we have stereo, we copy the mono data to each channel
            if (channels == 2) data[i + 1] = data[i];
            if (activeWave.phase > 2 * Mathf.PI) activeWave.phase = 0;
        }

        //add overtones
        one.CopyFrom(activeWave);

        //add a double octave
        one.note = Conductor.GetRelativeNoteInKey(Conductor.activeKey, one.note, 2 * 7);

        one.square = false;
        one.gainPhaseSpeed = 10;
        one.gainPhaseRange = one.gain * .5f;

        one.frequency = NoteConverter.getFreq(one.note);
        one.increment = one.frequency * 2 * Mathf.PI / GlobalSoundVariables.SAMPLING_FREQUENCY;
        for (var i = 0; i < data.Length; i = i + channels)
        {
            one.phase = one.phase + one.increment;
            // this is where we copy audio data to make them “available” to Unity
            float targetGain = (one.gain + Mathf.Sin(one.gainPhaseSpeed * time) * one.gainPhaseRange);
            data[i] += (float)(targetGain) * Mathf.Sin(one.phase);
            if (one.square) data[i] = (data[i] > 0) ? targetGain : -targetGain;
            // if we have stereo, we copy the mono data to each channel
            if (channels == 2) data[i + 1] = data[i];
            if (one.phase > 2 * Mathf.PI) one.phase = 0;
        }
        
        //add a double octave third?
        two.CopyFrom(activeWave);
        two.note = Conductor.GetRelativeNoteInKey(Conductor.activeKey, two.note, 2 * 7 + 2);
        if ( int.Parse( two.note[two.note.Length-1].ToString() ) > 4 )
        {
            two.note = two.note.Substring(0, two.note.Length - 1) + "4";
        }
        two.square = false;
        two.gain *= 2;
        two.gainPhaseSpeed = 1;
        two.gainPhaseRange = two.gain * .5f;
        two.frequency = NoteConverter.getFreq(two.note);
        two.increment = two.frequency * 2 * Mathf.PI / GlobalSoundVariables.SAMPLING_FREQUENCY;
        for (var i = 0; i < data.Length; i = i + channels)
        {
            two.phase = two.phase + two.increment;
            // this is where we copy audio data to make them “available” to Unity
            float targetGain = (two.gain + Mathf.Sin(two.gainPhaseSpeed * time) * two.gainPhaseRange);
            data[i] += (float)(targetGain) * Mathf.Sin(two.phase);
            if (two.square) data[i] = (data[i] > 0) ? targetGain : -targetGain;
            // if we have stereo, we copy the mono data to each channel
            if (channels == 2) data[i + 1] = data[i];
            if (two.phase > 2 * Mathf.PI) two.phase = 0;
        }
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
}
