using UnityEngine;

namespace DR.Utilities.Extensions
{
    public static class Vector2Extension
    {
        /// <summary>
        /// Creates a new Vector2 with the specified x and y values, or uses the original values if none are specified.
        /// </summary>
        public static Vector2 With(this Vector2 vector2, float? x = null, float? y = null)
        {
            return new Vector2(x ?? vector2.x, y ?? vector2.y);
        }
        
        /// <summary>
        /// Creates a new Vector2 with the specified X value.
        /// </summary>
        public static Vector2 WithX(this Vector2 vector2, float? x = null)
        {
            return new Vector2(x ?? vector2.x, vector2.y);
        }

        /// <summary>
        /// Creates a new Vector2 with the specified Y value.
        /// </summary>
        public static Vector2 WithY(this Vector2 vector2, float? y = null)
        {
            return new Vector2(vector2.x, y ?? vector2.y);
        }

        /// <summary>
        /// Adds the specified x and y values to the original Vector2 and returns a new Vector2.
        /// </summary>
        public static Vector2 Add(this Vector2 vector2, float x = 0, float y = 0)
        {
            return new Vector2(vector2.x + x, vector2.y + y);
        }
        
        /// <summary>
        /// Rotates a Vector2 by a specified angle.
        /// </summary>
        public static Vector2 Rotate(this Vector2 vector, float degrees)
        {
            float radians = degrees * Mathf.Deg2Rad;
            float sin = Mathf.Sin(radians);
            float cos = Mathf.Cos(radians);
            float x = vector.x * cos - vector.y * sin;
            float y = vector.x * sin + vector.y * cos;
            return new Vector2(x, y);
        }
        
        /// <summary>
        /// Generates a random Vector2 within the unit circle.
        /// </summary>
        public static Vector2 RandomInUnitCircle()
        {
            float angle = Random.Range(0f, Mathf.PI * 2);
            return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        }
        
        /// <summary>
        /// Returns a Vector2 with all components converted to absolute values.
        /// </summary>
        public static Vector2 Abs(this Vector2 vector2)
        {
            return new Vector2(Mathf.Abs(vector2.x), Mathf.Abs(vector2.y));
        }
        
        /// <summary>
        /// Limits the magnitude of the Vector2.
        /// </summary>
        public static Vector2 ClampMagnitude(this Vector2 vector2, float maxLength)
        {
            if (vector2.sqrMagnitude > maxLength * maxLength)
                return vector2.normalized * maxLength;
            return vector2;
        }
        
        /// <summary>
        /// Reflects a Vector2 through a specified normal.
        /// </summary>
        public static Vector2 Reflect(this Vector2 vector2, Vector2 normal)
        {
            return vector2 - 2 * Vector2.Dot(vector2, normal) * normal;
        }
        
        /// <summary>
        /// Checks whether two Vector2 are parallel to each other.
        /// </summary>
        public static bool IsParallelTo(this Vector2 vector2, Vector2 other)
        {
            return Mathf.Approximately(Vector2.Dot(vector2.normalized, other.normalized), 1);
        }
        
        /// <summary>
        /// Checks whether a point is to the left of a line.
        /// </summary>
        public static bool IsPointLeftOfLine(Vector2 lineStart, Vector2 lineEnd, Vector2 point)
        {
            return ((lineEnd.x - lineStart.x) * (point.y - lineStart.y) - (lineEnd.y - lineStart.y) * (point.x - lineStart.x)) > 0;
        }
        
        /// <summary>
        /// Returns the angle in radians between two Vector2.
        /// </summary>
        public static float GetAngleInRadian(this Vector2 v1, Vector2 v2)
        {
            return Mathf.Atan2(v2.y - v1.y, v2.x - v1.x);
        }

        /// <summary>
        /// Returns the angle in degrees between two Vector2.
        /// </summary>
        public static float GetAngleInDegree(this Vector2 v1, Vector2 v2)
        {
            return Mathf.Atan2(v2.y - v1.y, v2.x - v1.x) * Mathf.Rad2Deg;
        }

        /// <summary>
        /// Rotates a Vector2 by a specified angle using a quaternion.
        /// </summary>
        public static Vector2 RotateByQuaternion(this Vector2 vector2, float angle)
        {
            return Quaternion.AngleAxis(angle, Vector3.back) * vector2;
        }

        /// <summary>
        /// Returns the angle in degrees of a Vector2.
        /// </summary>
        public static float GetAngleInDegree(this Vector2 v1)
        {
            return Mathf.Atan2(v1.y, v1.x) * Mathf.Rad2Deg;
        }
    }
}