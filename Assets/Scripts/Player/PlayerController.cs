using System;
using UnityEngine;
using Input;
using static Player.Movement.Move;
using static Player.Movement.Jump;
using static Player.Movement.PushAndPull;
using static UnityEngine.InputSystem.InputAction;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Move")]
        [SerializeField] private float speed;
        [Header("Jump")]
        [SerializeField] private float jumpPower;
        
        private PlayerInputController _inputControls;
        private Rigidbody2D _rigidbody2D;

        private PlayerInputController.PlayerActions PlayerMap => _inputControls.Player;

        private void Awake()
        {
            _inputControls = new PlayerInputController();
            MovementControlsBind();
            
            _rigidbody2D = GetComponent<Rigidbody2D>();

            OnMovementAwake(_rigidbody2D, speed);
            OnJumpAwake(_rigidbody2D, jumpPower);
            OnPushAndPullAwake(_rigidbody2D);
        }

        private void OnEnable() => PlayerMap.Enable();
        private void OnDisable() => PlayerMap.Disable();

        private void Update()
        {
            OnMovementUpdate();
            OnJumpUpdate();
            OnPushAndPullUpdate();
        }

        private void FixedUpdate()
        {
            OnMovementFixedUpdate();
            OnJumpFixedUpdate();
            OnPushAndPullFixedUpdate();
        }
        
        private void OnCollisionEnter2D(Collision2D other) => OnPushAndPullCollisionEnter2D(other);
        private void OnCollisionExit2D(Collision2D other) => OnPushAndPullCollisionExit2D(other);


        private void MovementControlsBind()
        {
            PlayerMap.Move.performed += OnMovePreset;
            PlayerMap.Move.canceled += OnMoveRelist;

            PlayerMap.Jump.performed += OnJumpPreset;
            PlayerMap.Jump.canceled += OnJumpRelist;

            PlayerMap.PushOrPull.performed += OnPushOrPullPreset;
            PlayerMap.PushOrPull.canceled += OnPushOrPullRelist;
        }

        public static void OnMovePreset(CallbackContext callbackContext) => OnMove(callbackContext.ReadValue<float>());
        private static void OnMoveRelist(CallbackContext callbackContext) => OnMove(0);
        
        private static void OnJumpPreset(CallbackContext callbackContext) => OnJump(callbackContext.performed);
        private static void OnJumpRelist(CallbackContext callbackContext) => OnJump(false);
        
        private static void OnPushOrPullPreset(CallbackContext callbackContext) => OnPushAndPull();
        private static void OnPushOrPullRelist(CallbackContext callbackContext) => OnPushAndPull();
    }
}
