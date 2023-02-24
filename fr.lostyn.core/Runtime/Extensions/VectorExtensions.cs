using System.Collections.Generic;
using UnityEngine;

public static class VectorExtensions
{
    public static Vector3 InvertedY(this Vector3 vector)
    {
        return Vector3.Scale(vector, new Vector3(1, -1));
    }

    public static Vector2 InvertedY(this Vector2 vector)
    {
        return Vector2.Scale(vector, new Vector2(1, -1));
    }

    /// <summary>
    /// Finds the position closest to the given one.
    /// </summary>
    /// <param name="position">World position.</param>
    /// <param name="otherPositions">Other world positions.</param>
    /// <returns>Closest position.</returns>
    public static Vector3 GetClosest(this Vector3 position, IEnumerable<Vector3> otherPositions)
    {
        var closest = Vector3.zero;
        var shortestDistance = Mathf.Infinity;

        foreach (var otherPosition in otherPositions)
        {
            var distance = (position - otherPosition).sqrMagnitude;

            if (distance < shortestDistance)
            {
                closest = otherPosition;
                shortestDistance = distance;
            }
        }

        return closest;
    }
}
