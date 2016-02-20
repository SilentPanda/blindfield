using UnityEngine;
using System.Collections;

public static class NoteConverter
{
    //function for calculating frequency offset
    //fn = f0 * (x)n
    //x = pow( 2, ( 1.0 / 12.0 ) )
    static float x = Mathf.Pow(2, ( 1.0f / 12.0f ) );

    //f0 = A4 = 440 Hz

    public static float getFreq(string note)
    {
        return freqForSteps(calcHalfSteps(note));
    }

    //freq relative to A4 from given half step distance
    static float freqForSteps( int halfStepsFromA4 )
    {
        return 440 * Mathf.Pow(x, halfStepsFromA4);
    }

    //calculates half steps from A4 for a given note
    static int calcHalfSteps( string note )
    {
        //valid notes are things like C1, A4, G#5, Gb5
        //so they are either 2 or 3 chars
        if ( note.Length < 2 || note.Length > 3) return 0;

        char b = note[0];
        int offset = 0;
        int octaveSteps = 0;
        if (note.Length == 3)
        {
            if (note[1] == '#') offset = 1;
            else if (note[1] == 'b') offset = -1;
            octaveSteps = (int.Parse(note[2].ToString()) - 4) * 12;
        }
        else
        {
            octaveSteps = (int.Parse(note[1].ToString()) - 4) * 12;
        }
            return distanceFromA(b) + offset + octaveSteps;
    }

    //maybe there's some way to calculate this, but this is pretty simple...
    static int distanceFromA(char note)
    {
        switch (note)
        {
            case 'a': return 0;
            case 'b': return 2;
            case 'c': return 3;
            case 'd': return 5;
            case 'e': return 7;
            case 'f': return 8;
            case 'g': return 10;
        }
        return 0;
    }
}
