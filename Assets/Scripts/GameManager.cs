using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameObject player;
    public bool isDead;
    private int deathCount = 0;

    void Start()
    {
        player = transform.GetChild(0).gameObject;
    }
    
    void Update()
    {
        if (isDead)
        {
            Respawn();
        }
    }
    
    void Respawn()
    {
        deathCount += 1;
        if (isDead)
        {
            player.transform.position = new Vector3(0, 0, 0);
        }
        isDead = false;
    }
}
