using Unity.VisualScripting;
using UnityEngine;

public class DieOnCollision : MonoBehaviour
{

    GameManager gameManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        if(!collision.gameObject.CompareTag("Border"))
        {
            gameManager.isDead = true;
        }
    }
}
