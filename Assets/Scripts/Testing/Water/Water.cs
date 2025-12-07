using Player;
using Player.Movement;
using UnityEngine;
using static Global.GlobalVariablesLibrary;

public class Water : MonoBehaviour
{
    private bool WaterTouch = false;
    private bool flipFlop = false;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer != PlayerLayer) return;

        WaterTouch = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer != PlayerLayer) return;

        WaterTouch = false;
    }

    private void FixedUpdate()
    {
        
        if (!WaterTouch) return;
        if(flipFlop)
        {
            flipFlop = false;
            ForceAccumulate.SetWaterForce(PlayerController.GetPlayerController.WaterPush);
        }
        else
        {
            flipFlop= true;
            Vector2 force = PlayerController.GetPlayerController.WaterPush;
            force.y -= 10;
            ForceAccumulate.SetWaterForce(force);
        }
    }
}