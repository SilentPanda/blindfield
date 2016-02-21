using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Melody : WavePlayer
{
    static int[] arpeggioUp = new int[] { 0, 3, 5, 7, 5, 3 };
    static int[] arpeggioDown = new int[] { 0, -1, -4, -7, -4, -1 };
    static int[] halfArpeggioUp = new int[] { 0, 3, 5, 7 };
    static int[] halfArpeggioDown = new int[] { 0, -1, -4, -7};

    public Wave waveSettings;
    public List<string[]> variations;
    public string[] activeMelody;
    int currentNote = 0;

    int[] activeArpeggio;
    string baseNote;
    
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
            activeWave = null;
        }
        else
        {
            float f = NoteConverter.getFreq(note);
            if (activeWave != null && activeWave.frequency == f)
            {
                currentNote++;
                return; //keep playing
            }
            else
            {
                activeWave = new Wave();
                activeWave.CopyFrom(waveSettings);
                activeWave.frequency = f;
                activeWave.note = note;
                activeArpeggio = null;

                if (Conductor._BPM <= 160 && Conductor.NoteInActiveKey(note) )
                {
                    if (Random.value < .2f)
                    {
                        SelectArpeggio();
                        baseNote = note;
                    }
                }
            }
        }

        if ( ++currentNote >= activeMelody.Length ) SelectVariation();
    }

    protected override void OnAudioFilterRead(float[] data, int channels)
    {
        /*
        //if we're doing an arpeggio
        if ( activeArpeggio != null )
        {
            //calculate this note's index
            float t = 
        }
        */

        base.OnAudioFilterRead(data, channels);
    }

    void SelectArpeggio()
    {
        int r = Random.Range(0, 4);
        switch( r )
        {
            case 0:
                activeArpeggio = arpeggioUp;
                break;
            case 1:
                activeArpeggio = arpeggioDown;
                break;
            case 2:
                activeArpeggio = halfArpeggioUp;
                break;
            default:
                activeArpeggio = halfArpeggioDown;
                break;
        }
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
