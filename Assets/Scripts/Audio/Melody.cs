using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Melody : WavePlayer
{
    public Wave waveSettings;
    public List<string[]> variations;
    public string[] activeMelody;
    int currentNote = 0;
    
    public void SetNotes( List<string> data )
    {
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
    }
	
 //IEnumerator RunMelody()
 //   {
 //       if (melody.Count == 0) yield break;

 //       float timer = 0;
 //       //play first note
 //       DoNote(melody[0]);
 //       while( Application.isPlaying )
 //       {
 //           timer += Time.deltaTime;
 //           if (timer > beatTime)
 //           {
 //               timer -= beatTime;
 //               //change note in the melody (or go silent)
 //               currentNote = ++currentNote % melody.Count;
 //               DoNote(melody[currentNote]);
 //           }
 //           yield return null;
 //       }

 //       yield return null;
 //   }

    public void DoNote()
    {
        if (activeMelody == null) SelectVariation();

        string note = activeMelody[currentNote];
        if (string.IsNullOrEmpty(note) || note == "x" )
        {
            activeWave = null;
        }
        else
        {
            float f = NoteConverter.getFreq(note);
            if (activeWave != null && activeWave.frequency == f) return; //keep playing
            else
            {
                activeWave = new Wave();
                activeWave.CopyFrom(waveSettings);
                activeWave.frequency = f;
            }
        }

        if ( ++currentNote == activeMelody.Length ) SelectVariation();
    }

    void SelectVariation()
    {
        currentNote = 0;
        activeMelody = variations[Random.Range(0, variations.Count)];
    }
}
