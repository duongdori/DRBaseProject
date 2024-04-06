using UnityEngine;

namespace DR.Utilities.Extensions
{
    public static class Vector2Extension
    {
        // Creates a new Vector2 with the specified x and y values, or uses the original values if none are specified.
        public static Vector2 With(this Vector2 vector2, float? x = null, float? y = null)
        {
            return new Vector2(x ?? vector2.x, y ?? vector2.y);
        }
        
        // Creates a new Vector2 with the specified X value.
        public static Vector2 WithX(this Vector2 vector2, float? x = null)
        {
            return new Vector2(x ?? vector2.x, vector2.y);
        }

        // Creates a new Vector2 with the specified Y value.
        public static Vector2 WithY(this Vector2 vector2, float? y = null)
        {
            return new Vector2(vector2.x, y ?? vector2.y);
        }

        // Adds the specified x and y values to the original Vector2 and returns a new Vector2.
        public static Vector2 Add(this Vector2 vector2, float x = 0, float y = 0)
        {
            return new Vector2(vector2.x + x, vector2.y + y);
        }
        
        // Rotates a Vector2 by a specified angle.
        public static Vector2 Rotate(this Vector2 vector, float degrees)
        {
            float radians = degrees * Mathf.Deg2Rad;
            float sin = Mathf.Sin(radians);
            float cos = Mathf.Cos(radians);
            float x = vector.x * cos - vector.y * sin;
            float y = vector.x * sin + vector.y * cos;
            return new Vector2(x, y);
        }
        
        // Generates a random Vector2 within the unit circle.
        public static Vector2 RandomInUnitCircle()
        {
            float angle = Random.Range(0f, Mathf.PI * 2);
            return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        }
        
        // Returns a Vector2 with all components converted to absolute values.
        public static Vector2 Abs(this Vector2 vector2)
        {
            return new Vector2(Mathf.Abs(vector2.x), Mathf.Abs(vector2.y));
        }
        
        // Limits the magnitude of the Vector2.
        public static Vector2 ClampMagnitude(this Vector2 vector2, float maxLength)
        {
            if (vector2.sqrMagnitude > maxLength * maxLength)
                return vector2.normalized * maxLength;
            return vector2;
        }
        
        // Reflects a Vector2 through a specified normal.
        public static Vector2 Reflect(this Vector2 vector2, Vector2 normal)
        {
            return vector2 - 2 * Vector2.Dot(vector2, normal) * normal;
        }
        
        // Checks whether two Vector2s are parallel to each other.
        public static bool IsParallelTo(this Vector2 vector2, Vector2 other)
        {
            return Mathf.Approximately(Vector2.Dot(vector2.normalized, other.normalized), 1);
        }
        
        // Checks whether a point is to the left of a line.
        public static bool IsPointLeftOfLine(Vector2 lineStart, Vector2 lineEnd, Vector2 point)
        {
            return ((lineEnd.x - lineStart.x) * (point.y - lineStart.y) - (lineEnd.y - lineStart.y) * (point.x - lineStart.x)) > 0;
        }
    }
}