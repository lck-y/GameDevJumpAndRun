using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private float speed = 1f;
    private float edging = 1f;
    private float walling = 0.5f;
    private Vector3 movement;
    private float stuckTimer = 5f;
    
    [SerializeField] private LayerMask platformLayer;
    
    void Awake()
    {
        movement = RandomDirection();
    }

    Vector3 RandomDirection()
    {
        int step = Random.Range(0, 8);
        return Quaternion.Euler(0, step * 45f, 0) * Vector3.forward;
    }

    void FixedUpdate()
    {
        stuckTimer -= Time.deltaTime;
        Vector3 edgeCheck = transform.position + movement * edging + new Vector3(0, 2, 0) ;
        Vector3 wallCheck = transform.position - transform.forward * 0.5f + transform.up * 0.25f;  
        bool groundThere = Physics.Raycast(edgeCheck, Vector3.down, 5f, platformLayer);
        bool wallThere = Physics.Raycast(wallCheck, movement, walling, platformLayer);

        if (!groundThere || wallThere)
        {
            movement = -movement;
            movement = Quaternion.Euler(0, Random.Range(-90f, 90f), 0) * movement;
        }

        if (stuckTimer <= 0f)
        {
            movement = -movement;
            movement = Quaternion.Euler(0, Random.Range(-90f, 90f), 0) * movement;
            stuckTimer = 5f;
        }

        transform.position += movement * speed * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(movement);

        /*
        Debug.DrawRay(wallCheck, movement , Color.red);
        Debug.Log(groundThere);
        Debug.Log(wallThere);
        */
    }
    
}
