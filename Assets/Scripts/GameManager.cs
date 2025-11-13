using System.Collections;
using System.Threading;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private GameObject player;
    private FadeController fadeController;
    public bool isDead;
    private int deathCount = 0;
    
    private bool isRespawning = false;
    void Start()
    {
        player = transform.GetChild(0).gameObject;
        fadeController = transform.parent.Find("Canvas").GetComponent<FadeController>();
        fadeController.StartLevel();
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
        if (player.transform.position.x > 300)
        {
            StartCoroutine(NextLevel());
        }
    }

    IEnumerator Respawn()
    {
        isRespawning = true;
        fadeController.FadeToBlack();
        yield return new WaitForSeconds(1.5f);
        deathCount += 1;
        player.transform.position = new Vector3(0, 0, 0);
        fadeController.FadeFromBlack();
        yield return new WaitForSeconds(1);
        isDead = false;
        isRespawning = false;
    }
    IEnumerator NextLevel()
    {
        fadeController.FadeToBlack();
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
