using UnityEngine;

public class detectionAreaArmored : MonoBehaviour
{
    public ArmoredEnemy host;
    public bool playerFound = false;

    void OnTriggerEnter(Collider other)
    {
        if(!playerFound && other.CompareTag("Player")){
            playerFound = true;
            host.playerDetected = true;
        }
    }
}
