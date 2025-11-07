using System.Collections;
using UnityEngine;

namespace Time
{
    public static class Delays
    {
        /// <summary>
        /// Creates a delay with a length that you set
        /// </summary>
        /// <param name="value">The time you want to wait</param>
        /// <returns></returns>
        public static IEnumerator Delay(float value) { yield return new WaitForSeconds(value); }

        /// <summary>
        /// Creates a delay with a length of the animationClip length
        /// </summary>
        /// <param name="animationClip">animationClip you want to create a delay for</param>
        /// <returns></returns>
        public static IEnumerator Delay(AnimationClip animationClip) { yield return Delay(animationClip.length); }
    }
}