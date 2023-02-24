using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public static class StringExtension
{
    public static string RemoveAccent(this string str)
    {
        if (string.IsNullOrWhiteSpace(str))
            return str;

        str = str.Normalize(NormalizationForm.FormD);
        var chars = str.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();
        return new string(chars).Normalize(NormalizationForm.FormC);
    }

    public static string Simplify(this string str)
    {
        return Regex.Replace(str.RemoveAccent(), "[^a-zA-Z0-9]+", "", RegexOptions.Compiled);
    }

    public static int ToInt(this string str) {
        int value = 0;
        if (int.TryParse(str, out value)) {
            return value;
        }

        return 0;
    }
}
