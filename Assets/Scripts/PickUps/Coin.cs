using ScoreSystem;
using UnityEngine;

namespace PickUps
{
    public class Coin : PickUp
    {
        [SerializeField] private int value;
        public override void OnPickUp(Collider2D collisionObject)
        {
            // ScoreSystemHandler.ScoreSystem.AddScore(value);
        }

        public override void OnAfterPickUp(Collider2D collisionObject) {}
    }
}