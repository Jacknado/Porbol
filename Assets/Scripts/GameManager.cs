using System;
using System.Collections;
using System.Threading;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public GameObject nextLevel;
    private GameObject obstacleFolder;
    private GameObject player;
    private FadeController fadeController;
    private TextMeshProUGUI highScoreText;
    public bool isDead;
    public bool isMainMenu;
    private int deathCount = 0;
    
    private bool isRespawning = false;
    void Start()
    {
        player = transform.GetChild(0).gameObject;
        fadeController = transform.parent.Find("Canvas").GetComponent<FadeController>();
        obstacleFolder = transform.parent.Find("ObstacleFolder").gameObject;
        highScoreText = transform.parent.Find("Canvas").Find("HighScoreText").GetComponent<TextMeshProUGUI>();
        if (isMainMenu)
        {
            fadeController.MainMenuFade();
        }
        else
        {
            fadeController.StartLevel();
        }
    }
    
    void Update()
    {
        highScoreText.text = "Score\n" + MathF.Round(player.transform.position.x / 3) + "/100";

        if (isDead)
        {
            if (!isRespawning)
            {
                StartCoroutine(Respawn());
            }
        }
        if (player.transform.position.x > 300)
        {
            if(!isMainMenu)
            {
                StartCoroutine(NextLevel());
            }
        }
    }

    IEnumerator Respawn()
    {
        isRespawning = true;
        fadeController.FadeToBlack();
        yield return new WaitForSeconds(1.5f);


        deathCount += 1;
        player.transform.position = new Vector3(0, 0, 0);
        player.GetComponent<WaveTrailSmooth>().RemoveTrail();
        player.GetComponent<ExplosionPowerup>().Explode(player.transform.position);
        player.GetComponent<MistyStep>().Disable();
        foreach (Transform child in obstacleFolder.transform)
        {
            child.gameObject.SetActive(true);
        }

        foreach (GameObject child in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            if (child.name == "FastEnemy(Clone)" || child.name == "SlowEnemy(Clone)")
            {
                Destroy(child);
            }
        }
        // GameObject.Find("Audio Source").GetComponent<Music>().time = 0;
        // GameObject.Find("Audio Source").GetComponent<Music>().frequency = 440;
        fadeController.FadeFromBlack();
        isDead = false;
        yield return new WaitForSeconds(1);
        
        isRespawning = false;
    }
    IEnumerator NextLevel()
    {
        fadeController.FadeToBlack();
        yield return new WaitForSeconds(1.5f);
        nextLevel.GetComponent<TextMeshProUGUI>().text = $"CONGRATULATUIONS \n YOU BEAT MY LEVEL \n YOU TOOK {deathCount + 1} ATEMPTS";
        nextLevel.SetActive(true);
    }

    public void StartGame()
    {
        fadeController.transform.Find("Name").gameObject.SetActive(false);
        StartCoroutine(ButtonStartGame());
    }
    IEnumerator ButtonStartGame()
    {
        fadeController.FadeToBlack();
        yield return new WaitForSeconds(1f);
    }
    public void Continue()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Load the scene with the next build index
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}
