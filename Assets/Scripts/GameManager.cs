using System.Collections;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private GameObject player;
    public bool isDead;
    private int deathCount = 0;
    public FadeController fadeController;
    private bool isRespawning = false;
    void Start()
    {
        fadeController.FadeFromBlack();
        player = transform.GetChild(0).gameObject;
    }
    
    void Update()
    {
        if (isDead)
        {
            if (!isRespawning)
            {
                StartCoroutine(Respawn());
            }
        }
        // if (player.transform.position.x > 30)
        // {
        //     SceneManager.LoadScene();
        // }
    }
    
    IEnumerator Respawn()
    {
        isRespawning = true;
        fadeController.FadeToBlack();
        yield return new WaitForSeconds(1);
        deathCount += 1;
        player.transform.position = new Vector3(0, 0, 0);
        fadeController.FadeFromBlack();
        yield return new WaitForSeconds(1);
        isDead = false;
        isRespawning = false;
    }
}
