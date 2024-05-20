using UnityEngine;

namespace DRG.Utilities.Extensions
{
    public static class ColorExtension
    {
        public static string ToHtmlStringRGB(this Color color) => ColorUtility.ToHtmlStringRGB(color);

        public static Color WithAlpha(this Color color, float alpha)
        {
            color.a = alpha;
            return color;
        }
    }

}