using System;
using UnityEngine;

public static class IntExtension
{
    public static string ToMMSS(this int seconds)
    {
        return string.Format("{0}:{1}",
            Mathf.Floor(seconds / 60).ToString("00"),
            (seconds % 60).ToString("00")
        );
    }

    public static string ToHHMMSS(this int seconds)
    {
        TimeSpan t = TimeSpan.FromSeconds(seconds);
        return t.ToString(@"hh\:mm\:ss");
    }

    public static string toChrono(this int milliseconds) {
        int minutes = milliseconds / 60000;
        int seconds = (milliseconds / 1000) % 60;
        int milli = milliseconds % 1000;
        return string.Format("{0:D2}:{1:D2}.{2:D3}", minutes, seconds, milli);
    }

}
