using UnityEngine;

// contrast ratio checker (WCAG formula from the lecture)
// I used this to check my text colours against the panel colour
public static class ColourUtil
{
    static float Channel(float c)
    {
        return c <= 0.03928f ? c / 12.92f : Mathf.Pow((c + 0.055f) / 1.055f, 2.4f);
    }

    public static float Luminance(Color c)
    {
        return 0.2126f * Channel(c.r) + 0.7152f * Channel(c.g) + 0.0722f * Channel(c.b);
    }

    public static float Contrast(Color a, Color b)
    {
        float la = Luminance(a);
        float lb = Luminance(b);
        float light = Mathf.Max(la, lb);
        float dark = Mathf.Min(la, lb);
        return (light + 0.05f) / (dark + 0.05f);
    }
}
