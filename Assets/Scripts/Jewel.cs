using UnityEngine;

public class Jewel : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 60f;
    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIManager.Instance.TriggerVictory();
            Destroy(this.gameObject);
        }
    }
}