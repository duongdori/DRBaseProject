using UnityEngine;

namespace DR.Utilities
{
    public static class GizmosHelpers
    {
        public static void DrawCircle(Vector3 center, float radius, Color color, int segments = 20)
        {
            Gizmos.color = color;
            float angle = 0f;
            float angleStep = 360f / segments;
            Vector3 start = center + new Vector3(Mathf.Cos(0) * radius, 0, Mathf.Sin(0) * radius);
            for (int i = 0; i < segments + 1; i++)
            {
                var end = center + new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * radius, 0,
                    Mathf.Sin(angle * Mathf.Deg2Rad) * radius);
                Gizmos.DrawLine(start, end);
                start = end;
                angle += angleStep;
            }
        }

        public static void DrawWireCapsule(Vector3 pos, Quaternion rot, float radius, float height,
            Color color = default(Color))
        {
            if (color != default(Color))
                Gizmos.color = color;

            float halfHeight = height * 0.5f - radius;
            Vector3 capStart = pos + rot * Vector3.up * halfHeight;
            Vector3 capEnd = pos + rot * Vector3.down * halfHeight;

            //Radial circles
            DrawCircle(capStart, radius, color);
            DrawCircle(capEnd, radius, color);

            //Lines
            Gizmos.DrawLine(capStart + rot * Vector3.right * radius, capEnd + rot * Vector3.right * radius);
            Gizmos.DrawLine(capStart + rot * Vector3.forward * radius, capEnd + rot * Vector3.forward * radius);
            Gizmos.DrawLine(capStart + rot * Vector3.left * radius, capEnd + rot * Vector3.left * radius);
            Gizmos.DrawLine(capStart + rot * Vector3.back * radius, capEnd + rot * Vector3.back * radius);

            //Top and bottom lines
            Gizmos.DrawLine(capStart + rot * Vector3.right * radius, capStart + rot * Vector3.forward * radius);
            Gizmos.DrawLine(capStart + rot * Vector3.forward * radius, capStart + rot * Vector3.left * radius);
            Gizmos.DrawLine(capStart + rot * Vector3.left * radius, capStart + rot * Vector3.back * radius);
            Gizmos.DrawLine(capStart + rot * Vector3.back * radius, capStart + rot * Vector3.right * radius);
            
            Gizmos.DrawLine(capEnd + rot * Vector3.right * radius, capEnd + rot * Vector3.forward * radius);
            Gizmos.DrawLine(capEnd + rot * Vector3.forward * radius, capEnd + rot * Vector3.left * radius);
            Gizmos.DrawLine(capEnd + rot * Vector3.left * radius, capEnd + rot * Vector3.back * radius);
            Gizmos.DrawLine(capEnd + rot * Vector3.back * radius, capEnd + rot * Vector3.right * radius);
            
        }
    }
}