using System.Drawing;
using UnityEngine;
using UnityEngine.Device;

/*
How to get this code to work.
For this code to work you need to add a few things to the player in the unity editior
First you need to take the immiges you wish to use as foregrounds and place them as children to the camera. (which you can find in the player prefab)
Then you place the foreground at either above the middle of the camera or below it. The thoufther you put it the more a plaer has to do to reach it

Then check in the box if the object should be above or bellow the player with CellinForeground

if you want to use multiple foregrounds place them next to eachother each with this code.

Lastly set a peralex scrolling value. I have set it to be 0.5f at default. but you may change this number. (though stay in the disimals)
*/
public class RightParalex : MonoBehaviour
{
    //starting values for the paralex scroling, used to ancer them to a place in the game world
    private float startposX;
    private float startposY;

    //The player
    private GameObject Player;
    //Used to find the render
    public GameObject Self;

    //Two special objects placed on the player. There becouse I coldn't find a way to grab the screens edges in world transform
    private GameObject Top;
    private GameObject Bottom;

    //Imuages and its size
    private Renderer Render;

    //is the foreground gorounded or not?
    public bool CellingForeground = false;

    //Check for if statments
    private bool Stuck;

    //Value saved for a if statment check
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
        
        


        startposX = transform.position.x;
        startposY = transform.position.y;

    }

    
    void FixedUpdate()
    {
        //How much the foreground moves when moved by the paralexScrolling (is negative to get the right motion)
        float distx = (Player.transform.position.x * -paralexEffect);
        float disty = (Player.transform.position.y * -paralexEffect);


        // X should allways be modified to keep the foreground in position
        transform.position = new Vector3(startposX + distx, transform.position.y, 0);

        //The foreground should not be able to moveup or down beyond thir image.
        //To fix that I have made a series of if statments that makes sure they don't

        //This one checks to see if the foreground should be at the top of bottom of the screen
        if (CellingForeground)
        {
            //Checks if the threshold for the image is broken. The 0.5f is needed for a later point
            if (Top.transform.position.y + 0.5f > Render.bounds.max.y)
            {
                //Debug.Log("Too low");

                //if the foreground just now reached its treshold then trigger this code.
                if (!Stuck)
                {
                    //Save the y position the treshold brake happend at
                    cacheY = disty;
                    //Set the foregrounds position to the edge of the screen. Here we get a bug if we don't have the 0.5f in the previus if statment
                    transform.position = new Vector3(transform.position.x, Top.transform.position.y - (0.5f * Render.bounds.size.y), 0);
                    //Then don't re trigger this code
                    Stuck = true;
                }

                //If we are stuck check if we have returned to accsepteble levels.
                if (Stuck)
                {
                    //if we return to accseptable peramiters
                    if (disty > cacheY)
                    {
                        //Move the foreground to the new peramiters
                        transform.position = new Vector3(transform.position.x, startposY + disty, 0);
                        //Reload the stuck trigger
                        Stuck = false;

                    }
                }
                // If we are stuck then we shall not move the foreground in the y axies
                return;
            }

            //if we are not stuck move the foregorund in the Y axies
            transform.position = new Vector3(transform.position.x, startposY + disty, 0);

            
        }

        
        //Almost the same code as the previus one, but with diffrent peremiters. !!! THIS CAN NOT BE REMOVED AND SLAPED INTO THE PREVIUS IF STATMENTS WITH OUT EDITING IT -, < AND "BOTTOM" ARE INPORTANT !!!
        else if (!CellingForeground)
        {

            if (Bottom.transform.position.y - 0.5f < Render.bounds.min.y)
            {
                //Debug.Log("Too high");
                
                if (!Stuck)
                {
                    Debug.Log("render" + Render.bounds.size.y);
                    cacheY = disty;
                    Stuck = true;
                    transform.position = new Vector3(transform.position.x,Bottom.transform.position.y + (0.5f * Render.bounds.size.y), 0);
                }

                if (Stuck)
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
