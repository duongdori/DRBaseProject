using UnityEngine;

namespace DR.Utilities.Extensions
{
    public static class RendererExtension
    {
        public static void ChangeMaterial(this Renderer rend, Material mat, int index = 0)
        {
            Material[] mats = rend.materials;
            mats[index] = mat;
            rend.materials = mats;
        }
        public static void ChangeTextureProp(this Renderer renderer, Texture tex)
        {
            MaterialPropertyBlock propBlock = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(propBlock);
            int propID = Shader.PropertyToID("_MainTex");
            propBlock.SetTexture(propID, tex);
            renderer.SetPropertyBlock(propBlock);
        }

        public static void ChangeColorProp(this Renderer renderer, Color color)
        {
            MaterialPropertyBlock propBlock = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(propBlock);
            int propID = Shader.PropertyToID("_Color");
            propBlock.SetColor(propID, color);
            renderer.SetPropertyBlock(propBlock);
        }
        public static void SetColor(this Renderer renderer, Color color)
        {
            MaterialPropertyBlock propBlock = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(propBlock);
            propBlock.SetColor("_Color", color);
            renderer.SetPropertyBlock(propBlock);
        }
        
        public static void ChangeMesh(this SkinnedMeshRenderer rend, Mesh mesh)
        {
            rend.sharedMesh = mesh;
        }
    }
}