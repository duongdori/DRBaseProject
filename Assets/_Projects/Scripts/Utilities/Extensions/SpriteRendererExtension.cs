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
        
        public static void FitCamera(this SpriteRenderer spriteRenderer, Camera camera)
        {
            if (camera.orthographic)
            {
                float worldScreenHeight = camera.orthographicSize * 2;
                float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

                spriteRenderer.transform.localScale = new Vector3(
                    worldScreenWidth / spriteRenderer.sprite.bounds.size.x,
                    worldScreenHeight / spriteRenderer.sprite.bounds.size.y, 1);
            }
            else
            {
                float spriteHeight = spriteRenderer.sprite.bounds.size.y;
                float spriteWidth = spriteRenderer.sprite.bounds.size.x;
                float distance = spriteRenderer.transform.position.z - camera.transform.position.z;
                float screenHeight = 2 * Mathf.Tan(camera.fieldOfView * Mathf.Deg2Rad / 2) * distance;
                float screenWidth = screenHeight * camera.aspect;
                spriteRenderer.transform.localScale = new Vector3(screenWidth / spriteWidth, screenHeight / spriteHeight, 1f);
            }
        }
        
        public static void SetSortingOrder(this SpriteRenderer spriteRenderer, int sortingOrder)
        {
            spriteRenderer.sortingOrder = sortingOrder;
        }
        
        public static void SetSortingLayerName(this SpriteRenderer spriteRenderer, string sortingLayerName)
        {
            spriteRenderer.sortingLayerName = sortingLayerName;
        }
        
        public static void SetSortingLayerID(this SpriteRenderer spriteRenderer, int sortingLayerID)
        {
            spriteRenderer.sortingLayerID = sortingLayerID;
        }
        
        public static void SetSprite(this SpriteRenderer spriteRenderer, Sprite sprite)
        {
            spriteRenderer.sprite = sprite;
        }
        
        public static void SetColor(this SpriteRenderer spriteRenderer, Color color)
        {
            spriteRenderer.color = color;
        }
        
        public static void SetMaterial(this SpriteRenderer spriteRenderer, Material material)
        {
            spriteRenderer.material = material;
        }
        
        public static void SetMaterialPropertyBlock(this SpriteRenderer spriteRenderer, MaterialPropertyBlock properties)
        {
            spriteRenderer.SetPropertyBlock(properties);
        }
        
        public static void SetFlipX(this SpriteRenderer spriteRenderer, bool flipX)
        {
            spriteRenderer.flipX = flipX;
        }
        
        public static void SetFlipY(this SpriteRenderer spriteRenderer, bool flipY)
        {
            spriteRenderer.flipY = flipY;
        }
        
        public static void SetSize(this SpriteRenderer spriteRenderer, Vector2 size)
        {
            spriteRenderer.size = size;
        }
        
        public static void SetDrawMode(this SpriteRenderer spriteRenderer, SpriteDrawMode drawMode)
        {
            spriteRenderer.drawMode = drawMode;
        }
        
        public static void SetTileMode(this SpriteRenderer spriteRenderer, SpriteTileMode tileMode)
        {
            spriteRenderer.tileMode = tileMode;
        }
        
        public static void SetAdaptiveModeThreshold(this SpriteRenderer spriteRenderer, float threshold)
        {
            spriteRenderer.adaptiveModeThreshold = threshold;
        }
        
        
    }
}
