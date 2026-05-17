using UnityEngine;

public class RespawnTrigger : MonoBehaviour
{
    [SerializeField]
    private Transform respawnPoint;

    void OnTriggerEnter(Collider other)
    {
        Character c = other.gameObject.GetComponent<Character>();
        if (c != null)
        {
            c.InflictDamage(25f);
            CharacterController cc = other.gameObject.GetComponent<CharacterController>();
            cc.enabled = false;
            cc.transform.position = respawnPoint.position;
            cc.enabled = true;
        }
    }

    public void SetRespawnPoint(Transform newPoint)
    {
        this.respawnPoint = newPoint;
    }
}