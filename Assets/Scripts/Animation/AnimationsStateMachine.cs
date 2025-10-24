using UnityEngine;

namespace Animation
{
    public class AnimationsStateMachine : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        
        #region Animation Hash
        private static int _idlingHash;
        private static int _pushingHash;
        private static int _pullingHash;
        private static int _walkingHash;
        private static int _runningHash;
        private static int _jumpingHash;
        private static int _fallingHash;
        private static int _landingHash;
        #endregion Animation Hash
        
        private static AnimationsStateMachine _stateMachine;
        private void OnEnable() => _stateMachine = this;
        private void OnDisable() => _stateMachine = _stateMachine == this ? null : _stateMachine;
        
        private void OnValidate() { if (animator is null) Debug.LogWarning("animator is null in the AnimationsStateMachine"); }
        private void Start()
        { 
            _idlingHash = Animator.StringToHash("IsIdling"); 
            _pushingHash = Animator.StringToHash("IsPushing"); 
            _pullingHash = Animator.StringToHash("IsPulling"); 
            _walkingHash = Animator.StringToHash("IsWalking");
            _runningHash = Animator.StringToHash("IsRunning");
            _jumpingHash = Animator.StringToHash("IsJumping");
            _fallingHash = Animator.StringToHash("IsFalling");
            _landingHash = Animator.StringToHash("IsLanding");
        }

        public static void SetState(AnimationsStates newState)
        {
            switch(newState)
            {
                case AnimationsStates.IsIdling: _stateMachine.animator.SetTrigger(_idlingHash); break;
                case AnimationsStates.IsPushing: _stateMachine.animator.SetTrigger(_pushingHash); break;
                case AnimationsStates.IsPulling: _stateMachine.animator.SetTrigger(_pullingHash); break;
                case AnimationsStates.IsWalking: _stateMachine.animator.SetTrigger(_walkingHash); break;
                case AnimationsStates.IsRunning: _stateMachine.animator.SetTrigger(_runningHash); break;
                case AnimationsStates.IsJumping: _stateMachine.animator.SetTrigger(_jumpingHash); break;
                case AnimationsStates.IsFalling: _stateMachine.animator.SetTrigger(_fallingHash); break;
                case AnimationsStates.IsLanding: _stateMachine.animator.SetTrigger(_landingHash); break;
                default: Debug.LogError("State is not implemented! State is: " + newState); break;
            }
        }
    }
}