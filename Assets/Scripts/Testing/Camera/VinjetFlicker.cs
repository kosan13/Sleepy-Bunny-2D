using UnityEngine;

public class VinjetFlicker : MonoBehaviour
{
    [SerializeField] Sprite[] flickerStates;

    // [SerializeField] GameObject[] Cinemachine; No clue how to fix that ask mina perhaps?
    private Sprite newState;

    [SerializeField] int flickerSpeed;

    public GameObject self;

    public int CurrentSprite;

    private int flickerSpeedCashed;

    private void Start()
    {
        flickerSpeedCashed = flickerSpeed;
        
    }
    private void FixedUpdate()
    {

        if(flickerSpeed <= 0)
        {

            if (CurrentSprite == 2)
            {
                Debug.Log(flickerStates.Length + "crurrent Sprite");
                CurrentSprite = 0;
                return;
            }
            flickerSpeed = flickerSpeedCashed;

            newState = flickerStates[CurrentSprite];
            self.GetComponent<SpriteRenderer>().sprite = newState;



            CurrentSprite++;
            return;
        }

        flickerSpeed--;
        
    }
}
