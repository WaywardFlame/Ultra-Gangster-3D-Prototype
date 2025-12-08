using UnityEngine;

public class GrenadeScript : MonoBehaviour
{
    bool GoingToExplode = false;
    bool Exploding = false;
    float explosionTimer = 0f;
    float explodingTimer = 0f;

    public float secondsToExplode;
    public float secondsExploding;
    public GameObject ExplosionArea;

    public AudioSource audioSource;
    public AudioClip explosionSound;

    void Update()
    {
        if(GoingToExplode){
            explosionTimer += Time.deltaTime;
        }

        if(explosionTimer >= secondsToExplode){
            Exploding = true;
        }

        if(Exploding){
            ExplosionArea.SetActive(true);
            explodingTimer += Time.deltaTime;
        }

        if(explodingTimer >= secondsExploding){
            ExplosionArea.SetActive(false);
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("World") || collision.gameObject.CompareTag("Enemy")){
            GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            GoingToExplode = true;
            audioSource.resource = explosionSound;
            audioSource.Play();
        }
    }
}
