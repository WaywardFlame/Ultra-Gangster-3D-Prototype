using UnityEngine;
using UnityEngine.AI;

public class ArmoredEnemy : MonoBehaviour
{
    public float speed;
    public int health;
    public int armor;
    public int ammoClip;
    public int ammoAmount;
    public Transform player;
    public bool playerDetected = false;
    public GameObject Bullet;
    public GameObject ArmorPlate;
    public Transform FirePoint;
    public float projectileSpeed;
    public float timeToFire = 0;
    public float fireRate = 0.25f;
    public AudioSource audioSource;
    public AudioClip bulletSound;

    private NavMeshAgent agent;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    } 

    void Update()
    {
        if(playerDetected){
            LookTowardsPlayer();
            if(Vector3.Distance(transform.position, player.position) < 8){
                //MoveTowardsPlayer();
                agent.destination = transform.position;
            } else {
                MoveTowardsPlayer();
            }
            FireBullet();

            Vector3 pos = transform.position;
            pos.y = 1.6f;
            transform.position = pos;
        }
    }

    void LookTowardsPlayer(){
        Quaternion lookRotation = Quaternion.LookRotation((player.position - transform.position).normalized);
        transform.rotation = lookRotation;
    }

    void MoveTowardsPlayer(){
        //controller.Move( ( (player.position - transform.position).normalized * speed ) * Time.deltaTime );
        agent.destination = player.position;
    }

    void FireBullet(){
        if(timeToFire < fireRate){
            timeToFire += Time.deltaTime;
        } else {
            GameObject projectile = Instantiate(Bullet, FirePoint.position, Quaternion.identity) as GameObject;
            projectile.tag = "EnemyBullet";
            projectile.GetComponent<Rigidbody>().linearVelocity = (player.position - FirePoint.position).normalized * projectileSpeed;
            timeToFire = 0;
            audioSource.resource = bulletSound;
            audioSource.Play();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("PlayerBullet")){
            if(armor > 0){
                health -= 15;
                armor -= 10;
                ArmorCheck();
            } else {
                health -= 25;
            }
            DetectPlayer();
            Destroy(collision.gameObject);
        } else if(collision.gameObject.CompareTag("PlayerPellet")){
            if(armor > 0){
                health -= 40;
                armor -= 10;
                ArmorCheck();
            } else {
                health -= 100;
            }
            DetectPlayer();
            Destroy(collision.gameObject);
        }

        if(health <= 0){
            GlobalVariableStorage.EnemiesDead++;
            Debug.Log("GlobalVariableStorage Enemies Dead is " + GlobalVariableStorage.EnemiesDead);
            Destroy(gameObject);
        }
    } 

    void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("ThrownGrenade")){
            if(armor > 0){
                health -= 50;
                armor -= 25;
                ArmorCheck();
            } else {
                health -= 100;
            }
            DetectPlayer();
        } else if(other.gameObject.CompareTag("Thermite")){
            if(armor > 0){
                health -= 25;
                armor -= 75;
                ArmorCheck();
            } else {
                health -= 50;
            }
            DetectPlayer();
        } else if(other.gameObject.CompareTag("PlayerGrenade")){
            if(armor > 0){
                health -= 100;
                armor -= 25;
                ArmorCheck();
            } else {
                health -= 200;
            }
            DetectPlayer();
        }

        if(health <= 0){
            GlobalVariableStorage.EnemiesDead++;
            Debug.Log("GlobalVariableStorage Enemies Dead is " + GlobalVariableStorage.EnemiesDead);
            Destroy(gameObject);
        }
    }

    void ArmorCheck(){
        if(armor <= 0){
            armor = 0;
            ArmorPlate.SetActive(false);
        }
    }

    void DetectPlayer(){
        if(!playerDetected){
            playerDetected = true;
        }
    }
}
