using UnityEngine;

namespace DR.Utilities.Extensions
{
    public static class CameraExtension
    {
        public static bool IsVisible(this Camera camera, Vector3 position)
        {
            Vector3 viewPos = camera.WorldToViewportPoint(position);
        
            return viewPos.x is >= 0 and <= 1 && viewPos.y is >= 0 and <= 1 && viewPos.z > 0;
        }
    }
}