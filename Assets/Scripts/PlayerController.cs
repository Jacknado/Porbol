using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float zMinimum = -15f;
    public float zMaximum = 15f;
    public float sidewaysSpeed = 10f;
    public float forwardSpeed = 5f;
    public bool hasExplosion;
    private GameManager gameManager;
    private ShieldPowerup shieldPowerup;
    private ExplosionPowerup explosionPowerup;
    private GameObject explosionIndicator;
    void Start()
    {
        shieldPowerup = gameObject.GetComponent<ShieldPowerup>();
        explosionPowerup = gameObject.GetComponent<ExplosionPowerup>();
        explosionIndicator = transform.parent.parent.Find("Canvas").Find("ExplosionIndicator").gameObject;
        gameManager = transform.parent.GetComponent<GameManager>();
    }

    void Update()
    {
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
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Border") && !collision.gameObject.name.EndsWith("Powerup"))
        {
            gameManager.isDead = true;
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
    }
}
