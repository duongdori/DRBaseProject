using UnityEngine;

namespace DR.Utilities.Extensions
{
    public static class ParticleSystemExtension
    {
        public static void OnStoppedWithPooling(this ParticleSystem particle, System.Action onStoppedCallback)
        {
            ParticleSystemExtensionHelper helper = PoolManager.Instance
                .SpawnObject(PoolType.ObjectHelper, Vector3.zero, Quaternion.identity)
                .GetComponent<ParticleSystemExtensionHelper>();
            
            helper.StartWatching(particle, (() =>
            {
                onStoppedCallback?.Invoke();
                
                PoolManager.Instance.DespawnObject(helper.transform);
            }));
        }

        public static void OnStoppedWithDestroy(this ParticleSystem particle, System.Action onStoppedCallback)
        {
            ParticleSystemExtensionHelper helper = new GameObject("ParticleSystemExtensionHelper")
                .AddComponent<ParticleSystemExtensionHelper>();

            helper.StartWatching(particle, (() =>
            {
                onStoppedCallback?.Invoke();
                
                Object.Destroy(helper.gameObject);
            }));
        }
    }
}

