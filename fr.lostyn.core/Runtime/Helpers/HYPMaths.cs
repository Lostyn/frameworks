using UnityEngine;

public static class HYPMaths
{
    /// <summary>
    /// Move from "from" to "to" by the specified amount and returns the corresponding value
    /// </summary>
    /// <returns>The approach.</returns>
    /// <param name="from">From.</param>
    /// <param name="to">To.</param>
    /// <param name="amount">Amount.</param>
    public static float Approach(float from, float to, float amount)
    {
        if (from < to)
        {
            from += amount;
            if (from > to) return to;
        }
        else
        {
            from -= amount;
            if (from < to) return to;
        }

        return from;
    }

    /// <summary>
    /// Get position on Arc 
    /// </summary>
    /// <param name="angle"></param>
    /// <param name="radius"></param>
    /// <returns></returns>
    public static Vector2 GetArcPoint(float angle, float radius) {
        return new Vector2(
            radius * Mathf.Cos( angle ),
            radius * Mathf.Sin( angle )
        );
    }

        /// <summary>
    /// 
    /// </summary>
    /// <param name="polar">value over 360°</param>
    /// <param name="helevation">value orver 180°</param>
    /// <param name="radius"></param>
    /// <returns></returns>
    public static Vector3 GetPointOnSphere(float polar, float helevation, float radius) {
        polar = polar * Mathf.Deg2Rad;
        helevation = helevation * Mathf.Deg2Rad;
        return new Vector3(
            radius * Mathf.Sin(polar) * Mathf.Sin(helevation),
            radius * Mathf.Cos(helevation),  
            radius * Mathf.Cos(polar) * Mathf.Sin(helevation)
        );
    }
}
