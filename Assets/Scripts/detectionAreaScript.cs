using UnityEngine;

public class detectionAreaScript : MonoBehaviour
{
    public BasicEnemy host;
    public bool playerFound = false;

    void OnTriggerEnter(Collider other)
    {
        if(!playerFound && other.CompareTag("Player")){
            playerFound = true;
            host.playerDetected = true;
        }
    }
}
