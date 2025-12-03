using Unity.Cinemachine;
using UnityEngine;


public class BecomeCamera : MonoBehaviour
{
    private bool currentCamera = false;
    private bool secondHit = false; //due to the players hit boxes the game regirsters players hiting this twice. Need a fix for final version

    private const int PlayerLayer = 7;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Work damit");
        if (other.gameObject.layer != PlayerLayer) return;

        if (!currentCamera)
        {
            if (!secondHit)
            {
                secondHit = true;
                return;
            }
            gameObject.GetComponent<CinemachineCamera>().Priority = 1;
            currentCamera = true;
            secondHit = false;
        }

        if (currentCamera)
        {
            if (!secondHit)
            {
                secondHit = true;
                return;
            }
            gameObject.GetComponent<CinemachineCamera>().Priority = -1;
            currentCamera = false;
            secondHit = false;
        }
    }
}
