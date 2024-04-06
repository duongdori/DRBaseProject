using UnityEngine;

namespace DR.Utilities.Extensions
{
    public static class Color32Extensions
    {
        /// <summary>
        /// Returns true if the <see cref="Color32.r">red,</see> <see cref="Color32.g">green,</see>
        /// <see cref="Color32.b">blue,</see> and <see cref="Color32.a">alpha</see> channels of both
        /// <see cref="Color32">Color32s</see> passed in are equal.
        /// </summary>
        public static bool IsEqualTo(this Color32 first, Color32 second) =>
            first.r == second.r && first.g == second.g && first.b == second.b && first.a == second.a;

        public static bool IsTransparent(this Color32 color) => color is { r: 0, g: 0, b: 0, a: 0 };
    }
}
