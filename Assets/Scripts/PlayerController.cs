using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float zMinimum = -15f;
    public float zMaximum = 15f;
    public float sidewaysSpeed = 10f;
    public float forwardSpeed = 5f;
    
    public bool hasExplosion;
    public bool hasStep;
    public ParticleSystem deathEffect;
    private GameManager gameManager;
    private ShieldPowerup shieldPowerup;
    private ExplosionPowerup explosionPowerup;
    private MistyStep mistyStep;
    private GameObject explosionIndicator;
    private GameObject meleeRange;
    private float cooldown = 0;
    void Start()
    {
        shieldPowerup = gameObject.GetComponent<ShieldPowerup>();
        explosionPowerup = gameObject.GetComponent<ExplosionPowerup>();
        mistyStep = gameObject.GetComponent<MistyStep>();
        meleeRange = transform.Find("MeleeShower").GetChild(0).gameObject;
        explosionIndicator = transform.parent.parent.Find("Canvas").Find("ExplosionIndicator").gameObject;
        gameManager = transform.parent.GetComponent<GameManager>();
    }

    void Update()
    {
        if(gameManager.isDead)
            return;
        // Player movement
        float horizontal = Input.GetAxis("Horizontal");
        float moveZ = -horizontal * sidewaysSpeed * Time.deltaTime;
        Vector3 newPosition = transform.position + new Vector3(0, 0, moveZ);

        if (newPosition.z >= zMinimum && newPosition.z <= zMaximum)
            transform.Translate(forwardSpeed * Time.deltaTime, 0, moveZ, Space.World);
        else
            transform.Translate(forwardSpeed * Time.deltaTime, 0, 0, Space.World);
        if (Input.GetKeyDown(KeyCode.Q) && hasExplosion)
            explosionPowerup.Explode(transform.position);
        else if (Input.GetKeyDown(KeyCode.E) && hasStep)
            mistyStep.Step(transform.position);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Border") && !collision.gameObject.name.EndsWith("Powerup"))
        {
            gameManager.isDead = true;
            deathEffect.time = 0;
            deathEffect.Play();
        }

        if (collision.gameObject.name == "ShieldPowerup")
        {
            shieldPowerup.Enable();
            collision.gameObject.SetActive(false);
        }
        if (collision.gameObject.name == "ExplosionPowerup")
        {
            hasExplosion = true;
            
            explosionIndicator.SetActive(true);
            collision.gameObject.SetActive(false);
        }  
        if (collision.gameObject.name == "MistyStepPowerup")
        {    
            mistyStep.Enable();
            hasStep = true;
            collision.gameObject.SetActive(false);
        }  
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name.StartsWith("FastEnemy"))
        {
            //Debug.Log("staying");
            if (Input.GetMouseButtonDown(0) && cooldown == 0)
            {
                Destroy(other.gameObject);
                meleeCooldown();
            }
        }
    }

    private IEnumerator meleeCooldown()
    {
        cooldown = 1.5f;
        yield return new WaitForSeconds(cooldown);
        cooldown = 0;
    }
}