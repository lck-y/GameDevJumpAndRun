using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    [SerializeField] private Transform checkpointPosition;
    [SerializeField] private RespawnTrigger respawnTrigger;

    void OnTriggerEnter(Collider other)
    {
        CharacterController cc = other.gameObject.GetComponent<CharacterController>();
        if (cc != null)
        {
            respawnTrigger.SetRespawnPoint(checkpointPosition);
            cc.GetComponent<Character>().spawnPosition = checkpointPosition.position;
        }
    }
}