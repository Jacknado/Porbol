using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float zMinimum = -15f;
    public float zMaximum = 15f;
    public float sidewaysSpeed = 10f;
    public float forwardSpeed = 5f;

    private GameManager gameManager;
    private ShieldPowerup shieldPowerup;
    void Start()
    {
        shieldPowerup = gameObject.GetComponent<ShieldPowerup>();
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
            Destroy(collision.gameObject);
        }  
    }
}
