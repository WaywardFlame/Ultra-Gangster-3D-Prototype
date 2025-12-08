using UnityEngine;

public class bossDetectionArea : MonoBehaviour
{
    public BossEnemy host;
    public bool playerFound = false;

    void OnTriggerEnter(Collider other)
    {
        if(!playerFound && other.CompareTag("Player")){
            playerFound = true;
            host.playerDetected = true;
        }
    }
}
