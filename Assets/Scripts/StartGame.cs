using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public void Start()
    {
        
    }
    public void LoadNextScene()
    {
        // Get the build index of the currently active scene
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Load the scene with the next build index
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}
