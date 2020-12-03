using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    public static float RemapToNumberRange(this float value, float from, float to, float newFrom, float newTo)
    {
        return (value - from) / (to - from) * (newTo - newFrom) + newFrom;
    }

    public static bool Approximately(this Vector3 thisVector, Vector3 otherVector)
    {
        return
            Mathf.Approximately(thisVector.x, otherVector.x) &&
            Mathf.Approximately(thisVector.y, otherVector.y) &&
            Mathf.Approximately(thisVector.z, otherVector.z);
    }

    public static bool Approximately(this Vector2 thisVector, Vector2 otherVector)
    {
        return
            Mathf.Approximately(thisVector.x, otherVector.x) &&
            Mathf.Approximately(thisVector.y, otherVector.y);
    }
    public static bool ApproximatelyY(this Vector2 thisVector, Vector2 otherVector)
    {
        return Mathf.Approximately(thisVector.y, otherVector.y);
    }
    public static bool Approximately(this float thisFloat, float otherFloat)
    {
        return
            Mathf.Approximately(thisFloat, otherFloat);
    }
}
