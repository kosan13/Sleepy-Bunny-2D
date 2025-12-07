using UnityEngine;

public class VinjetFlicker : MonoBehaviour
{
    private SpriteRenderer spriterender;
    [SerializeField] Sprite[] flickerStates;

    // [SerializeField] GameObject[] Cinemachine; No clue how to fix that ask mina perhaps?
    private Sprite newState;

    [SerializeField] int flickerSpeed;

    

    private int CurrentSprite;

    private int flickerTime;

    private void Start()
    {
        flickerTime = flickerSpeed;
        spriterender = GetComponent<SpriteRenderer>();
        
    }
    private void Update()
    {

        if(flickerTime <= 0)
        {

            if (CurrentSprite == 2)
            {
                Debug.Log(flickerStates.Length + "crurrent Sprite");
                CurrentSprite = 0;
                return;
            }
            flickerTime = flickerSpeed;

            newState = flickerStates[CurrentSprite];
            spriterender.sprite = newState;


            CurrentSprite++;
            return;
        }

        flickerTime--;
        
    }
}
