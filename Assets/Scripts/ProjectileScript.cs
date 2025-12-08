using Unity.VisualScripting;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    private bool collided = false;

    void OnCollisionEnter(Collision collision)
    {
        if(!collided && collision.gameObject.tag == "World"){
            collided = true;
            Destroy(gameObject);
        }
    }
}
