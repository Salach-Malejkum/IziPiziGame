using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    public float walkSpeed = 4f;
    public float runSpeed = 6f;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;
    public int killCounter = 0;
    public int lives = 3;
    private bool gotHit = false;
    public float delayBetweenLostHp = 2f;
    public Camera playerCamera;
    public GameObject melee;
    public GameObject ranged;
    private AudioSource audio_source;
    public GameObject life1;
    public GameObject life2;
    public GameObject life3;
    public GameObject lostScreen;
    public GameObject playerUI;
    public bool isTutorial;

    public int weaponDamage = MELEEDAMAGE;
    public string weapon = "Melee";
    public float range = MELEERANGE;

    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;
    float speed = 4f;
    float gravity = 2*9.8f;
    float vSpeed = 0f; // current vertical velocity
    float jumpSpeed = 8f;

    CharacterController characterController;

    [SerializeField]
    public AudioClip[] damageClips;
    [SerializeField]
    public AudioClip[] introClips;
    [SerializeField]
    public AudioClip[] swordClips;
    [SerializeField]
    public AudioClip[] gunClips;
    public bool playIntro = true;

    void Start()
    {   
        //Makes it invisable
        Cursor.visible = false;
        //Locks the mouse in place
        Cursor.lockState = CursorLockMode.Locked;
        // In unity press esc to unlock the mouse and unhide it
        characterController = GetComponent<CharacterController>();
        // Equip melee weapon
        melee.GetComponent<MeshRenderer>().enabled = true;
        audio_source = GetComponent<AudioSource>();

        if (playIntro)
        {
            int pickedClip = Random.Range(0, introClips.Length - 1);
            //Debug.Log(pickedClip);
            audio_source.PlayOneShot(introClips[pickedClip]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Attack();
        IsLost();
    }

    void Movement()
    {
        speed = walkSpeed;

        // Press Left Shift to run
        if (Input.GetKey("left shift"))
        {
            speed = runSpeed;
        }

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        Vector3 vel = transform.forward * Input.GetAxis("Vertical") * speed;
        if (characterController.isGrounded)
        {
            vSpeed = 0; // grounded character has vSpeed = 0...
            if (Input.GetKeyDown("space"))
            { // unless it jumps:
                vSpeed = jumpSpeed;
            }
        }
        // apply gravity acceleration to vertical speed:
        vSpeed -= gravity * Time.deltaTime;
        vel.y = vSpeed; // include vertical speed in vel
                        // convert vel to displacement and Move the character:
        characterController.Move(vel * Time.deltaTime);

        float curSpeedX = speed * Input.GetAxis("Vertical");
        float curSpeedY = speed * Input.GetAxis("Horizontal");
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        // Player and Camera rotation
        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
    }

    void Attack()
    {
        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }

        if (weapon == "Melee") {
            int pickedClip = Random.Range(0, swordClips.Length - 1);
            melee.transform.GetComponent<AudioSource>().PlayOneShot(swordClips[pickedClip]);
        }
        else if (weapon == "Ranged") {
            int pickedClip = Random.Range(0, gunClips.Length - 1);
            ranged.transform.GetComponent<AudioSource>().PlayOneShot(gunClips[pickedClip]);
        }

        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 7;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, range, layerMask))
        {

            if (hit.transform.gameObject.CompareTag("Enemy"))
            {
                EnemyAI target = hit.transform.GetComponent<EnemyAI>();
                target.TakeDamage(weaponDamage);
            }     
        }
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.transform.gameObject.CompareTag("Weapon"))
        {
            //equip weapon
            switch(coll.transform.gameObject.name)
            {
                case var s when coll.transform.gameObject.name.Contains("Melee"):
                    range = MELEERANGE;
                    weaponDamage = MELEEDAMAGE;
                    weapon = "Melee";
                    ranged.GetComponent<MeshRenderer>().enabled = false;
                    melee.GetComponent<MeshRenderer>().enabled = true;
                    break;
                case var s when coll.transform.gameObject.name.Contains("Ranged"):
                    range = RANGEDRANGE;
                    weaponDamage = RANGEDDAMAGE;
                    weapon = "Ranged";
                    melee.GetComponent<MeshRenderer>().enabled = false;
                    ranged.GetComponent<MeshRenderer>().enabled = true;
                    break;
                case "RPG":
                    range = RPGRANGE;
                    weaponDamage = RPGDAMAGE;
                    break;
            }
            Destroy(coll.transform.gameObject);
        }
    }

    public void LoseHP()
    {
        if (!gotHit && !isTutorial)
        {
            int pickedClip = Random.Range(0, damageClips.Length - 1);
            //Debug.Log(pickedClip);
            audio_source.PlayOneShot(damageClips[pickedClip]);

            gotHit = true;
            lives -= 1;
            switch (lives)
            {
                case 2:
                    life3.GetComponent<Animator>().Play("Life3");
                    Destroy(life3, 1.0f);
                    break;
                case 1:
                    life2.GetComponent<Animator>().Play("Life2");
                    Destroy(life2, 1.0f);
                    break;
                case 0:
                    life1.GetComponent<Animator>().Play("Life1");
                    Destroy(life1, 1.0f);
                    break;
            }
            Invoke(nameof(ResetHpCountdown), delayBetweenLostHp);
        }
    }

    void ResetHpCountdown()
    {
        gotHit = false;
    }

    IEnumerator LostGame()
    {
        playerUI.SetActive(false);
        lostScreen.SetActive(true);
        audio_source.mute = true;
        yield return new WaitForSeconds(5.0f);
        SceneManager.LoadScene("MainMenu"); 
        //Makes it invisable
        Cursor.visible = true;
        //Locks the mouse in place
        Cursor.lockState = CursorLockMode.None;
    }

    void IsLost()
    {
        if (life1 == null && !isTutorial)
        {
            StartCoroutine(LostGame());
        }
    }
}
