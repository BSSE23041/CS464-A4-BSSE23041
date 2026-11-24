using System.IO;
using UnityEditor;
using UnityEngine;

// I made all the sounds for the game with this instead of downloading them.
// it mixes simple waves (sine, square, triangle, saw and noise) and saves .wav files
// into Assets/Audio. run it from Tools > Make Sounds
public static class SoundMaker
{
    const int Rate = 22050;
    const string Folder = "Assets/Audio";

    [MenuItem("Tools/Make Sounds")]
    public static void MakeAll()
    {
        Directory.CreateDirectory(Folder);

        Save("pickup", Pickup());
        Save("hit", Hit());
        Save("click", Click());
        Save("win", Win());
        Save("lose", Lose());
        Save("hazard_hum", HazardHum());
        Save("goal_chime", GoalChime());
        Save("music", Music());

        AssetDatabase.Refresh();
        Debug.Log("Sounds made in " + Folder);
    }

    // basic waves

    static float Sine(float f, float t) { return Mathf.Sin(2f * Mathf.PI * f * t); }
    static float Square(float f, float t) { return Sine(f, t) >= 0f ? 1f : -1f; }
    static float Triangle(float f, float t) { float p = f * t % 1f; return 4f * Mathf.Abs(p - 0.5f) - 1f; }
    static float Saw(float f, float t) { float p = f * t % 1f; return 2f * p - 1f; }

    static float Note(int midi) { return 440f * Mathf.Pow(2f, (midi - 69) / 12f); }

    // adds one note to the sound, fades in quickly and fades out at the end
    static void AddNote(float[] buf, float start, float length, float freq, float volume, int wave)
    {
        int s0 = (int)(start * Rate);
        int count = (int)(length * Rate);
        for (int i = 0; i < count && s0 + i < buf.Length; i++)
        {
            float t = (float)i / Rate;
            float attack = Mathf.Clamp01(t / 0.01f);
            float release = Mathf.Clamp01((length - t) / 0.05f);
            float decay = Mathf.Exp(-2.5f * t / length);
            float v;
            if (wave == 0) v = Sine(freq, t);
            else if (wave == 1) v = Square(freq, t) * 0.5f;
            else v = Triangle(freq, t);
            buf[s0 + i] += v * volume * attack * release * decay;
        }
    }

    // the actual sounds

    static float[] Pickup()
    {
        float[] b = new float[(int)(0.22f * Rate)];
        AddNote(b, 0f, 0.08f, Note(83), 0.5f, 1);
        AddNote(b, 0.07f, 0.15f, Note(88), 0.5f, 1);
        return b;
    }

    static float[] Hit()
    {
        float[] b = new float[(int)(0.3f * Rate)];
        System.Random rnd = new System.Random(1);
        for (int i = 0; i < b.Length; i++)
        {
            float t = (float)i / Rate;
            float env = Mathf.Exp(-12f * t);
            float freq = Mathf.Lerp(220f, 60f, t / 0.3f);
            float noise = (float)(rnd.NextDouble() * 2.0 - 1.0);
            b[i] = (Square(freq, t) * 0.4f + noise * 0.5f) * env * 0.8f;
        }
        return b;
    }

    static float[] Click()
    {
        float[] b = new float[(int)(0.07f * Rate)];
        for (int i = 0; i < b.Length; i++)
        {
            float t = (float)i / Rate;
            b[i] = Sine(1200f, t) * Mathf.Exp(-60f * t) * 0.6f;
        }
        return b;
    }

    static float[] Win()
    {
        float[] b = new float[(int)(0.9f * Rate)];
        int[] notes = { 72, 76, 79, 84 };
        for (int i = 0; i < notes.Length; i++)
        {
            float len = i == notes.Length - 1 ? 0.45f : 0.14f;
            AddNote(b, i * 0.12f, len, Note(notes[i]), 0.55f, 2);
            AddNote(b, i * 0.12f, len, Note(notes[i] + 12), 0.15f, 0);
        }
        return b;
    }

    static float[] Lose()
    {
        float[] b = new float[(int)(1.1f * Rate)];
        int[] notes = { 67, 64, 60, 48 };
        for (int i = 0; i < notes.Length; i++)
        {
            float len = i == notes.Length - 1 ? 0.5f : 0.2f;
            AddNote(b, i * 0.2f, len, Note(notes[i]), 0.45f, 1);
        }
        return b;
    }

    // 1 second buzz for the hazards. whole number of cycles so the loop doesn't click
    static float[] HazardHum()
    {
        float[] b = new float[Rate];
        for (int i = 0; i < b.Length; i++)
        {
            float t = (float)i / Rate;
            float wobble = 0.65f + 0.35f * Sine(8f, t);
            b[i] = (Saw(110f, t) * 0.5f + Sine(55f, t) * 0.4f) * wobble * 0.6f;
        }
        return b;
    }

    // 2 second chime for the goal, pulses twice a second
    static float[] GoalChime()
    {
        float[] b = new float[Rate * 2];
        for (int i = 0; i < b.Length; i++)
        {
            float t = (float)i / Rate;
            float pulse = 0.55f + 0.45f * Sine(2f, t);
            b[i] = (Sine(523.5f, t) * 0.5f + Sine(784f, t) * 0.35f + Sine(1047f, t) * 0.15f) * pulse * 0.6f;
        }
        return b;
    }

    // background music: Am F C G chords, bass + arpeggio, 110 bpm, 8 bars then it loops
    static float[] Music()
    {
        float beat = 60f / 110f;
        float eighth = beat / 2f;
        int bars = 8;
        float[] b = new float[(int)(bars * 4 * beat * Rate)];

        int[][] chords =
        {
            new[] { 57, 60, 64 }, // A minor
            new[] { 53, 57, 60 }, // F
            new[] { 48, 52, 55 }, // C
            new[] { 55, 59, 62 }, // G
        };
        int[] patternA = { 0, 1, 2, 1, 0, 1, 2, 1 };
        int[] patternB = { 2, 1, 0, 1, 2, 2, 1, 0 };

        for (int bar = 0; bar < bars; bar++)
        {
            int[] chord = chords[bar % 4];
            float barStart = bar * 4 * beat;

            // bass note on every beat
            for (int q = 0; q < 4; q++)
                AddNote(b, barStart + q * beat, beat * 0.9f, Note(chord[0] - 12), 0.28f, 2);

            // arpeggio one octave higher
            int[] pattern = bar < 4 ? patternA : patternB;
            for (int e = 0; e < 8; e++)
                AddNote(b, barStart + e * eighth, eighth * 0.9f, Note(chord[pattern[e]] + 12), 0.12f, 1);

            // long quiet note under it
            AddNote(b, barStart, 4 * beat * 0.95f, Note(chord[2] + 12), 0.08f, 0);
        }
        return b;
    }

    // writes a 16 bit mono wav file

    static void Save(string name, float[] samples)
    {
        string path = Path.Combine(Folder, name + ".wav");
        using (BinaryWriter w = new BinaryWriter(File.Create(path)))
        {
            int dataSize = samples.Length * 2;
            w.Write(System.Text.Encoding.ASCII.GetBytes("RIFF"));
            w.Write(36 + dataSize);
            w.Write(System.Text.Encoding.ASCII.GetBytes("WAVE"));
            w.Write(System.Text.Encoding.ASCII.GetBytes("fmt "));
            w.Write(16);
            w.Write((short)1);      // PCM
            w.Write((short)1);      // mono
            w.Write(Rate);
            w.Write(Rate * 2);      // bytes per second
            w.Write((short)2);      // block align
            w.Write((short)16);     // bits
            w.Write(System.Text.Encoding.ASCII.GetBytes("data"));
            w.Write(dataSize);
            foreach (float s in samples)
                w.Write((short)(Mathf.Clamp(s, -1f, 1f) * 32000f));
        }
    }
}
