using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject nextLevel;
    public GameObject levelDropdown;
    public bool isMainMenu;
    public bool isDead;
    public int coins;

    private GameObject obstacleFolder;
    private GameObject player;
    private FadeController fadeController;
    private TextMeshProUGUI highScoreText;
    private TextMeshProUGUI coinText;
    private int deathCount = 0;
    private bool isRespawning = false;
    private bool beatLevel = false;
    private GameObject bigWave;
    private Vector3 bigWaveStartingPos;
    
    void Start()
    {
        player = transform.GetChild(0).gameObject;
        fadeController = transform.parent.Find("Canvas").GetComponent<FadeController>();
        obstacleFolder = transform.parent.Find("ObstacleFolder").gameObject;
        highScoreText = transform.parent.Find("Canvas").Find("HighScoreText").GetComponent<TextMeshProUGUI>();
        coinText = transform.parent.Find("Canvas").Find("CoinText").GetComponent<TextMeshProUGUI>();
        bigWave = transform.parent.Find("Wave").GetChild(0).gameObject;
        
        if (isMainMenu)
        {
            fadeController.MainMenuFade();
        }
        else
        {
            fadeController.StartLevel();
            bigWaveStartingPos = bigWave.transform.position;
        }
    }
    
    void Update()
    {
        if (highScoreText != null && player != null)
        {
            highScoreText.text = "Score " + Mathf.Round(player.transform.position.x / 3) + "/100";
        }
        if (coinText != null && player != null)
        {
            coinText.text = "Coins " + coins;
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
        coins = 0;
        deathCount += 1;
        player.transform.position = Vector3.zero;
        player.GetComponent<WaveTrailSmooth>().RemoveTrail();
        player.GetComponent<ExplosionPowerup>().Explode(player.transform.position, true);
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
        bigWave.transform.position = bigWaveStartingPos;
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
                nextLevelText.text = $"CONGRATULATIONS\nYOU BEAT THE LEVEL\nYOU TOOK {deathCount + 1} ATTEMPTS\nYOU COLLECTED {coins} COINS";
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
        if (currentSceneIndex == 7) {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
    }

    public void GoToLevel()
    {
        char levelToGoTo = levelDropdown.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text.Last();
        int intValue = (int)char.GetNumericValue(levelToGoTo);
        SceneManager.LoadScene(intValue);
    }
}