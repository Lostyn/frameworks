using System;
using UnityEngine;

[Serializable]
public class TriColor {
    private const string UNNAMED_COLOR = "Unnamed Color";

    public string ColorName;
    public Color Light = new Color().FromHex("FFFFFF");
    public Color Normal = new Color().FromHex("9e9e9e");
    public Color Dark = new Color().FromHex("607d8b");

    public TriColor() { 
        ColorName = UNNAMED_COLOR;
    }

    public TriColor(string colorName, Color normal) {
        ColorName = colorName;
        Light = normal.Lighter();
        Normal = normal;
        Dark = normal.Darker();
    }

    public TriColor(string colorName, Color light, Color normal, Color dark)
    {
        ColorName = colorName;
        Light = light;
        Normal = normal;
        Dark = dark;
    }
}