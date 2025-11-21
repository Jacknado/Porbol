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
    public void ShowTut()
    {
        fadeController.transform.Find("Name").gameObject.SetActive(false);
        transform.parent.Find("Canvas (1)").Find("HowToPlay").gameObject.SetActive(true);
        transform.parent.Find("Canvas (1)").Find("Button").gameObject.SetActive(false);
        transform.parent.Find("Canvas (1)").GetComponent<Canvas>().sortingOrder = 100;
        fadeController.FadeToBlack();   
    }
    public void StartGame()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}
