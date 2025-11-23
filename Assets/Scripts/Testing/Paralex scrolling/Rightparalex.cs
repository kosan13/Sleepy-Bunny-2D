using UnityEngine;

public class RightParalex : MonoBehaviour
{
    private float lenght, startpos;
    private GameObject Cam;

    public float paralexEffect;

    public GameObject rightside;
    public GameObject middle;
    public GameObject leftside;
   

    private bool rightTwice;
    private bool leftTwice;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject player = GameObject.Find("player");
        GameObject Right = GameObject.Find("Blured foreground 2");
        GameObject Middle = GameObject.Find("Blured foreground 1");
        GameObject Left = GameObject.Find("Blured foreground 3");

        Cam = player;
        rightside = Right;
        middle = Middle;
       

        startpos = transform.position.x;
        lenght = GetComponent<SpriteRenderer>().bounds.size.x;
        rightside = GetComponent<GameObject>();
        middle = GetComponent<GameObject>();
       
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float temp = (Cam.transform.position.x * (1 - paralexEffect));
        float dist = (Cam.transform.position.x * -paralexEffect);

        transform.position = new Vector3(startpos + dist, transform.position.y, 0);

       

        // Does not work do not know why
        /*
        // Used to repeat infenetly
        if (temp > startpos + lenght)
        {
            if (rightTwice)
            {
                middle.transform.position = new Vector2(startpos += 2 * lenght, 0);
                return;
            }
            rightside.transform.position = new Vector2(startpos += 2 * lenght, 0);

        }
        else if (temp < startpos - lenght)
        {
            if (leftTwice)
            {
                middle.transform.position = new Vector2(startpos += -2 * lenght, 0);
                return;
            }
            rightside.transform.position = new Vector2(startpos -= 2 * lenght, 0);
        }
        */

    }
}
