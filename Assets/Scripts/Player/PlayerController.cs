using System.Collections;
using System.Runtime.CompilerServices;
using Animation;
using Events;
using UnityEngine;
using Input;
using Time;
using UI;
using static DeathTrigger.FallingDeathTrigger;
using static Player.Movement.Move;
using static Player.Movement.Jump;
using static Player.Movement.PushAndPull;
using static UnityEngine.InputSystem.InputAction;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour, IDeathEvent
    {
        [Header("DeathTrigers")]
        [Tooltip("The number you check the linearVelocity.y on FallingDeathTrigger to se if the fall is safe")]
        [SerializeField] private float safeFallVelocity;
        
        [Header("Move")]
        [SerializeField] private float walkSpeed;
        [SerializeField] private float runSpeed;
        
        [Header("crouch")]
        [SerializeField] private float crouchWalkSpeed;
        [SerializeField] private float crouchRunSpeed;
        
        [Header("Jump")]
        [SerializeField] private float jumpPower;
        [SerializeField] private float runJumpPower;
        [Tooltip("The number you add to the linearVelocity.x on a runJump")]
        [SerializeField] private float runJumpMomentumBoost;

        [Header("Colliders")] 
        [Tooltip("Players main collider")]
        [SerializeField] private CapsuleCollider2D playerCollider;
        [Tooltip("Players collider when crouching")]
        [SerializeField] private CapsuleCollider2D playerCrouchCollider;
        
        private PlayerInputController _inputControls;
        private PlayerInputController.PlayerActions PlayerMap => _inputControls.Player;

        public Rigidbody2D GetRigidbody2D { get; private set; }
        public static PlayerController GetPlayerController { get; private set; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Awake()
        {
            _inputControls = new PlayerInputController();
            MovementControlsBind();
            
            GetRigidbody2D = GetComponent<Rigidbody2D>();

            OnFallingDeathTriggerAwake(GetRigidbody2D, safeFallVelocity);

            OnMovementAwake(GetRigidbody2D, playerCollider, playerCrouchCollider, walkSpeed, runSpeed, crouchWalkSpeed, crouchRunSpeed);
            OnJumpAwake(GetRigidbody2D, jumpPower, runJumpPower, runJumpMomentumBoost);
            OnPushAndPullAwake(GetRigidbody2D);
        }
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
        private void Update()
        {
            OnFallingDeathTriggerUpdate();
            
            OnMovementUpdate();
            OnJumpUpdate();
            OnPushAndPullUpdate();
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void FixedUpdate()
        {
            OnFallingDeathTriggerFixedUpdate();
                
            OnMovementFixedUpdate();
            OnJumpFixedUpdate();
            OnPushAndPullFixedUpdate();
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
            if (!AnimationsStateMachine.StateMachine.AnimationMachineStatesMapping.TryGetValue(AnimationsStates.IsDyingRight, out AnimationsStateMachine.AnimationMachineStates animationMachineStates))
                Debug.LogError("IsDying is do not exist in AnimationMachineStatesMapping");
            if (animationMachineStates is null) Debug.LogError("IsDying value is not set");
            else
            {
                IEnumerator enumerator = Delays.Delay(animationMachineStates.AnimationClip.length);
            }

            ResetLevel.ResetCurrentLevel();
        }
    }
}
