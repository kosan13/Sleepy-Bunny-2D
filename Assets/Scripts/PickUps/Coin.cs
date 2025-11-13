using ScoreSystem;
using UnityEngine;

namespace PickUps
{
    public class Coin : PickUp
    {
        [SerializeField] private int value;
        public override void OnPickUp(Collision2D collisionObject) => ScoreSystemHandler.ScoreSystem.AddScore(value);
        public override void OnAfterPickUp(Collision2D collisionObject) {}
    }
}