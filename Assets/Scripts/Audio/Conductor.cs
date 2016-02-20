using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Song
{
    public List<string> melody = new List<string>();
    public List<string> synth = new List<string>();
    public List<string> drums = new List<string>();
}

public class Conductor : MonoBehaviour
{
    public static int beat = 0; 

    List<Song> songs;
    public float BPM = 140;

    Song activeSong;
    bool running = false;

    float t = 0;
    float rBPM;

    Drums drums;
    Synth synth;
    Melody melody;

	// Use this for initialization
	IEnumerator Start ()
    {
        rBPM = 60f / BPM;
        songs = new List<Song>();

        drums = GetComponentInChildren<Drums>();
        synth = GetComponentInChildren<Synth>();
        melody = GetComponentInChildren<Melody>();

        //load all songs
        int count = 1;
        bool run = true;
        while (run)
        {
            string path = Path.Combine(Application.streamingAssetsPath, count.ToString() + ".txt");

            if ( File.Exists( path ) )
            {
                string[] lines = File.ReadAllLines(path);
                Song newSong = new Song();
                
                for( int i = 0; i < lines.Length; ++i )
                {
                    if ( lines[i][0] == '_' )
                    {
                        int x = i + 1;
                        while(lines[x][0] != '_')
                        {
                            switch( lines[i] )
                            {
                                case "_synth":
                                    newSong.synth.Add(lines[x]);
                                    break;
                                case "_melody":
                                    newSong.melody.Add(lines[x]);
                                    break;
                                case "_drums":
                                    newSong.drums.Add(lines[x]);
                                    break;
                            }
                            x++;
                        }
                    }
                }
            }
            else
            {
                run = false;
            }

            count++;
            yield return null;
        }

        if (songs.Count == 0)
        {
            enabled = false;
            yield break;
        }

        Song s = SelectRandomSong();
        DoSong(s);
    }

    Song SelectRandomSong()
    {
        return songs[Random.Range(0, songs.Count)];
    }

    void DoSong( Song s )
    {
        activeSong = s;
        melody.SetNotes(s.melody);
        running = true;
        beat = 0;
    }

    void FixedUpdate()
    {
        t += Time.fixedDeltaTime;
        if ( t > rBPM )
        {
            melody.DoNote();
            //synth.DoNote();
            //drums.DoNote();
        }
    }
}
