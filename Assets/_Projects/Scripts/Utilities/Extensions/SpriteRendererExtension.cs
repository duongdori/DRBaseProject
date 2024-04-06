using UnityEngine;

namespace DR.Utilities.Extensions
{
    public static class SpriteRendererExtension
    {
        public static void SetAlpha(this SpriteRenderer spriteRenderer, float alpha)
        {
            Color color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;
        }
    }
}
