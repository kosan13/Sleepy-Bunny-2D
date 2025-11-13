using System.Runtime.CompilerServices;
using UnityEngine;

namespace ScoreSystem
{
    public class ScoreSystemHandler : MonoBehaviour
    {
        public static ScoreSystemHandler ScoreSystem { get; private set; }
        
        public int Score { get; private set; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OnEnable() => ScoreSystem = this;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OnDisable() => ScoreSystem = ScoreSystem == this ? null : ScoreSystem;

        
        public void SetScore(int newScore) => Score = newScore;
        public void AddScore(int value) => Score += value;
        public void RemoveScore(int value) => Score -= value;
    }
}