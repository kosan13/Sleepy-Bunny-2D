using System.Drawing;
using UnityEngine;
using UnityEngine.Device;

public class RightParalex : MonoBehaviour
{
    private float startposX;
    private float startposY; 
    private GameObject Player;
    public GameObject Self;
    private GameObject Top;
    private GameObject Bottom;

    private Renderer Render;

    public Camera Cam;

    //is the foreground gorounded or not?
    public bool CellingForeground = false;

    private bool Stuck;
    private float cacheY;

    //How much the foreground should move
    public float paralexEffect = 0.5f;
   

  
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject player = GameObject.Find("player");
        GameObject top = GameObject.Find("CamraTop");
        GameObject bottom = GameObject.Find("CameraBottom");

        Render = Self.GetComponent<Renderer>();


        Top = top;
        Bottom = bottom;
        Player = player;
        
        Cam = Camera.main;

        startposX = transform.position.x;
        startposY = transform.position.y;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //How much the foreground moves when moved by the paralexScrolling
        float distx = (Player.transform.position.x * -paralexEffect);
        float disty = (Player.transform.position.y * -paralexEffect);

        transform.position = new Vector3(startposX + distx, transform.position.y, 0);
        if (CellingForeground)
        {
           if (Top.transform.position.y > Render.bounds.max.y)
           {
                Debug.Log("Too low");
                if (!Stuck)
                {
                    cacheY = disty;
                    Stuck = true;
                }

                else if (Stuck)
                {
                    if(disty > cacheY)
                    {
                        transform.position = new Vector3(transform.position.x, startposY + disty, 0);
                        Stuck = false;
                        transform.position = new Vector3(transform.position.x, Top.transform.position.y - (0.5f * Render.bounds.size.y), 0);
                    }
                }
                return;
           }

                transform.position = new Vector3(transform.position.x, startposY + disty, 0);

            
        }

        

        else if (!CellingForeground)
        {

            if (Bottom.transform.position.y - 0.5f < Render.bounds.min.y)
            {
                Debug.Log("Too high");
                
                if (!Stuck)
                {
                    Debug.Log("render" + Render.bounds.size.y);
                    cacheY = disty;
                    Stuck = true;
                    transform.position = new Vector3(transform.position.x,Bottom.transform.position.y + (0.5f * Render.bounds.size.y), 0);
                }

                else if (Stuck)
                {
                    if (disty < cacheY)
                    {
                        transform.position = new Vector3(transform.position.x, startposY + disty, 0);
                        Stuck = false;
                    }
                }
                return;
            }
            Debug.Log(disty);

            transform.position = new Vector3(transform.position.x, startposY + disty, 0);
        }



    }
}
