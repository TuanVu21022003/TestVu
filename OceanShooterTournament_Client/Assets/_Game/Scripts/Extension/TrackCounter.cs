using UnityEngine;

public class TrackCounter
{
    public static string BuildKey(object type)
    {
        return $"eventCount.{type}";
    }

    public static int OnCounter(object type)
    {
        int count = GetCounter(type);
        count++;
        PlayerPrefs.SetInt(BuildKey(type), count);
        return count;
    }

    public static void SetCounter(object type, int number)
    {
        PlayerPrefs.SetInt(BuildKey(type), number);
    }

    public static int GetCounter(object type)
    {
        return PlayerPrefs.GetInt(BuildKey(type));
    }
}