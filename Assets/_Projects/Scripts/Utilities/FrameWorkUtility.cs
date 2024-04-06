using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

public static class FrameWorkUtility
{
    #region Math
    public static float Sign(Vector2 p1, Vector2 p2, Vector2 p3)
    {
        return (p1.x - p3.x) * (p2.y - p3.y) - (p2.x - p3.x) * (p1.y - p3.y);
    }

    public static bool PointInTriangle(Vector2 pt, Vector2 v1, Vector2 v2, Vector2 v3)
    {
        var d1 = Sign(pt, v1, v2);
        var d2 = Sign(pt, v2, v3);
        var d3 = Sign(pt, v3, v1);

        var hasNeg = (d1 < 0) || (d2 < 0) || (d3 < 0);
        var hasPos = (d1 > 0) || (d2 > 0) || (d3 > 0);

        return !(hasNeg && hasPos);
    }
    public static Vector3 WrapRotation(Quaternion rot)
    {
        return new Vector3(WrapAngle(rot.eulerAngles.x), WrapAngle(rot.eulerAngles.y), WrapAngle(rot.eulerAngles.z));
    }

    private static float WrapAngle(float angle)
    {
        angle %= 360;
        if (angle > 180)
            return angle - 360;

        return angle;
    }

    public static float UnwrapAngle(float angle)
    {
        if (angle >= 0)
            return angle;

        angle = -angle % 360;

        return 360 - angle;
    }
    #endregion

    #region Convert
    public static long DateTimeToUnix(DateTime myDateTime)
    {
        TimeSpan timeSpan = myDateTime - new DateTime(1970, 1, 1, 0, 0, 0);

        return (long)timeSpan.TotalSeconds;
    }

    public static DateTime UnixTimeToDateTime(long unixTime)
    {
        System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        dtDateTime = dtDateTime.AddSeconds(unixTime).ToLocalTime();
        return dtDateTime;
    }

    public static TimeSpan GetTimeSpan(DateTime to, DateTime from)
    {
        var toDay = new DateTime(to.Year, to.Month, to.Day);
        var fromDay = new DateTime(from.Year, from.Month, from.Day);

        return toDay - fromDay;
    }

    public static string ToString(Int64 num)
    {
        if (num > 999999999 || num < -999999999)
        {
            num = num / 1000000000;
            num = num * 1000000000;
            return num.ToString("0,,,.#B", CultureInfo.InvariantCulture);
        }
        else if (num > 999999 || num < -999999)
        {
            num = num / 1000000;
            num = num * 1000000;
            return num.ToString("0,,.#M", CultureInfo.InvariantCulture);
        }
        // else if (num > 99999 || num < -99999)
        // {
        //     num = num / 1000;
        //     num = num * 1000;
        //     return num.ToString("0,.#K", CultureInfo.InvariantCulture);
        // }
        else if (num > 10000 || num < -10000)
        {
            num = num / 1000;
            num = num * 1000;
            return num.ToString("0,,#00", CultureInfo.InvariantCulture);
        }
        else
        {
            return num.ToString(CultureInfo.InvariantCulture);
        }
    }
    
    #endregion

    public static T PickRandom<T>(this IEnumerable<T> source)
    {
        return source.PickRandom(1).Single();
    }

    public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count)
    {
        return source.Shuffle().Take(count);
    }

    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
    {
        return source.OrderBy(x => Guid.NewGuid());
    }

    public static bool IsObjectVisible(this Camera @this, Renderer renderer)
    {
        return GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(@this), renderer.bounds);
    }

    public static void MultipleRayCastAll(ref List<RaycastHit2D> result, Vector2 original, Vector2 point1, Vector2 point2, float distance, int layer)
    {
        if (result == null)
            return;

        result.Clear();

        Vector2 diff = point2 - point1;
        Vector2 direction = diff.normalized;

        int maxIndex = (int)(diff.magnitude / distance);

        Vector2 diff2;
        RaycastHit2D[] hitResult;
        for (int i = 0; i <= maxIndex; i++)
        {
            Vector2 end = point1 + i * direction * distance;
            diff2 = end - original;
            hitResult = Physics2D.RaycastAll(original, diff2.normalized, diff2.magnitude, layer);

            foreach (RaycastHit2D hit in hitResult)
                if (hit.collider != null && !result.Find(x => x.collider == hit.collider))
                {
                    result.Add(hit);
                }
        }

        // cast last
        diff2 = point2 - original;
        hitResult = Physics2D.RaycastAll(original, diff2.normalized, diff2.magnitude, layer);
        foreach (RaycastHit2D hit in hitResult)
            if (hit.collider != null && !result.Find(x => x.collider == hit.collider))
            {
                result.Add(hit);
            }
    }
    public static RaycastHit2D MultipleRayCast(Vector2 original, Vector2 point1, Vector2 point2, float distance, int layer)
    {
        Vector2 diff = point2 - point1;
        Vector2 direction = diff.normalized;

        int maxIndex = (int)(diff.magnitude / distance);

        Vector2 diff2;
        RaycastHit2D hitResult;
        for (int i = 0; i <= maxIndex; i++)
        {
            Vector2 end = point1 + i * direction * distance;
            diff2 = end - original;
            hitResult = Physics2D.Raycast(original, diff2.normalized, diff2.magnitude, layer);
            if (hitResult.collider != null)
            {
                return hitResult;
            }
        }

        // cast last
        diff2 = point2 - original;
        hitResult = Physics2D.Raycast(original, diff2.normalized, diff2.magnitude, layer);
        return hitResult;
    }

    public static void DrawPlane(Vector3 normal, Vector3 position)
    {
        Vector3 v3;
        if (normal.normalized != Vector3.forward)
            v3 = Vector3.Cross(normal, Vector3.forward).normalized * normal.magnitude;
        else
            v3 = Vector3.Cross(normal, Vector3.up).normalized * normal.magnitude; ;

        var corner0 = position + v3;
        var corner2 = position - v3;
        var q = Quaternion.AngleAxis(90.0f, normal);
        v3 = q * v3;
        var corner1 = position + v3;
        var corner3 = position - v3;

        Debug.DrawLine(corner0, corner2, Color.green, 100);
        Debug.DrawLine(corner1, corner3, Color.green, 100);
        Debug.DrawLine(corner0, corner1, Color.green, 100);
        Debug.DrawLine(corner1, corner2, Color.green, 100);
        Debug.DrawLine(corner2, corner3, Color.green, 100);
        Debug.DrawLine(corner3, corner0, Color.green, 100);
        Debug.DrawRay(position, normal, Color.red);
    }

}
