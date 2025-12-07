using System;
using System.Runtime.CompilerServices;
using Global;
using UnityEngine;

using static Global.GlobalVariablesLibrary;

namespace Player.Movement
{
    public static class ForceAccumulate
    {
        [Serializable]
        public struct AccumulateForceFalloffValue
        {
            public float gravity;
            public float inputForceFalloffValue;
            public float runJumpBoostForceFalloffValue;
            public float waterForceFallofValue;
        }

        private static Vector2 _accumulatedForce;
        
        private static Vector2 _inputForce;
        private static float _inputForceFalloffValue;
        
        private static Vector2 _gravityForce;
        private static float _gravity;
        
        private static Vector2 _runJumpBoostForce;
        private static float _runJumpBoostForceFalloffValue;

        private static Vector2 _waterForce;
        private static float _waterForceFalloffValue;


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void OnForceAccumulateAwake(AccumulateForceFalloffValue accumulateForceFalloffValue)
        {
            _gravity = accumulateForceFalloffValue.gravity;
            _inputForceFalloffValue = accumulateForceFalloffValue.inputForceFalloffValue;
            _runJumpBoostForceFalloffValue = accumulateForceFalloffValue.runJumpBoostForceFalloffValue;
            _waterForceFalloffValue = accumulateForceFalloffValue.waterForceFallofValue;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void OnForceAccumulateUpdate() {}
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void OnForceAccumulateFixedUpdate() => ApplyForces();
        
        public static void SetInputForce(Vector2 force)
        {
            if (force.magnitude < 0.1) return;
            _inputForce = force;
        }
        public static void SetGravityForce(Vector2 force) => _gravityForce = force;
        public static void SetRunJumpBoostForce(Vector2 force) => _runJumpBoostForce = force;
        public static void SetWaterForce(Vector2 force) => _waterForce = force;

        private static void ApplyForces()
        {
            _accumulatedForce = Vector2.zero;
            
            ApplyFalloff(ref _inputForce, _inputForceFalloffValue);
            _accumulatedForce += _inputForce;
            
            if (!GlobalFunctionsLibrary.IsGrounded(PlayerRigidbody) && !GlobalFunctionsLibrary.IsFloating(PlayerRigidbody)) AddMod(ref _gravityForce, Vector2.down, _gravity);
            else if (_gravityForce.y < 0) _gravityForce.y = 0;
            _accumulatedForce += _gravityForce;
            
            
            ApplyFalloff(ref _runJumpBoostForce, _runJumpBoostForceFalloffValue);
            _accumulatedForce += _runJumpBoostForce;

            _accumulatedForce += _waterForce;
            ApplyFalloff(ref _waterForce, _waterForceFalloffValue);

            if (_accumulatedForce.x > PlayerMaxVelocityX) _accumulatedForce.x = PlayerMaxVelocityX;
            if (_accumulatedForce.x < PlayerMinVelocityX) _accumulatedForce.x = PlayerMinVelocityX;
            if (_accumulatedForce.y > PlayerMaxVelocityY) _accumulatedForce.y = PlayerMaxVelocityY;
            if (_accumulatedForce.y < PlayerMinVelocityY)
            {
                if (!GlobalFunctionsLibrary.IsGrounded(PlayerRigidbody)) _accumulatedForce.y = 0;
                else _accumulatedForce.y = PlayerMinVelocityY;
            }
            PlayerRigidbody.linearVelocity = _accumulatedForce;
            Debug.Log("Player Velocity: "+ PlayerRigidbody.linearVelocity);
        }
        
        private static void ApplyFalloff(ref Vector2 source, float strength) => source = Vector2.Lerp(source, Vector2.zero, Time.fixedDeltaTime * strength);
        private static void AddMod(ref Vector2 source, Vector2 direction, float strength) => source += direction.normalized * (strength * Time.fixedDeltaTime);

    }
}