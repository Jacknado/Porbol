using UnityEngine;

public class GameManager : MonoBehaviour
{
    GameObject player;
    public bool isDead;
    private int deathCount = 0;
    // Update is called once per frame
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
    }
}
