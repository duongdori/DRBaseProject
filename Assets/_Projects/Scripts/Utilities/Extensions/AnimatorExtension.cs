using UnityEngine;

namespace DR.Utilities.Extensions
{
    public static class AnimatorExtension
    {
        public static bool HasParameterOfType(this Animator animator, string paramName, AnimatorControllerParameterType paramType)
        {
            if (string.IsNullOrEmpty(paramName))
                return false;

            AnimatorControllerParameter[] parameters = animator.parameters;
            foreach (AnimatorControllerParameter currParam in parameters)
            {
                if (currParam.type == paramType && currParam.name == paramName)
                    return true;
            }
            return false;
        }

        public static bool ContainsAnimation(this Animator animator, string animationName)
        {
            RuntimeAnimatorController ac = animator.runtimeAnimatorController;
            string lowerCase = animationName.ToLowerInvariant();
            for (int i = 0; i < ac.animationClips.Length; i++)
            {
                if (ac.animationClips[i].name.ToLowerInvariant() == lowerCase)
                    return true;
            }
            return false;
        }

        public static bool ContainsAnimation(this Animator animator, AnimationClip animationClip)
        {
            RuntimeAnimatorController ac = animator.runtimeAnimatorController;
            for (int i = 0; i < ac.animationClips.Length; i++)
            {
                if (ac.animationClips[i] == animationClip)
                    return true;
            }
            return false;
        }

        public static bool TryGetAnimation(this Animator animator, string animationName, out AnimationClip animationClip)
        {
            animationClip = null;
            RuntimeAnimatorController ac = animator.runtimeAnimatorController;
            string lowerCase = animationName.ToLowerInvariant();
            for (int i = 0; i < ac.animationClips.Length; i++)
            {
                if (ac.animationClips[i].name.ToLowerInvariant() == lowerCase)
                {
                    animationClip = ac.animationClips[i];
                    return true;
                }
            }
            return false;
        }
    }
}