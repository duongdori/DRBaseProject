using UnityEngine;

namespace DR.Utilities.Extensions
{
    public static class Vector3Extension
    {
        /// <summary>
        /// Creates a new Vector3 with the specified x, y, and z values, or uses the original values if none are specified.
        /// </summary>
        public static Vector3 With(this Vector3 vector, float? x = null, float? y = null, float? z = null)
        {
            return new Vector3(x ?? vector.x, y ?? vector.y, z ?? vector.z);
        }
        
        /// <summary>
        /// Creates a new Vector3 with the specified X value.
        /// </summary>
        public static Vector3 WithX(this Vector3 vector, float x)
        {
            return new Vector3(x, vector.y, vector.z);
        }

        /// <summary>
        /// Creates a new Vector3 with the specified Y value.
        /// </summary>
        public static Vector3 WithY(this Vector3 vector, float y)
        {
            return new Vector3(vector.x, y, vector.z);
        }

        /// <summary>
        /// Creates a new Vector3 with the specified Z value.
        /// </summary>
        public static Vector3 WithZ(this Vector3 vector, float z)
        {
            return new Vector3(vector.x, vector.y, z);
        }

        /// <summary>
        /// Adds the specified x, y, and z values to the original Vector3 and returns a new Vector3.
        /// </summary>
        public static Vector3 Add(this Vector3 vector, float x = 0, float y = 0, float z = 0)
        {
            return new Vector3(vector.x + x, vector.y + y, vector.z + z);
        }

        /// <summary>
        /// Converts a Vector3 to a Vector3Int by truncating the decimal part of each component.
        /// </summary>
        public static Vector3Int ToVector3Int(this Vector3 vector3)
        {
            return new Vector3Int((int)vector3.x, (int)vector3.y, (int)vector3.z);
        }

        /// <summary>
        /// Flattens the Vector3 by setting the y component to 0.
        /// </summary>
        public static Vector3 Flatten(this Vector3 vector3)
        {
            return new Vector3(vector3.x, 0, vector3.z);
        }

        /// <summary>
        /// Converts a Vector3 to a Vector2 by discarding the z component.
        /// </summary>
        public static Vector2 ToVector2(this Vector3 vector3)
        {
            return new Vector2(vector3.x, vector3.y);
        }
        
        /// <summary>
        /// Rotates a Vector3 around a point and along a specified axis.
        /// </summary>
        public static Vector3 RotateAround(this Vector3 point, Vector3 pivot, Vector3 angles)
        {
            Vector3 dir = point - pivot;
            dir = Quaternion.Euler(angles) * dir;
            return pivot + dir;
        }
        
        /// <summary>
        /// Returns the angle in radians between two Vector3.
        /// </summary>
        public static float GetAngleInRadian(this Vector3 v1, Vector3 v2)
        {
            return Mathf.Atan2(v2.y - v1.y, v2.x - v1.x);
        }

        /// <summary>
        /// Returns the angle in degrees between two Vector3.
        /// </summary>
        public static float GetAngleInDegree(this Vector3 v1, Vector3 v2)
        {
            return Mathf.Atan2(v2.y - v1.y, v2.x - v1.x) * Mathf.Rad2Deg;
        }

        /// <summary>
        /// Returns the angle in degrees of a Vector3.
        /// </summary>
        public static float GetAngleInDegree(this Vector3 v1)
        {
            return Mathf.Atan2(v1.y, v1.x) * Mathf.Rad2Deg;
        }

        /// <summary>
        /// Rotates a Vector3 by a specified angle.
        /// </summary>
        public static Vector3 Rotate(this Vector3 v, float angle)
        {
            v = Quaternion.AngleAxis(angle, Vector3.back) * v;
            return v;
        }
    }
}