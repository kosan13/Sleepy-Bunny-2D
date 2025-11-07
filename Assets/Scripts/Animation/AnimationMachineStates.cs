using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Animation
{
    public partial class AnimationsStateMachine
    {
        [Serializable]
        public class AnimationMachineStates
        {
            [SerializeField] private AnimationsStates animationsState;
            [SerializeField] private string triggerParameter = string.Empty;
            [SerializeField] private AnimationClip animationClip;

            public AnimationsStates AnimationsStates => animationsState;
            public string TriggerParameter => triggerParameter;
            public AnimationClip AnimationClip => animationClip;

            private int _parameterHash;
                
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Poll(AnimationsStates stateMatch)
            {
                if (animationsState != stateMatch)
                {
                    try { StateMachine.animator.ResetTrigger(triggerParameter); }
                    catch (Exception exception) { Debug.Log(exception.Message); }
                }
                else
                {
                    try { StateMachine.animator.SetTrigger(triggerParameter); }
                    catch (Exception exception) { Debug.Log(exception.Message); }
                }
            }
                
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void GenerateHash() => _parameterHash = Animator.StringToHash(triggerParameter);
        }
    }
}