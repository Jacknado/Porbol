using TMPro;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class LevelSelector : MonoBehaviour
{
    // LevelSelector must be enabled when game complete. Coin count must be set to level currently selected.
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(GameObject.Find("GameProgress").GetComponent<GameProgress>().gameFinished)
        {
            GameObject.Find("LevelSelector").SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateCoins(int value)
    {
        GameObject progress = GameObject.Find("GameProgress");
        Levels level = progress.GetComponent<GameProgress>().levels[progress.GetComponent<GameProgress>().levels.FindIndex(x => x.index == value)];
        GameObject.Find("CoinCount").GetComponent<TextMeshProUGUI>().text = level.collectedCoins.ToString();
    }
}
