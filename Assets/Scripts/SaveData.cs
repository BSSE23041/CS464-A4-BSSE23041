using UnityEngine;

// all the PlayerPrefs stuff is in here so the key names are only typed once
public static class SaveData
{
    public const int LevelCount = 5;

    const string UnlockedKey = "unlocked";
    const string BestKeyStart = "best";   // best1, best2 ... best5
    const string MusicVolumeKey = "MusicVolume";

    static string BestKey(int level)
    {
        return BestKeyStart + level;
    }

    // unlocked levels

    public static int GetUnlocked()
    {
        return PlayerPrefs.GetInt(UnlockedKey, 1); // level 1 is always open
    }

    public static bool IsUnlocked(int level)
    {
        return level <= GetUnlocked();
    }

    public static void Unlock(int level)
    {
        level = Mathf.Clamp(level, 1, LevelCount);
        if (level > GetUnlocked())
        {
            PlayerPrefs.SetInt(UnlockedKey, level);
            PlayerPrefs.Save();
        }
    }

    // best times

    public static bool HasBest(int level)
    {
        return PlayerPrefs.HasKey(BestKey(level));
    }

    public static float GetBest(int level)
    {
        return PlayerPrefs.GetFloat(BestKey(level), 0f);
    }

    // saves the time if there was no time before or if this one is faster
    // returns true when it's a new best
    public static bool Record(int level, float time)
    {
        if (HasBest(level) && time >= GetBest(level))
            return false;

        PlayerPrefs.SetFloat(BestKey(level), time);
        PlayerPrefs.Save();
        return true;
    }

    // music volume

    public static float GetMusicVolume()
    {
        return PlayerPrefs.GetFloat(MusicVolumeKey, 1f);
    }

    public static void SetMusicVolume(float value)
    {
        PlayerPrefs.SetFloat(MusicVolumeKey, value);
        PlayerPrefs.Save();
    }
}
