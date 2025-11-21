using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Splines;

public class StuckFan : MonoBehaviour
{
   
    private const int PlayerLayer = 7;

   
    [SerializeField] private float fanSpeed;
    [SerializeField] private float fanRange;
    [SerializeField] private bool onOff = true;
    [SerializeField] private bool hitPlayer = false;
    [SerializeField] private float distancePercentage;

    // here to fix the issue where when the player reaches the end it dosn't grab the player again.
    [SerializeField] private float workAroundTimer;
    [SerializeField] private GameObject player;

    [SerializeField] public SplineContainer spline;
    public bool OnOff { get; private set; }

    private Rigidbody2D PlayerRigidbody;

    private void Start()
    {
        OnOff = onOff;
        // getting the lentgh spline 
        fanRange = spline.CalculateLength();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (workAroundTimer != 0) return;
        if (other.gameObject.layer != PlayerLayer) return;
        PlayerRigidbody = other.gameObject.GetComponent<Rigidbody2D>();
        hitPlayer = true;

        //Need this for the update
        player = other.gameObject;

        PlayerRigidbody.gravityScale = 0;

        //Would need some code that can figure out how far into the fan range the player is so they start there.
        //Something like: distancePrecentage = Player.postion (something) Stuckfan.postion / fanRange

    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer != PlayerLayer) return;
        PlayerRigidbody = null;
        hitPlayer = false;
    }

    private void FixedUpdate()
    {
        if(workAroundTimer != 0)
        workAroundTimer -= 1 * UnityEngine.Time.deltaTime;

        //fixing a bug, where the timer would not reset
        if (workAroundTimer < 0)
            workAroundTimer = 0;
        if (workAroundTimer > 0) return;
        if (!OnOff) return;
        if (PlayerRigidbody is null) return;

        distancePercentage += fanSpeed * UnityEngine.Time.deltaTime / fanRange;

        Vector3 currentPosition = spline.EvaluatePosition(distancePercentage);
        player.transform.position = currentPosition;

        if (distancePercentage > 1f)
        {
            hitPlayer = false;
            distancePercentage = 0f;
            workAroundTimer = 5f;
            //Gravitry scale is currently hard coded. 
            PlayerRigidbody.gravityScale = 3.5f;
        }



    }


    //When press button call this method 
    public void setFanOnOff(bool value)
    {
        OnOff = value;
    }
}
