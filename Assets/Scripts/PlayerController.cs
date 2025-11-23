using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float zMinimum = -15f;
    public float zMaximum = 15f;
    public float sidewaysSpeed = 10f;
    public float forwardSpeed = 5f;
    public bool hasExplosion;
    public bool hasStep;
    public GameObject deathEffect;

    private GameManager gameManager;
    private ShieldPowerup shieldPowerup;
    private ExplosionPowerup explosionPowerup;
    private MistyStep mistyStep;
    private GameObject explosionIndicator;

    void Start()
    {
        shieldPowerup = GetComponent<ShieldPowerup>();
        explosionPowerup = GetComponent<ExplosionPowerup>();
        mistyStep = GetComponent<MistyStep>();
        gameManager = transform.parent.GetComponent<GameManager>();
        
        explosionIndicator = transform.parent.parent.Find("Canvas").Find("ExplosionIndicator").gameObject;
        
        if (explosionIndicator != null)
        {
            explosionIndicator.SetActive(false);
        }
    }

    void Update()
    {
        if (gameManager != null && gameManager.isDead)
            return;

        HandleMovement();
        HandlePowerupInput();
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float moveZ = -horizontal * sidewaysSpeed * Time.deltaTime;
        Vector3 newPosition = transform.position + new Vector3(0, 0, moveZ);

        if (newPosition.z >= zMinimum && newPosition.z <= zMaximum)
        {
            transform.Translate(forwardSpeed * Time.deltaTime, 0, moveZ, Space.World);
        }
        else
        {
            transform.Translate(forwardSpeed * Time.deltaTime, 0, 0, Space.World);
        }
    }

    void HandlePowerupInput()
    {
        if (Input.GetKeyDown(KeyCode.Q) && hasExplosion && explosionPowerup != null)
        {
            explosionPowerup.Explode(transform.position);
        }
        else if (Input.GetKeyDown(KeyCode.E) && hasStep && mistyStep != null)
        {
            mistyStep.Step(transform.position);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Border") || collision.gameObject.name.Contains("WaveMesh"))
            return;

        if (collision.gameObject.name == "ShieldPowerup")
        {
            HandleShieldPickup(collision.gameObject);
        }
        else if (collision.gameObject.name == "ExplosionPowerup")
        {
            HandleExplosionPickup(collision.gameObject);
        }
        else if (collision.gameObject.name == "MistyStepPowerup")
        {
            HandleMistyStepPickup(collision.gameObject);
        }
        else if (collision.gameObject.name.Contains("Coin"))
        {
            gameManager.coins += 1;
            collision.gameObject.SetActive(false);
        }
        else
        {
            HandleDeath();
        }
    }
    void HandleShieldPickup(GameObject powerup)
    {
        if (shieldPowerup != null)
        {
            shieldPowerup.Enable();
            powerup.SetActive(false);
        }
    }

    void HandleExplosionPickup(GameObject powerup)
    {
        hasExplosion = true;
        if (explosionIndicator != null)
        {
            explosionIndicator.SetActive(true);
        }
        powerup.SetActive(false);
    }

    void HandleMistyStepPickup(GameObject powerup)
    {
        hasStep = true;
        if (mistyStep != null)
        {
            mistyStep.Enable();
        }
        powerup.SetActive(false);
    }

    void HandleDeath()
    {
        if (gameManager != null)
        {
            gameManager.isDead = true;
        }

        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, deathEffect.transform.rotation);
        }
    }
}