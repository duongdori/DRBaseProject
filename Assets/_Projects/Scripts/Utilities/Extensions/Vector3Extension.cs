using UnityEngine;

namespace DR.Utilities.Extensions
{
    public static class Vector3Extension
    {
        // Creates a new Vector3 with the specified x, y, and z values, or uses the original values if none are specified.
        public static Vector3 With(this Vector3 vector, float? x = null, float? y = null, float? z = null)
        {
            return new Vector3(x ?? vector.x, y ?? vector.y, z ?? vector.z);
        }
        
        // Creates a new Vector3 with the specified X value.
        public static Vector3 WithX(this Vector3 vector, float x)
        {
            return new Vector3(x, vector.y, vector.z);
        }

        // Creates a new Vector3 with the specified Y value.
        public static Vector3 WithY(this Vector3 vector, float y)
        {
            return new Vector3(vector.x, y, vector.z);
        }

        // Creates a new Vector3 with the specified Z value.
        public static Vector3 WithZ(this Vector3 vector, float z)
        {
            return new Vector3(vector.x, vector.y, z);
        }

        // Adds the specified x, y, and z values to the original Vector3 and returns a new Vector3.
        public static Vector3 Add(this Vector3 vector, float x = 0, float y = 0, float z = 0)
        {
            return new Vector3(vector.x + x, vector.y + y, vector.z + z);
        }

        // Converts a Vector3 to a Vector3Int by truncating the decimal part of each component.
        public static Vector3Int ToVector3Int(this Vector3 vector3)
        {
            return new Vector3Int((int)vector3.x, (int)vector3.y, (int)vector3.z);
        }

        // Flattens the Vector3 by setting the y component to 0.
        public static Vector3 Flatten(this Vector3 vector3)
        {
            return new Vector3(vector3.x, 0, vector3.z);
        }

        // Converts a Vector3 to a Vector2 by discarding the z component.
        public static Vector2 ToVector2(this Vector3 vector3)
        {
            return new Vector2(vector3.x, vector3.y);
        }
        
        //  Rotates a Vector3 around a point and along a specified axis.
        public static Vector3 RotateAround(this Vector3 point, Vector3 pivot, Vector3 angles)
        {
            Vector3 dir = point - pivot;
            dir = Quaternion.Euler(angles) * dir;
            return pivot + dir;
        }
    }
}