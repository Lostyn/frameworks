using UnityEngine;

public static class FloatExtensions
{
    public static bool Within(this float v, float min, float max)
    {
        return v >= min && v <= max;
    }

    public static float Magnitude(this float v)
    {
        return Mathf.Abs(v);
    }

    public static float Snap(this float v, int snapValue)
    {
        return Mathf.Round(v / snapValue) * snapValue;
    }

    public static float Remap(this float v, float iMin, float iMax, float oMin, float oMax)
    {
        float t = (v - iMin) / (iMax - iMin);
        return oMax * t + oMin * (1f - t);
    }

    public static float Frac(this float v) => v - Mathf.Floor(v);
}
