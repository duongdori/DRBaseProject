using System.Collections;
using UnityEngine;

namespace DR.Utilities
{
    public class ParticleSystemExtensionHelper : MonoBehaviour
    {
        public void StartWatching(ParticleSystem particle, System.Action callback)
        {
            StartCoroutine(CheckIfParticleSystemStopped(particle, callback));
        }

        private IEnumerator CheckIfParticleSystemStopped(ParticleSystem particle, System.Action onStopped)
        {
            while (!particle.isPlaying)
            {
                yield return null;
            }
        
            while (particle.isPlaying)
            {
                yield return null;
            }
        
            onStopped?.Invoke();
        }
    }
}