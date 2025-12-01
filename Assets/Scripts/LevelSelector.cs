using System.Linq;
using TMPro;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class LevelSelector : MonoBehaviour
{
    public GameObject levelSelector;
    // LevelSelector must be enabled when game complete. Coin count must be set to level currently selected.
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(GameObject.Find("GameProgress").GetComponent<GameProgress>().gameFinished)
        {
            levelSelector.SetActive(true);
            UpdateCoins();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateCoins()
    {
        char levelToGoTo = levelSelector.transform.Find("Dropdown").transform.GetChild(0).GetComponent<TextMeshProUGUI>().text.Last();
        int intValue = (int)char.GetNumericValue(levelToGoTo);
        GameObject progress = GameObject.Find("GameProgress");
        Levels level = progress.GetComponent<GameProgress>().levels[progress.GetComponent<GameProgress>().levels.FindIndex(x => x.index == intValue)];
        GameObject.Find("CoinCount").GetComponent<TextMeshProUGUI>().text = $"Coins Collected: {level.collectedCoins.ToString()}";
    }
}
