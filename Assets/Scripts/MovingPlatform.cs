using Unity.VisualScripting;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private float platformSpeed;
    [SerializeField] private Vector3 start;
    [SerializeField] private Vector3 end;
    
    [SerializeField] private bool startsActive = true;
    private bool isActive = false;
    private float elapsedTime = 0.0f;
    
    private Vector3 velocity;
    private Vector3 lastPosition;

    void Start()
    {
        lastPosition = this.transform.localPosition;
        this.isActive = startsActive;
    }
    void FixedUpdate()
    {
        if (!isActive) return;
        elapsedTime += Time.deltaTime;
        float pingPong = Mathf.PingPong(elapsedTime * this.platformSpeed, 1.0f);
        var newPosition = Vector3.Lerp(this.start, this.end, pingPong);
        this.velocity = (newPosition - this.transform.localPosition) / Time.fixedDeltaTime;
        this.transform.localPosition = newPosition;
        lastPosition = newPosition;
    }
    
    public void SetActive(bool active)
    {
        this.isActive = active;
    }

    public Vector3 GetVelocity()
    {
        return this.velocity;
    }
}