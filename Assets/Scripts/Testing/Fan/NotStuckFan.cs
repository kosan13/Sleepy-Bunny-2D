using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class NotStuckFan : MonoBehaviour
{
    public enum Directions
    {
        Up,
        Down,
        Left,
        Right
    }
    private const int PlayerLayer = 7;

    [SerializeField] private Directions fanDirection;
    [SerializeField] private float fanPower;
    [SerializeField] private float fanRange;
    [SerializeField] private bool onOff = true;
    public bool OnOff { get; private set; }

    private Rigidbody2D PlayerRigidbody;

    private void Start()
    {
        OnOff = onOff;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer != PlayerLayer) return;
        PlayerRigidbody = other.gameObject.GetComponent<Rigidbody2D>();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer != PlayerLayer) return;
        PlayerRigidbody = null;
    }

    private void FixedUpdate()
    {

        if (!OnOff) return;
        if (PlayerRigidbody is null) return;

        switch (fanDirection)
        {
            case Directions.Up:
                if (Physics2D.Raycast(transform.position, Vector2.up, fanRange, LayerMask.GetMask("Player")))
                {
                    PlayerRigidbody.AddForce(transform.up * fanPower);
                }
                Debug.DrawRay(transform.position, Vector2.up * fanRange, Color.green);
                break;
            case Directions.Down:
                if (Physics2D.Raycast(transform.position, Vector2.down, fanRange, LayerMask.GetMask("player")))
                {
                    PlayerRigidbody.AddForce(-transform.up * fanPower);
                }
                Debug.DrawRay(transform.position, Vector2.down * fanRange, Color.green);

                break;
            case Directions.Left:
                if (Physics2D.Raycast(transform.position, Vector2.left, fanRange, LayerMask.GetMask("player")))
                {
                    PlayerRigidbody.AddForce(-transform.right * fanPower);
                }
                Debug.DrawRay(transform.position, Vector2.left * fanRange, Color.green);

                break;
            case Directions.Right:
                if (Physics2D.Raycast(transform.position, Vector2.right, fanRange, LayerMask.GetMask("player")))
                {
                    PlayerRigidbody.AddForce(transform.right * fanPower);
                }
                Debug.DrawRay(transform.position, Vector2.right * fanRange, Color.green);

                break;
        }
    }

    //When press button call this method 
    public void setFanOnOff(bool value)
    {
        OnOff = value;
    }
}
