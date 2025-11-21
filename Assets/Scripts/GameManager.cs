using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject nextLevel;
    public bool isMainMenu;
    public bool isDead;

    private GameObject obstacleFolder;
    private GameObject player;
    private FadeController fadeController;
    private TextMeshProUGUI highScoreText;
    private int deathCount = 0;
    private bool isRespawning = false;
    private bool beatLevel = false;
    
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
        if (highScoreText != null && player != null)
        {
            highScoreText.text = "Score\n" + Mathf.Round(player.transform.position.x / 3) + "/100";
        }

        if (isDead && !isRespawning)
        {
            StartCoroutine(Respawn());
        }

        if (!isMainMenu && !beatLevel && player != null && player.transform.position.x > 300)
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
        player.transform.position = Vector3.zero;
        player.GetComponent<WaveTrailSmooth>().RemoveTrail();
        player.GetComponent<ExplosionPowerup>().Explode(player.transform.position);
        player.GetComponent<MistyStep>().Disable();

        foreach (Transform child in obstacleFolder.transform)
        {
            child.gameObject.SetActive(true);
        }

        GameObject[] rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (GameObject obj in rootObjects)
        {
            if (obj.name == "FastEnemy(Clone)" || obj.name == "SlowEnemy(Clone)")
            {
                Destroy(obj);
            }
        }

        fadeController.FadeFromBlack();
        isDead = false;
        yield return new WaitForSeconds(1);
        
        isRespawning = false;
    }

    IEnumerator NextLevel()
    {
        highScoreText.gameObject.SetActive(false);
        fadeController.transform.Find("ExplosionIndicator").gameObject.SetActive(false);
        beatLevel = true;
        fadeController.FadeToBlack();
        yield return new WaitForSeconds(1.5f);
        
        if (nextLevel != null)
        {
            TextMeshProUGUI nextLevelText = nextLevel.GetComponent<TextMeshProUGUI>();
            if (nextLevelText != null)
            {
                nextLevelText.text = $"CONGRATULATIONS\nYOU BEAT MY LEVEL\nYOU TOOK {deathCount + 1} ATTEMPTS";
            }
            nextLevel.SetActive(true);
        }
    }

    public void ShowTut()
    {
        fadeController.transform.Find("Name").gameObject.SetActive(false);
        
        Transform canvas1 = transform.parent.Find("Canvas (1)");
        canvas1.Find("HowToPlay").gameObject.SetActive(true);
        canvas1.Find("Button").gameObject.SetActive(false);
        canvas1.GetComponent<Canvas>().sortingOrder = 100;
        
        fadeController.FadeToBlack();
    }

    public void StartGame()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}