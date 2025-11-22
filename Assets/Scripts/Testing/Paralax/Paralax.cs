using UnityEngine;

public class SimpleParallax : MonoBehaviour
{
    [Header("Target (Camera or Player)")]
    public Transform target;  // drag your Camera or Player here in Inspector

    [Header("Parallax Amount")]
    [Range(0f, 1f)]
    public float parallaxSpeed = 0.5f;// 0 = no move, 1 = same as target

    Vector3 startPos;
    float startTargetX;
    

    void Start()
    {
        if (target == null)
        {
            target = Camera.main.transform; // fallback
        }

        startPos = transform.position;
        startTargetX = target.position.x;
    }

    void LateUpdate()
    {
        float deltaX = (target.position.x - startTargetX) * parallaxSpeed;

        transform.position = new Vector3(
            startPos.x + deltaX,
            startPos.y,
            startPos.z
        );
    }
}
