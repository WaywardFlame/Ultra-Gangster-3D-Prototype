using UnityEngine;
using UnityEngine.AI;

public class BossEnemy : MonoBehaviour
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
    public Transform FirePoint1;
    public Transform FirePoint2;
    public float projectileSpeed;
    public float timeToFire = 0;
    public float fireRate = 0.25f;
    public AudioSource audioSource;
    public AudioClip bulletSound;

    private NavMeshAgent agent;
    private bool SecondPhase = false;
    private float armorTimer = 0f;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    } 

    void Update()
    {
        Debug.Log("health: " + health + " armor: " + armor);
        if(playerDetected){
            LookTowardsPlayer();
            if(Vector3.Distance(transform.position, player.position) < 8){
                //MoveTowardsPlayer();
                agent.destination = transform.position;
            } else {
                MoveTowardsPlayer();
            }
            FireBullet();

            if(SecondPhase){
                RegenArmor();
            }

            Vector3 pos = transform.position;
            pos.y = 2.6f;
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
            GameObject projectile1 = Instantiate(Bullet, FirePoint1.position, Quaternion.identity) as GameObject;
            projectile1.tag = "EnemyBullet";
            projectile1.GetComponent<Rigidbody>().linearVelocity = (player.position - FirePoint1.position).normalized * projectileSpeed;
            audioSource.resource = bulletSound;
            audioSource.Play();

            if(SecondPhase){
                GameObject projectile2 = Instantiate(Bullet, FirePoint2.position, Quaternion.identity) as GameObject;
                projectile2.tag = "EnemyBullet";
                projectile2.GetComponent<Rigidbody>().linearVelocity = (player.position - FirePoint2.position).normalized * projectileSpeed;
                audioSource.Play();
            }

            timeToFire = 0;
        }
    }

    void RegenArmor(){
        if(armor <= 0){
            armorTimer += Time.deltaTime;
        }

        if(armorTimer >= 10){
            armor = 100;
            ArmorPlate.SetActive(true);
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

        if(!SecondPhase && health <= 500){
            SecondPhase = true;
            armor = 100;
            ArmorPlate.SetActive(true);
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

        if(!SecondPhase && health <= 500){
            SecondPhase = true;
            armor = 100;
            ArmorPlate.SetActive(true);
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
