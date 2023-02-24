
using System.Globalization;
using UnityEngine;


public static class ColorExtensions 
{
    private const float LIGHT_OFFSET = 0.0625f;

    /// <summary>
    ///     Returns a new color from a hex value
    /// </summary>
    /// <param name="color"> The color</param>
    /// <param name="hexValue"> FFFFFF or #FFFFFF or #FFFFFFFF</param>
    /// <param name="alpha"> Custom alpha (default: 1) </param>
    /// <returns></returns>
    public static Color FromHex(this Color color, string hexValue, float alpha = 1)
    {
        if (string.IsNullOrEmpty(hexValue)) return Color.clear;

        if (hexValue[0] == '#') hexValue = hexValue.TrimStart('#');
        if (hexValue.Length > 6) hexValue = hexValue.Remove(6, hexValue.Length - 6);

        int value = int.Parse(hexValue, NumberStyles.HexNumber);
        int r = value >> 16 & 255;
        int g = value >> 8 & 255;
        int b = value & 255;
        float a = 255 * alpha;

        return new Color().ColorFrom255(r, g, b, a);
    }

    /// <summary>
    ///     Returns a new Color with the given settings.
    /// </summary>
    /// <param name="color">The Color.</param>
    /// <param name="r">red</param>
    /// <param name="g">green</param>
    /// <param name="b">blue</param>
    /// <param name="a">alpha</param>
    /// <returns>The new Color.</returns>
    public static Color ColorFrom255(this Color color, float r, float g, float b, float a = 255) { return new Color(r / 255f, g / 255f, b / 255f, a / 255f); }

    /// <summary>
    ///     Returns a Color lighter than the given color.
    /// </summary>
    /// <param name="color">The Color.</param>
    /// <returns>The new Color.</returns>
    public static Color Lighter(this Color color)
    {
        return new Color(
            color.r + LIGHT_OFFSET,
            color.g + LIGHT_OFFSET,
            color.b + LIGHT_OFFSET,
            color.a);
    }

    /// <summary>
    ///     Returns a Color darker than the given color.
    /// </summary>
    /// <param name="color">The Color.</param>
    /// <returns>The new Color.</returns>
    public static Color Darker(this Color color)
    {
        return new Color(
            color.r - LIGHT_OFFSET,
            color.g - LIGHT_OFFSET,
            color.b - LIGHT_OFFSET,
            color.a);
    }

    /// <summary>
    ///     Returns a new Color with the same settings and a new alpha.
    /// </summary>
    /// <param name="color">The Color.</param>
    /// <param name="alpha">Alpha for the Color.</param>
    /// <returns></returns>
    public static Color WithAlpha(this Color color, float alpha) { return new Color(color.r, color.g, color.b, alpha); }
}