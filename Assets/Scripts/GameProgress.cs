using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Constraints;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Levels
{
    public int index;
    public int numCoins;
    public int collectedCoins;
    public bool completed;
    public int numAttempts;
}
public class GameProgress : MonoBehaviour
{
    public bool gameFinished;
    public bool replay;
    public List<Levels> levels = new List<Levels>();
    public Levels currentLevel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameFinished = false;
    }
    // Update is called once per frame
    void Update()
    {
    }
    public void StartScene(int index, int numCoins)
    {
        if(!levels.Exists(x => x.index == index))
        {
            Levels level = new Levels();
            level.index = index;
            level.numCoins = numCoins;
            currentLevel = level;
            replay = false;
        }
        else
        {
            currentLevel = levels.Find(x => x.index == index);
            replay = true;
        }
    }
    
    public void EndScene(int collectedCoins, int numAttempts)
    {
        if(replay)
        {
            currentLevel.collectedCoins = (currentLevel.collectedCoins <= collectedCoins) ? collectedCoins : currentLevel.collectedCoins;
            currentLevel.numAttempts += numAttempts;
            levels[levels.FindIndex(x => x.index == currentLevel.index)] = currentLevel;
        }
        else
        {
            currentLevel.collectedCoins = collectedCoins;
            currentLevel.numAttempts = numAttempts;
            levels.Add(currentLevel);
        }
    }
    public void GameFinished()
    {
        gameFinished = true;
    }
}
