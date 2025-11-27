using System.Runtime.CompilerServices;
using Global;
using UnityEngine;

using static Global.GlobalVariablesLibrary;

namespace Player.Movement
{
    public static class ForceAccumulate
    {
        private static float _gravity;
        private static Vector2 _accumulatedForce;
        
        private static Vector2 _inputForce;
        private static float _inputForceFalloffValue;
        
        private static Vector2 _gravityForce;
        
        private static Vector2 _runJumpBoostForce;
        private static float _runJumpBoostForceFalloffValue;


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void OnForceAccumulateAwake(float gravity, float inputForceFalloffValue, float runJumpBoostForceFalloffValue)
        {
            _gravity = gravity;
            _inputForceFalloffValue = inputForceFalloffValue;
            _runJumpBoostForceFalloffValue = runJumpBoostForceFalloffValue;
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
        
        private static void ApplyForces()
        {
            _accumulatedForce = Vector2.zero;
            
            ApplyFalloff(ref _inputForce, _inputForceFalloffValue);
            _accumulatedForce += _inputForce;
            
            if (!GlobalFunctionsLibrary.IsGrounded(PlayerRigidbody)) AddMod(ref _gravityForce, Vector2.down, _gravity);
            else if (_gravityForce.y < 0) _gravityForce.y = 0;
            _accumulatedForce += _gravityForce;
            
            ApplyFalloff(ref _runJumpBoostForce, _runJumpBoostForceFalloffValue);
            _accumulatedForce += _runJumpBoostForce;
            
            PlayerRigidbody.linearVelocity = _accumulatedForce;
        }
        
        private static void ApplyFalloff(ref Vector2 source, float strength) => source = Vector2.Lerp(source, Vector2.zero, Time.fixedDeltaTime * strength);
        private static void AddMod(ref Vector2 source, Vector2 direction, float strength) => source += direction.normalized * (strength * Time.fixedDeltaTime);

    }
}