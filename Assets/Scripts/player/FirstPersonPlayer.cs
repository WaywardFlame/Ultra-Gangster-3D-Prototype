using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class FirstPersonPlayer : MonoBehaviour
{
    [Header("Stats Variables")]
    public int health;
    public int armor;
    public int ammoClip = 10;
    public int ClipAmount = 10;
    public int ammoAmount = 1000;

    [Header("Weapon Variables")]
    public int CurrentWeapon = 1;
    public int CurrentItem = 1;
    public float FireRate = 0.2f;
    public GameObject PistolModel;
    public int pistolAmmo = 1000;
    public int pistolClip = 10;
    public bool hasPistol = true;
    public GameObject RifleModel;
    public int rifleAmmo = 1000;
    public int rifleClip = 30;
    public bool hasRifle = false;
    public GameObject ShotgunModel;
    public int shotgunAmmo = 1000;
    public int shotgunClip = 6;
    public bool hasShotgun = false;
    public GameObject LauncherModel;
    public int launcherAmmo = 10;
    public int launcherClip = 1;
    public bool hasLauncher = false;
    public GameObject ThrowGrenade;
    public int ThrowGrenades = 0;
    public GameObject ThermiteCharge;
    public int Thermites = 0;

    [Header("Movement Variables")]
    //public int FPS = 60;
    public CharacterController controller;
    public float speed = 10f;
    public float gravity = -20f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    [Header("Shooting Variables")]
    public Camera cam; 
    public GameObject Bullet;
    public GameObject Pellet;
    public GameObject LaunchedGrenade;
    public float projectileSpeed; 
    public Transform FirePoint;

    [Header("UI Variables")]
    public TextMeshProUGUI healthNum;
    public TextMeshProUGUI armorNum;
    public TextMeshProUGUI ammoNum;
    public TextMeshProUGUI itemNum;
    public GameObject UI;
    public GameObject PauseMenu;

    [Header("Audio Variables")]
    public AudioSource audioSource;
    public AudioClip bulletSound, shotgunSound, launcherSound, walkSound;

    Vector3 velocity = Vector3.zero;
    bool isGrounded;
    Vector3 pDestination;
    float TimeToFire = 0f;

    void Start()
    {
        // Application.targetFrameRate = FPS;
        // //QualitySettings.vSyncCount = 1;
        healthNum.text = "" + health;
        ammoNum.text = "" + ammoClip + "\n-----\n" + ammoAmount;
        itemNum.text = "Grenades" + "\n-----\n" + ThrowGrenades;
    }

    void Update()
    {
        // Application.targetFrameRate = FPS;
        PlayerMovement(); // player movement logic here
        FireAndReloadWeapon(); // weapon firing and reloading here
        ThrowItem(); // item throwing logic here
        WeaponSwap(); // swapping weapons and swapping items here
        if(Input.GetKeyDown(KeyCode.Escape)){
            Time.timeScale = 0;
            UI.SetActive(false);
            PauseMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void ResumeGame(){
        PauseMenu.SetActive(false);
        UI.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
    }

    void PlayerMovement(){
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if(isGrounded && velocity.y < 0){
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = (transform.right * x + transform.forward * z).normalized;
        controller.Move(move * speed * Time.deltaTime);
        velocity.y -= gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        
        // audioSource.resource = walkSound;
        // audioSource.Play();
    }

    void FireAndReloadWeapon(){
        if(ammoClip > 0 && TimeToFire >= FireRate){ // if ammo available
            if(CurrentWeapon == 1 && Input.GetKeyDown(KeyCode.Mouse0)){ // on left mouse click, fire pistol
                FireBullet();
                audioSource.resource = bulletSound;
                audioSource.Play();
            } else if(CurrentWeapon == 2 && Input.GetKey(KeyCode.Mouse0)){ // on holding down left mouse, fire rifle
                FireBullet();
                audioSource.resource = bulletSound;
                audioSource.Play();
            } else if(CurrentWeapon == 3 && Input.GetKeyDown(KeyCode.Mouse0)){ // fire shotgun
                FireBullet();
                audioSource.resource = shotgunSound;
                audioSource.Play();
            } else if(CurrentWeapon == 4 && Input.GetKeyDown(KeyCode.Mouse0)){ // fire grenade launcher
                FireBullet();
                audioSource.resource = launcherSound;
                audioSource.Play();
            }
        } else {
            TimeToFire += Time.deltaTime;
        }
        if(Input.GetKeyDown(KeyCode.R) && ammoClip < ClipAmount){ // reload weapon if ammo clip isn't full
            ReloadWeapon();
        }
    }

    void ThrowItem(){
        if(Input.GetKeyDown(KeyCode.Mouse1)){
            pDestination = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)).GetPoint(1000);
            if(CurrentItem == 1 && ThrowGrenades > 0){
                GameObject projectile = Instantiate(ThrowGrenade, FirePoint.position, Quaternion.identity) as GameObject;
                projectile.tag = "ThrownGrenade";
                projectile.GetComponent<Rigidbody>().linearVelocity = (pDestination - FirePoint.position).normalized * projectileSpeed;
                ThrowGrenades -= 1;
                itemNum.text = "Grenades" + "\n-----\n" + ThrowGrenades;
            } else if(CurrentItem == 2 && Thermites > 0) {
                GameObject projectile = Instantiate(ThermiteCharge, FirePoint.position, Quaternion.identity) as GameObject;
                projectile.tag = "Thermite";
                projectile.GetComponent<Rigidbody>().linearVelocity = (pDestination - FirePoint.position).normalized * projectileSpeed;
                Thermites -= 1;
                itemNum.text = "Thermite" + "\n-----\n" + Thermites;
            }
        }
    }

    void WeaponSwap(){
        if(Input.GetKeyDown(KeyCode.Alpha1) && hasPistol){
            StoreAmmoInfo();
            CurrentWeapon = 1;
            ClipAmount = 10;
            FireRate = 0.2f;
            ammoClip = pistolClip;
            ammoAmount = pistolAmmo;
            PistolModel.SetActive(true);
            RifleModel.SetActive(false);
            ShotgunModel.SetActive(false);
            LauncherModel.SetActive(false);
        } else if(Input.GetKeyDown(KeyCode.Alpha2) && hasRifle){
            StoreAmmoInfo();
            CurrentWeapon = 2;
            ClipAmount = 30;
            FireRate = 0.1f;
            ammoClip = rifleClip;
            ammoAmount = rifleAmmo;
            RifleModel.SetActive(true);
            PistolModel.SetActive(false);
            ShotgunModel.SetActive(false);
            LauncherModel.SetActive(false);
        } else if(Input.GetKeyDown(KeyCode.Alpha3) && hasShotgun){
            StoreAmmoInfo();
            CurrentWeapon = 3;
            ClipAmount = 6;
            FireRate = 0.3f;
            ammoClip = shotgunClip;
            ammoAmount = shotgunAmmo;
            ShotgunModel.SetActive(true);
            RifleModel.SetActive(false);
            PistolModel.SetActive(false);
            LauncherModel.SetActive(false);
        } else if(Input.GetKeyDown(KeyCode.Alpha4) && hasLauncher){
            StoreAmmoInfo();
            CurrentWeapon = 4;
            ClipAmount = 1;
            FireRate = 1f;
            ammoClip = launcherClip;
            ammoAmount = launcherAmmo;
            LauncherModel.SetActive(true);
            PistolModel.SetActive(false);
            RifleModel.SetActive(false);
            ShotgunModel.SetActive(false);
        }

        if(Input.GetKeyDown(KeyCode.E)){
            if(CurrentItem == 1){
                CurrentItem = 2;
                itemNum.text = "Thermites" + "\n-----\n" + Thermites;
            } else {
                CurrentItem = 1;
                itemNum.text = "Grenades" + "\n-----\n" + ThrowGrenades;
            }
        }
        ammoNum.text = "" + ammoClip + "\n-----\n" + ammoAmount;
    }

    void StoreAmmoInfo(){
        if(CurrentWeapon == 1){
            pistolAmmo = ammoAmount;
            pistolClip = ammoClip;
        } else if(CurrentWeapon == 2){
            rifleAmmo = ammoAmount;
            rifleClip = ammoClip;
        } else if(CurrentWeapon == 3){
            shotgunAmmo = ammoAmount;
            shotgunClip = ammoClip;
        } else if(CurrentWeapon == 4){
            launcherAmmo = ammoAmount;
            launcherClip = ammoClip;
        }
    }

    void FireBullet(){
        TimeToFire = 0;
        pDestination = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)).GetPoint(1000);
        GameObject projectile;
        if(CurrentWeapon == 1 || CurrentWeapon == 2){
            projectile = Instantiate(Bullet, FirePoint.position, Quaternion.identity) as GameObject;
            projectile.tag = "PlayerBullet";
            projectile.GetComponent<Rigidbody>().linearVelocity = (pDestination - FirePoint.position).normalized * projectileSpeed;
        } else if(CurrentWeapon == 3) {
            projectile = Instantiate(Pellet, FirePoint.position, transform.rotation) as GameObject;
            projectile.tag = "PlayerPellet";
            projectile.GetComponent<Rigidbody>().linearVelocity = (pDestination - FirePoint.position).normalized * projectileSpeed;
        } else if(CurrentWeapon == 4) {
            projectile = Instantiate(LaunchedGrenade, FirePoint.position, transform.rotation) as GameObject;
            projectile.tag = "PlayerGrenade";
            projectile.GetComponent<Rigidbody>().linearVelocity = (pDestination - FirePoint.position).normalized * projectileSpeed;
        }

        ammoClip -= 1;
        ammoNum.text = "" + ammoClip + "\n-----\n" + ammoAmount;
    }

    void ReloadWeapon(){
        if(ammoAmount == 0){
            return;
        } 
        
        int toReload = ClipAmount - ammoClip;
        if(ammoAmount < toReload){
            ammoClip += ammoAmount;
            ammoAmount -= ammoAmount;
            ammoNum.text = "" + ammoClip + "\n-----\n" + ammoAmount;
        } else {
            ammoClip += toReload;
            ammoAmount -= toReload;
            ammoNum.text = "" + ammoClip + "\n-----\n" + ammoAmount;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("EnemyBullet")){
            if(armor > 0){
                health -= 5;
                armor -= 10;
                if(armor < 0){
                    armor = 0;
                }
                armorNum.text = "" + armor;
            } else {
                health -= 10;
            }
            healthNum.text = "" + health;
            Destroy(collision.gameObject);
        }

        if(health <= 0){
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("HealthPickup") && health < 100){
            health += 10;
            if(health > 100){
                health = 100;
            }
            healthNum.text = "" + health;
            Destroy(other.gameObject);
        } else if(other.gameObject.CompareTag("ArmorPickup") && armor < 100){
            armor += 25;
            if(armor > 100){
                armor = 100;
            }
            armorNum.text = "" + armor;
            Destroy(other.gameObject);
        } else if(other.gameObject.CompareTag("RifleAmmoPickup")){
            if(CurrentWeapon == 2){
                ammoAmount += 30;
                ammoNum.text = "" + ammoClip + "\n-----\n" + ammoAmount;
            } else {
                rifleAmmo += 30;
            }
            Destroy(other.gameObject);
        } else if(other.gameObject.CompareTag("ShotgunAmmoPickup")){
            if(CurrentWeapon == 3){
                ammoAmount += 6;
                ammoNum.text = "" + ammoClip + "\n-----\n" + ammoAmount;
            } else {
                shotgunAmmo += 6;
            }
            Destroy(other.gameObject);
        } else if(other.gameObject.CompareTag("ThrownGrenadePickup")){
            ThrowGrenades += 1;
            if(CurrentItem == 1){
                itemNum.text = "Grenades" + "\n-----\n" + ThrowGrenades;
            }
            Destroy(other.gameObject);
        } else if(other.gameObject.CompareTag("ThermitePickup")){
            Thermites += 1;
            if(CurrentItem == 2){
                itemNum.text = "Thermites" + "\n-----\n" + Thermites;
            }
            Destroy(other.gameObject);
        } else if(other.gameObject.CompareTag("LaunchedGrenadePickup")){
            if(CurrentWeapon == 4){
                ammoAmount += 2;
                ammoNum.text = "" + ammoClip + "\n-----\n" + ammoAmount;
            } else {
                launcherAmmo += 2;
            }
            Destroy(other.gameObject);
        }
    }
}
