using System.Collections;
using System.Runtime.CompilerServices;
using Animation;
using Events;
using Global;
using UnityEngine;
using Input;
using TimeFunctions;
using static LevelFunctionsLibrary.LevelFunctions;
using static DeathTrigger.FallingDeathTrigger;
using static Player.Movement.Walk;
using static Player.Movement.Run;
using static Player.Movement.Jump;
using static Player.Movement.Crouch;
using static Player.Movement.PushAndPull;
using static Player.Movement.ForceAccumulate;
using static UnityEngine.InputSystem.InputAction;

namespace Player
{
    public enum PlayerAnimationsDirectionTypes
    {
        Left,
        Right
    }
    
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour, IDeathEvent
    {
        [Header("DeathTriggers")]
        [Tooltip("The number you check the linearVelocity.y on FallingDeathTrigger to se if the fall is safe")]
        [SerializeField] private float safeFallVelocity;
        
        [Header("Move")]
        [SerializeField] private float walkSpeed = 10;
        [SerializeField] private float runSpeed = 15;
        
        [Header("crouch")]
        [SerializeField] private float crouchWalkSpeed = 5;
        [SerializeField] private float crouchRunSpeed = 7.5f;
        
        [Header("Jump")]
        [SerializeField] private float jumpPower = 15;
        [SerializeField] private float runJumpPower = 7.5f;
        [Tooltip("The number you add to the linearVelocity.x on a runJump")]
        [SerializeField] private float runJumpMomentumBoost = 10;
        
        [Header("Force")]
        [SerializeField] private float gravity = 50;
        [SerializeField] private float playerForceFalloffValue = 10;
        [SerializeField] private float runJumpMomentumBoostFalloffValue = 5;

        [Header("Colliders")] 
        [Tooltip("Players main collider")]
        [SerializeField] private CapsuleCollider2D playerCollider;
        [Tooltip("Players collider when crouching")]
        [SerializeField] private CapsuleCollider2D playerCrouchCollider;
        
        [Header("Others")] 
        [SerializeField] private Transform mainAnimationBone;
        
        public static PlayerController GetPlayerController { get; private set; }
        public Rigidbody2D GetRigidbody2D { get; private set; }

        public Transform MainAnimationBone => mainAnimationBone;
        public float WalkSpeed => walkSpeed;
        public float CrouchWalkSpeed => crouchWalkSpeed;
        
        
        private PlayerInputController _inputControls;
        private PlayerInputController.PlayerActions PlayerMap => _inputControls.Player;
        

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OnEnable()
        {
            PlayerMap.Enable();
            GetPlayerController = this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OnDisable()
        {
            PlayerMap.Disable();
            GetPlayerController = GetPlayerController == this ? null : GetPlayerController;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Awake()
        {
            _inputControls = new PlayerInputController();
            MovementControlsBind();
            
            GetRigidbody2D = GetComponent<Rigidbody2D>();

            OnFallingDeathTriggerAwake(GetRigidbody2D, safeFallVelocity);

            OnWalkAwake(walkSpeed, crouchWalkSpeed);
            OnRunAwake(runSpeed, crouchRunSpeed);
            OnJumpAwake(jumpPower, runJumpPower, runJumpMomentumBoost);
            OnPushAndPullAwake(GetRigidbody2D);
            OnCrouchAwake(playerCollider, playerCrouchCollider);
            OnForceAccumulateAwake(gravity, playerForceFalloffValue, runJumpMomentumBoostFalloffValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Update()
        {
            OnFallingDeathTriggerUpdate();
            
            OnWalkUpdate();
            OnRunUpdate();
            OnJumpUpdate();
            OnPushAndPullUpdate();
            OnCrouchUpdate();
            OnForceAccumulateUpdate();
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void FixedUpdate()
        {
            OnFallingDeathTriggerFixedUpdate();
                
            OnWalkFixedUpdate();
            OnRunFixedUpdate();
            OnJumpFixedUpdate();
            OnPushAndPullFixedUpdate();
            OnCrouchFixedUpdate();
            OnForceAccumulateFixedUpdate();

        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OnCollisionEnter2D(Collision2D other) => OnPushAndPullCollisionEnter2D(other);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OnCollisionExit2D(Collision2D other) => OnPushAndPullCollisionExit2D(other);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OnTriggerEnter2D(Collider2D other) => OnPushAndPullTriggerEnter2D(other);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OnTriggerExit2D(Collider2D other) => OnPushAndPullOnTriggerExit2D(other);
        
        private void MovementControlsBind()
        {
            PlayerMap.Move.performed += OnMovePreset;
            PlayerMap.Move.canceled += OnMoveRelist;
            
            PlayerMap.Run.performed += OnRunPreset;
            PlayerMap.Run.canceled += OnRunRelist;

            PlayerMap.Jump.performed += OnJumpPreset;
            PlayerMap.Jump.canceled += OnJumpRelist;

            PlayerMap.PushOrPull.performed += OnPushOrPullPreset;
            PlayerMap.PushOrPull.canceled += OnPushOrPullRelist;
            
            PlayerMap.Crouch.performed += OnCrouchPreset;
            PlayerMap.Crouch.canceled += OnCrouchRelist;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void OnMovePreset(CallbackContext callbackContext) => OnWalk(callbackContext.ReadValue<float>());
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void OnMoveRelist(CallbackContext callbackContext) => OnWalk(0);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void OnRunPreset(CallbackContext callbackContext) => OnRun(callbackContext.ReadValue<float>());
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void OnRunRelist(CallbackContext callbackContext) => OnRun(0);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void OnJumpPreset(CallbackContext callbackContext) => OnJump(callbackContext.performed);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void OnJumpRelist(CallbackContext callbackContext) => OnJump(false);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void OnPushOrPullPreset(CallbackContext callbackContext) => OnPushAndPull();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void OnPushOrPullRelist(CallbackContext callbackContext) => OnPushAndPull(true);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void OnCrouchPreset(CallbackContext callbackContext) => OnCrouch();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void OnCrouchRelist(CallbackContext callbackContext) => OnCrouch(true);

        
        
        public void TriggerDeathEvent()
        {
            AnimationsStates animationsStates = AnimationsStateMachine.SetPlayerAnimationAndAnimationsDirection(mainAnimationBone, AnimationsStates.IsDyingLeft, AnimationsStates.IsDyingRight);
            if (!AnimationsStateMachine.StateMachine.AnimationMachineStatesMapping.TryGetValue(animationsStates, out AnimationsStateMachine.AnimationMachineStates animationMachineStates))
                Debug.LogError("IsDying is do not exist in AnimationMachineStatesMapping");
            Debug.Log(Time.time);
            if (animationMachineStates is null) Debug.LogError("IsDying value is not set");
            else { IEnumerator enumerator = Delays.Delay(animationMachineStates.AnimationClip.length); }
            Debug.Log(Time.time);

            ResetCurrentLevel();
        }
    }
}
