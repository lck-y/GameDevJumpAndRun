using UnityEngine;

public class RespawnTrigger : MonoBehaviour
{
    [SerializeField]
    private Transform respawnPoint;

    void OnTriggerEnter(Collider other)
    {
        CharacterController cc = other.gameObject.GetComponent<CharacterController>();
        if (cc != null)
        {
            Respawn(cc);
        }
    }

    void Respawn(CharacterController cc)
    {
        cc.enabled = false;
        cc.transform.position = respawnPoint.position;
        cc.enabled = true;
    }
}