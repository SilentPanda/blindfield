using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Audio;

public delegate void BeatEvent();

public class Song
{
    public List<string> melody = new List<string>();
    public List<string> synth = new List<string>();
    public List<string> drums = new List<string>();
}

public class Conductor : MonoBehaviour
{
    public enum MusicalKey
    {
        BMinor = 0,
        BMajor,
    }

    public static Dictionary<MusicalKey, List<string>> keys = new Dictionary<MusicalKey, List<string>>
    {
        {  MusicalKey.BMinor, new List<string> { "b", "c#", "d", "e", "f#", "g", "a" } },
        {  MusicalKey.BMajor, new List<string> { "b", "c#", "d#", "e", "f#", "g#", "a#" } },
    };

	public static BeatEvent onBeat;

    public static int beat = 0;
    public static MusicalKey activeKey = MusicalKey.BMinor;

    public static string GetRelativeNoteInKey( MusicalKey key, string note, int distance )
    {
        if (string.IsNullOrEmpty(note)) return note;

        int octave = int.Parse( note[ note.Length - 1 ].ToString() );
        string checkNote = note.Substring(0, note.Length - 1);
        string returnNote = checkNote;
        if ( keys.ContainsKey( key ) )
        {
            List<string> notes = keys[key];
            if ( notes.Contains( checkNote ) )
            {
                int index = notes.IndexOf(checkNote);
                //remove octaves first (up or down)
                while( distance > 7 )
                {
                    octave++;
                    distance -= 7;
                }
                while( distance < -7 )
                {
                    octave--;
                    distance += 7;
                }
                //so we're within one octave now, either up or down
                //beyond upper limit
                if ( index + distance > notes.Count - 1 )
                {
                    octave++;
                    index = (index + distance) - notes.Count;
                    returnNote = notes[index];
                }
                //beyond lower limit
                else if ( index + distance < 0 )
                {
                    octave--;
                    index = (index + distance) + notes.Count;
                    returnNote = notes[index];
                }
                //in range
                else
                {
                    returnNote = notes[index + distance];
                }
            }
            else
            {
                Debug.LogError(string.Format("Note not in key {0} {1}", checkNote, key));
            }
        }
        else
        {
            Debug.LogError("Not implemented: " + key);
        }

        return returnNote + octave.ToString();
    }

    public static bool NoteInActiveKey( string note )
    {
        string checkNote = note.Substring(0, note.Length - 1);
        if (keys[activeKey].Contains(checkNote))
        {
            return true;
        }

        return false;
    }

    public static void Play()
    {
        instance.running = true;
        instance.on.TransitionTo(1f);
    }

    public static void Stop()
    {
        instance.running = false;
        instance.off.TransitionTo(3f);
    }
    
    static Conductor instance;
    public static float _BPM
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<Conductor>();
            if (instance == null) return 300;
            else return instance.BPM;
        }
    }

    public AudioMixer mixer;
    public AudioMixerSnapshot on, off;

    List<Song> songs;
    public float BPM = 140;
    float oldBPM = 140;

    Song activeSong;
    bool running = false;

    float t = 0;
    float rBPM;

    Drums drums;
    Synth synth;
    Melody melody;

	// Use this for initialization
	void Awake ()
    {
		instance = this;

        rBPM = 60f / BPM;
        oldBPM = BPM;
        songs = new List<Song>();

        drums = GetComponentInChildren<Drums>();
        synth = GetComponentInChildren<Synth>();
        melody = GetComponentInChildren<Melody>();

        //load all songs
        int count = 1;
        bool run = true;
        while (run)
        {
            string path = Path.Combine(Application.streamingAssetsPath, "Songs/" + count.ToString() + ".txt");

            if ( File.Exists( path ) )
            {
                string[] lines = File.ReadAllLines(path);
                Song newSong = new Song();
                
                for( int i = 0; i < lines.Length; ++i )
                {
                    if (lines[i].Length == 0) break;

                    if ( lines[i][0] == '_' )
                    {
                        int x = i + 1;
                        while( x < lines.Length && lines[x].Length > 0 && lines[x][0] != '_')
                        {
                            switch( lines[i] )
                            {
                                case "_synth":
                                    newSong.synth.Add(lines[x]);
                                    //Debug.Log("Added synth");
                                    break;
                                case "_melody":
                                    newSong.melody.Add(lines[x]);
                                    //Debug.Log("Added melody");
                                    break;
                                case "_drums":
                                    newSong.drums.Add(lines[x]);
                                    //Debug.Log("Added drums");
                                    break;
                            }
                            x++;
                        }
                    }
                }

                songs.Add(newSong);
            }
            else
            {
                run = false;
            }

            count++;
        }

        if (songs.Count == 0)
        {
            enabled = false;
			return;
        }

        Song s = SelectRandomSong();
        DoSong(s);
    }

    Song SelectRandomSong()
    {
		ForceSongInLevel fsil = FindObjectOfType<ForceSongInLevel> ();
		if (FindObjectOfType<ForceSongInLevel> ()) 
		{
			int index = int.Parse (fsil.songName) - 1;
			return songs [index];
		}

        return songs[Random.Range(0, songs.Count)];
    }

    void DoSong( Song s )
    {
        activeSong = s;
        melody.SetNotes(s.melody);
        drums.SetNotes(s.drums);
        synth.SetNotes(s.synth);
        beat = 0;

        Play();
    }

    void FixedUpdate()
    {
        if (running)
        {
            t += Time.fixedDeltaTime;
            if (t > rBPM)
            {
				if (onBeat != null)
					onBeat ();
				
                melody.DoNote();
                synth.DoNote();
                drums.DoNote();
                t -= rBPM;

                if (BPM != oldBPM)
                {
                    rBPM = 60f / BPM;
                    oldBPM = BPM;
                }
            }
        }
    }
}
