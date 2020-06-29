using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StringExtensions {
    public static string Substring(this string str, string since, string until, bool excludeSince = true) {
        int startIndex = str.IndexOf(since);
        if (excludeSince) startIndex += since.Length;
        int length = str.IndexOf(until, startIndex) - startIndex;
        return str.Substring(startIndex, length);
    }
}
