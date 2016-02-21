using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Melody : WavePlayer
{
    static List<int[]> arpeggios = new List<int[]>()
    {
        new int[] { 0, 3, 5, 7, 5, 3 },
        new int[] { 0, -1, -4, -7, -4, -1 },
        new int[] { 0, 3, 5, 7 },
        new int[] { 0, -1, -4, -7},
        new int[] { 0, 3, 7 },
        new int[] { 0, -4, -7 },
        new int[] { 0, -4, -7, -4 },
        new int[] { 0, 3, 7, 3 },
        new int[] { 0, -4, -1, -7, -1, -4 },
        new int[] { 0, 5, 3, 7, 3, 5 },
    };

    public Wave waveSettings;
    public List<string[]> variations;
    public string[] activeMelody;
    public bool enableArpeggios = true;
    int currentNote = 0;

    int[] activeArpeggio;
    string baseNote;
    float noteStart;
    
    public void SetNotes( List<string> data )
    {
        variations = new List<string[]>();

        //parse into variations
        for( int i = 0; i < data.Count; ++i )
        {
            variations.Add(data[i].Split(';'));
        }

        //check for doubled notes
        for( int i = 0; i < variations.Count; ++i )
        {
            for( int x = 0; x < variations[i].Length; ++x )
            {
                //double sign = copy previous, should never go wrong
                if ( variations[i][x] == "-" )
                {
                    variations[i][x] = variations[i][x - 1];
                }
            }
        }

        enabled = true;
    }

    public void DoNote()
    {
        if (activeMelody == null || currentNote >= activeMelody.Length ) SelectVariation();

        string note = activeMelody[currentNote];
        if (string.IsNullOrEmpty(note) || note == "x" )
        {
            activeWave.gain = 0;
        }
        else
        {
            float f = NoteConverter.getFreq(note);
            //if (activeWave.frequency == f)
            //{
            //    currentNote++;
            //    return; //keep playing
            //}
            //else
            //{
                activeWave.CopyFrom(waveSettings);
                activeWave.frequency = f;
                activeWave.note = note;
                activeArpeggio = null;

                if ( Conductor.NoteInActiveKey(note) && enableArpeggios )
                {
                    if (Random.value < .5f)
                    {
                        SelectArpeggio();
                        baseNote = note;
                        noteStart = Time.time;
                    }
                }
            //}
        }

        if ( ++currentNote >= activeMelody.Length ) SelectVariation();
    }

    protected override void OnAudioFilterRead(float[] data, int channels)
    {   
        //if we're doing an arpeggio
        if ( activeArpeggio != null )
        {
            //calculate this note's index
            float rBPM = 60f / Conductor._BPM;
            float t = (time - noteStart) / rBPM;
            int index = (int)Mathf.Lerp(0, activeArpeggio.Length, t);
            if (index == activeArpeggio.Length) index--;

            activeWave.note = Conductor.GetRelativeNoteInKey(Conductor.activeKey, baseNote, activeArpeggio[index]);
            activeWave.frequency = NoteConverter.getFreq(activeWave.note);
        }
        

        base.OnAudioFilterRead(data, channels);
    }

    void SelectArpeggio()
    {
        List<int[]> possibles = new List<int[]>();
        for( int i = 0; i < arpeggios.Count; ++i )
        {
            //compare arpeggio length to BPM, too long will not be cool to play at high speeds
            if ( arpeggios[i].Length * Conductor._BPM < 650 )
            {
                possibles.Add(arpeggios[i]);
            }
        }
        
        if (possibles.Count == 0) return;
        int r = Random.Range(0, possibles.Count);
        activeArpeggio = possibles[r];
    }

    void SelectVariation()
    {
        if (variations == null)
        {
            Debug.LogError("No variations for melody!");
            return;
        }
        currentNote = 0;
        activeMelody = variations[Random.Range(0, variations.Count)];
    }
}
